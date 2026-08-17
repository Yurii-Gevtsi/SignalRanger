#!/usr/bin/env python3
"""Prepare clean RGBA visuals and matching collision masks from cluster renders."""

from __future__ import annotations

from pathlib import Path
import shutil

import numpy as np
from PIL import Image, ImageFilter


ROOT = Path(__file__).resolve().parents[2]
OUTPUT = ROOT / "art_source/arenas/desert_sci_fi/production_v1"
GENERATED = Path(r"C:\Users\Yurii\AppData\Roaming\orca\codex-runtime-home\home\generated_images\019ffba4-9810-7002-9105-09acc5d77066")

RENDERS = {
    "upper_left": "exec-cced8a6c-d0d0-4568-baef-bbd745269aa9.png",
    "inner_left": "exec-440dd98f-f7af-4cfd-9c86-56eab71cdd3f.png",
    "lower_left": "exec-5e440ce7-f732-4de4-ac15-320684e71e4d.png",
    "center": "exec-4317f6f3-0250-4269-86a7-0715ddc9876b.png",
    "floating_large": "exec-cc99b40d-7395-491a-a20d-e1641eeef34f.png",
    "floating_small": "exec-48e8a7c3-e829-47df-816b-98cad3a60fd6.png",
    "upper_right": "exec-c396dd21-2dfd-4cfc-90bd-76a35a1434f1.png",
    "lower_right": "exec-7462e628-6365-48af-b832-47cdfbd0dc15.png",
}


def chroma_alpha(rgb: np.ndarray) -> np.ndarray:
    key = np.array([255.0, 0.0, 240.0], dtype=np.float32)
    distance = np.linalg.norm(rgb.astype(np.float32) - key, axis=2)
    # Generated backgrounds vary slightly around vivid magenta.
    magenta = is_key_colour(rgb)
    alpha = np.where(magenta, 0, np.clip((distance - 25.0) / 35.0, 0, 1) * 255).astype(np.uint8)
    return np.asarray(Image.fromarray(alpha, "L").filter(ImageFilter.GaussianBlur(0.45)), dtype=np.uint8)


def is_key_colour(rgb: np.ndarray) -> np.ndarray:
    # The chroma key ranges from hot pink to dark purple after antialiasing.
    # Natural stone is warm (low blue) and technology accents are cyan (low red),
    # so this safely isolates only the synthetic backdrop.
    return (
        (rgb[..., 0] > 80)
        & (rgb[..., 2] > 65)
        & (rgb[..., 1] < rgb[..., 0] * 0.68)
        & (rgb[..., 1] < rgb[..., 2] * 0.72)
    )


def remove_magenta_halo(rgb: np.ndarray, alpha: np.ndarray) -> tuple[np.ndarray, np.ndarray]:
    """Suppress only magenta-key colour that survived edge resampling."""
    magenta = is_key_colour(rgb)
    cleaned = rgb.copy()
    alpha = alpha.copy()
    cleaned[magenta] = 0
    alpha[magenta] = 0
    return cleaned, alpha


def subject_bounds(alpha: np.ndarray) -> tuple[int, int, int, int]:
    ys, xs = np.nonzero(alpha > 24)
    if not len(xs):
        raise ValueError("No non-chroma subject found")
    return int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1


def resize_premultiplied(subject: np.ndarray, size: tuple[int, int]) -> np.ndarray:
    """Resize transparent art without blending the removed key colour into edges."""
    alpha = subject[..., 3].astype(np.float32) / 255.0
    rgb = subject[..., :3].astype(np.float32) * alpha[..., None]
    resized_rgb = np.stack(
        [np.asarray(Image.fromarray(rgb[..., channel].astype(np.uint8), "L").resize(size, Image.Resampling.LANCZOS))
         for channel in range(3)],
        axis=2).astype(np.float32)
    resized_alpha = np.asarray(
        Image.fromarray(subject[..., 3], "L").resize(size, Image.Resampling.LANCZOS),
        dtype=np.float32)
    alpha_factor = resized_alpha / 255.0
    output = np.zeros((size[1], size[0], 4), dtype=np.uint8)
    nonzero = alpha_factor > 0.02
    output[..., :3][nonzero] = np.clip(
        resized_rgb[nonzero] / alpha_factor[nonzero, None], 0, 255).astype(np.uint8)
    output[..., 3] = np.where(alpha_factor >= 0.25, 255, 0).astype(np.uint8)
    return output


def mask_bounds(mask: Image.Image) -> tuple[int, int, int, int]:
    values = np.asarray(mask)
    ys, xs = np.nonzero(values > 0)
    return int(xs.min()), int(ys.min()), int(xs.max()) + 1, int(ys.max()) + 1


def fit_render(render_path: Path, template_mask_path: Path, visual_path: Path, mask_path: Path) -> None:
    source = Image.open(render_path).convert("RGB")
    source_rgb = np.asarray(source)
    source_alpha = chroma_alpha(source_rgb)
    source_rgb, source_alpha = remove_magenta_halo(source_rgb, source_alpha)
    source_alpha = np.where(source_alpha >= 128, 255, 0).astype(np.uint8)
    source_rgb[source_alpha == 0] = 0
    sx0, sy0, sx1, sy1 = subject_bounds(source_alpha)
    subject = np.dstack((source_rgb, source_alpha))[sy0:sy1, sx0:sx1]

    # The original master masks define stable texture dimensions and placement
    # padding, but must not be used as the final silhouette. The independently
    # rendered clusters have their own cutouts and negative spaces.
    template_mask = Image.open(template_mask_path).convert("L")
    tx0, ty0, tx1, ty1 = mask_bounds(template_mask)
    fitted = Image.fromarray(resize_premultiplied(subject, (tx1 - tx0, ty1 - ty0)), "RGBA")
    canvas = Image.new("RGBA", template_mask.size, (0, 0, 0, 0))
    canvas.alpha_composite(fitted, (tx0, ty0))

    rgba = np.asarray(canvas).copy()
    residual_magenta = is_key_colour(rgba[..., :3])
    rgba[residual_magenta] = 0
    # A manual template mask may deliberately remove a rendered support or
    # decoration. Intersect it with the real RGBA silhouette: neither source
    # can create opaque black regions, while both visual and collision honour
    # deliberate terrain cutouts.
    template_alpha = np.asarray(template_mask)
    rgba[..., 3] = np.where(
        (rgba[..., 3] >= 32) & (template_alpha >= 128),
        255,
        0).astype(np.uint8)
    rgba = dilate_transparent_rgb(rgba)
    Image.fromarray(rgba, "RGBA").save(visual_path)
    Image.fromarray(rgba[..., 3], "L").save(mask_path)


def dilate_transparent_rgb(rgba: np.ndarray, radius: int = 12) -> np.ndarray:
    """Bleed subject colour into transparent padding for bilinear sampling.

    Alpha remains untouched and collision still reads the separate binary mask.
    This avoids dark rectangular seams where adjacent runtime fragments overlap.
    """
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


def main() -> None:
    candidates = OUTPUT / "source_layers/production_renders"
    candidates.mkdir(parents=True, exist_ok=True)
    mask_templates = OUTPUT / "source_layers/layout_mask_templates"
    mask_templates.mkdir(parents=True, exist_ok=True)
    for asset_id, generated_name in RENDERS.items():
        source = GENERATED / generated_name
        snapshot = candidates / f"arena_desert_sci_fi_{asset_id}_01_chroma_source.png"
        if not snapshot.exists():
            snapshot.write_bytes(source.read_bytes())
        stem = f"arena_desert_sci_fi_{asset_id}_01"
        template_mask = mask_templates / f"{stem}_mask.png"
        if not template_mask.exists():
            shutil.copy2(OUTPUT / "masks" / f"{stem}_mask.png", template_mask)
        fit_render(
            snapshot,
            template_mask,
            OUTPUT / "visuals" / f"{stem}_visual.png",
            OUTPUT / "masks" / f"{stem}_mask.png",
        )


if __name__ == "__main__":
    main()
