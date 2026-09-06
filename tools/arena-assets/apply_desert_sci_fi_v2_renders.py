#!/usr/bin/env python3
"""Fit transparent individually enhanced renders into immutable arena masks."""

from __future__ import annotations

import csv
import shutil
import sys
from pathlib import Path

import numpy as np
from PIL import Image


ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / "art_source/arenas/desert_sci_fi/production_v2"
RUNTIME_VISUALS = ROOT / "Assets/_Game/Content/Resources/Visuals/Arenas/DesertSciFi"
REFERENCE_EDGE_GUARD_PIXELS = 4


def save_png_atomically(image: Image.Image, path: Path) -> None:
    """Avoid a partially written arena asset when a viewer observes the old PNG."""
    staged = path.with_name(f"{path.stem}.staged{path.suffix}")
    image.save(staged)
    staged.replace(path)


def subject_bounds(subject_alpha: np.ndarray) -> tuple[int, int, int, int]:
    subject = subject_alpha >= 16
    ys, xs = np.nonzero(subject)
    if not len(xs):
        raise RuntimeError("Generated render contains no visible alpha pixels.")
    return int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1


def mask_bounds(mask: np.ndarray) -> tuple[int, int, int, int]:
    ys, xs = np.nonzero(mask >= 128)
    if not len(xs):
        raise RuntimeError("Template mask contains no solid pixels.")
    return int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1


def inset_solid_mask(mask: np.ndarray, pixels: int) -> np.ndarray:
    """Keep reference RGB along the collision silhouette and crop boundaries."""
    solid = mask >= 128
    for _ in range(pixels):
        inset = np.zeros_like(solid)
        inset[1:-1, 1:-1] = (
            solid[1:-1, 1:-1]
            & solid[:-2, 1:-1]
            & solid[2:, 1:-1]
            & solid[1:-1, :-2]
            & solid[1:-1, 2:]
        )
        solid = inset
    return solid


def resize_rgb(image: np.ndarray, size: tuple[int, int]) -> np.ndarray:
    return np.asarray(
        Image.fromarray(image, "RGB").resize(size, Image.Resampling.LANCZOS),
        dtype=np.uint8,
    ).copy()


def resize_transparent_rgba(image: np.ndarray, size: tuple[int, int]) -> np.ndarray:
    """Resize premultiplied RGBA so transparent edge pixels cannot create halos."""
    alpha = image[..., 3:4].astype(np.float32) / 255.0
    premultiplied_rgb = np.rint(image[..., :3].astype(np.float32) * alpha).astype(np.uint8)
    resized_rgb = resize_rgb(premultiplied_rgb, size).astype(np.float32)
    resized_alpha = np.asarray(
        Image.fromarray(image[..., 3], "L").resize(size, Image.Resampling.LANCZOS),
        dtype=np.uint8,
    )
    result = np.zeros((*size[::-1], 4), dtype=np.uint8)
    alpha_float = resized_alpha.astype(np.float32) / 255.0
    visible = alpha_float > (1.0 / 255.0)
    result[..., 3] = resized_alpha
    result[..., :3][visible] = np.clip(
        np.rint(resized_rgb[visible] / alpha_float[visible, None]), 0, 255
    ).astype(np.uint8)
    return result


def load_transparent_render(path: Path) -> np.ndarray:
    if not path.exists():
        raise RuntimeError(f"Missing transparent generated render: {path}")
    with Image.open(path) as source:
        if "A" not in source.getbands():
            raise RuntimeError(f"Generated render has no alpha channel: {path}")
        rgba = np.asarray(source.convert("RGBA"), dtype=np.uint8)

    alpha = rgba[..., 3]
    transparent_pixels = int(np.count_nonzero(alpha == 0))
    opaque_pixels = int(np.count_nonzero(alpha > 0))
    if transparent_pixels == 0 or opaque_pixels == 0:
        raise RuntimeError(
            f"Generated render must contain both transparent background and visible art: {path}"
        )
    return rgba


def unwanted_purple_mask(rgb: np.ndarray, alpha: np.ndarray) -> np.ndarray:
    """Suppress accidental model colour artifacts without deriving alpha from colour."""
    return (
        (alpha > 0)
        & (rgb[..., 0] > 200)
        & (rgb[..., 2] > 120)
        & (rgb[..., 1] < np.minimum(rgb[..., 0], rgb[..., 2]) * 0.55)
    )


def dilate_transparent_rgb(rgba: np.ndarray, radius: int = 12) -> np.ndarray:
    output = rgba.copy()
    alpha = output[..., 3]
    known = alpha > 0
    for _ in range(radius):
        source = output.copy()
        source_known = known.copy()
        for dy, dx in ((-1, 0), (1, 0), (0, -1), (0, 1)):
            shifted_known = np.zeros_like(source_known)
            shifted_rgb = np.zeros_like(source[..., :3])
            y_source = slice(max(0, -dy), source.shape[0] - max(0, dy))
            x_source = slice(max(0, -dx), source.shape[1] - max(0, dx))
            y_target = slice(max(0, dy), source.shape[0] - max(0, -dy))
            x_target = slice(max(0, dx), source.shape[1] - max(0, -dx))
            shifted_known[y_target, x_target] = source_known[y_source, x_source]
            shifted_rgb[y_target, x_target] = source[y_source, x_source, :3]
            fill = ~known & shifted_known
            output[fill, :3] = shifted_rgb[fill]
            known |= fill
    output[..., 3] = alpha
    return output


def repair_reference_rgb(reference: np.ndarray, required_mask: np.ndarray) -> np.ndarray:
    """Replace invalid transparent-source RGB before the authoritative mask makes it visible."""
    repaired = reference.copy()
    original_alpha = repaired[..., 3].copy()
    repaired[..., 3] = np.where(original_alpha >= 16, 255, 0).astype(np.uint8)
    repaired = dilate_transparent_rgb(repaired)
    invalid_reference_rgb = (original_alpha < 16) & (required_mask >= 128)
    reference = reference.copy()
    reference[invalid_reference_rgb, :3] = repaired[invalid_reference_rgb, :3]
    return reference


def main() -> None:
    visuals = OUTPUT / "visuals"
    generated_sources = OUTPUT / "source_layers/transparent_renders"
    references = OUTPUT / "source_layers/reference_crops"
    review = OUTPUT / "review"
    for folder in (visuals, generated_sources, review):
        folder.mkdir(parents=True, exist_ok=True)

    with (OUTPUT / "arena_desert_sci_fi_pieces.csv").open(newline="", encoding="utf-8") as handle:
        rows = list(csv.DictReader(handle))

    assembled = Image.new("RGBA", (4096, 2304), (0, 0, 0, 0))
    for row in rows:
        piece_id = row["piece_id"]
        render_path = generated_sources / f"{piece_id}_transparent_source.png"
        generated = load_transparent_render(render_path)
        sx0, sy0, sx1, sy1 = subject_bounds(generated[..., 3])
        template_mask = np.asarray(Image.open(OUTPUT / row["mask_file"]).convert("L"))
        tx0, ty0, tx1, ty1 = mask_bounds(template_mask)
        reference = np.asarray(Image.open(references / f"{piece_id}_reference.png").convert("RGBA"))
        reference = repair_reference_rgb(reference, template_mask)
        generated_fit = resize_transparent_rgba(
            generated[sy0:sy1, sx0:sx1],
            (tx1 - tx0, ty1 - ty0),
        )

        rgba = reference.copy()
        reference_region = rgba[ty0:ty1, tx0:tx1, :3]
        generated_region = generated_fit[..., :3]
        generated_visible = generated_fit[..., 3] >= 16
        safe_generated_region = generated_visible & inset_solid_mask(
            template_mask[ty0:ty1, tx0:tx1],
            REFERENCE_EDGE_GUARD_PIXELS,
        )
        safe_generated_region &= ~unwanted_purple_mask(
            generated_region,
            generated_fit[..., 3],
        )
        reference_region[safe_generated_region] = generated_region[safe_generated_region]
        rgba[..., 3] = np.where(template_mask >= 128, 255, 0).astype(np.uint8)
        rgba[rgba[..., 3] == 0, :3] = 0
        rgba = dilate_transparent_rgb(rgba)
        visual_path = visuals / f"{piece_id}_visual.png"
        save_png_atomically(Image.fromarray(rgba, "RGBA"), visual_path)
        assembled.alpha_composite(Image.open(visual_path).convert("RGBA"), (int(row["x_px"]), int(row["y_px"])))

    save_png_atomically(assembled, review / "arena_desert_sci_fi_reconstructed_unique_assets.png")
    for name, color in (("dark", (20, 28, 38)), ("light", (230, 236, 240)), ("blue", (36, 135, 185))):
        background = Image.new("RGBA", assembled.size, (*color, 255))
        background.alpha_composite(assembled)
        save_png_atomically(background.convert("RGB"), review / f"arena_desert_sci_fi_reconstructed_{name}.png")

    if "--install-runtime-visuals" in sys.argv:
        for visual in sorted(visuals.glob("*_visual.png")):
            shutil.copy2(visual, RUNTIME_VISUALS / visual.name)


if __name__ == "__main__":
    main()
