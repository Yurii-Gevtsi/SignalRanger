#!/usr/bin/env python3
"""Decompose the supplied forest scene into three Unity arena pieces.

The supplied source is kept untouched. The scene is fitted to the project's
2048x1152 arena master and split into overlapping left, center, and right
visual/mask pairs for the existing arena preparation tool.
"""

from __future__ import annotations

import json
from pathlib import Path

import numpy as np
from PIL import Image


ROOT = Path(__file__).resolve().parents[2]
SOURCE = Path(r"C:\Users\Yurii\Desktop\Game\UI\Arenas\Scenes\forest.png")
OUTPUT = ROOT / "tools/arena-assets/forest"
SOURCE_OUTPUT = ROOT / "art_source/arenas/forest/production_source"
GENERATED_CANOPY = SOURCE_OUTPUT / "forest_right_canopy_generated.png"
MASTER_SIZE = (2048, 1152)
PADDING = 24

REGIONS = (
    ("left", (0, 255, 790, 1152), -7),
    ("center", (650, 650, 1435, 1152), -8),
    ("right", (1190, 0, 2048, 1152), -7),
)


def resize_premultiplied(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    rgba = np.asarray(image.convert("RGBA"), dtype=np.float32) / 255.0
    alpha = rgba[..., 3:4]
    premultiplied = np.concatenate((rgba[..., :3] * alpha, alpha), axis=2)
    resized = np.asarray(
        Image.fromarray(np.clip(premultiplied * 255.0, 0, 255).astype(np.uint8), "RGBA").resize(
            size, Image.Resampling.LANCZOS
        ),
        dtype=np.float32,
    ) / 255.0
    resized_alpha = resized[..., 3:4]
    safe_alpha = np.maximum(resized_alpha, 1.0 / 255.0)
    rgb = np.where(resized_alpha > 0, resized[..., :3] / safe_alpha, 0)
    return Image.fromarray(np.clip(np.concatenate((rgb, resized_alpha), axis=2) * 255.0, 0, 255).astype(np.uint8), "RGBA")


def tight_crop(image: Image.Image, box: tuple[int, int, int, int]) -> tuple[Image.Image, Image.Image, tuple[int, int, int, int]]:
    rgba = np.asarray(image.convert("RGBA"), dtype=np.uint8)
    x0, y0, x1, y1 = box
    alpha = rgba[y0:y1, x0:x1, 3]
    ys, xs = np.nonzero(alpha > 8)
    if not len(xs):
        raise ValueError(f"empty forest region: {box}")
    crop_x0 = max(0, x0 + int(xs.min()) - PADDING)
    crop_y0 = max(0, y0 + int(ys.min()) - PADDING)
    crop_x1 = min(image.width, x0 + int(xs.max()) + 1 + PADDING)
    crop_y1 = min(image.height, y0 + int(ys.max()) + 1 + PADDING)
    visual = image.crop((crop_x0, crop_y0, crop_x1, crop_y1)).convert("RGBA")
    mask = Image.fromarray((np.asarray(visual)[..., 3] >= 128).astype(np.uint8) * 255, "L")
    return visual, mask, (crop_x0, crop_y0, crop_x1, crop_y1)


def clear_checkerboard_background(image: Image.Image) -> Image.Image:
    """Turn the generator's baked light checkerboard into a clean RGBA cutout."""
    rgb = np.asarray(image.convert("RGB"), dtype=np.int16)
    near_white = np.max(np.abs(rgb - 255), axis=2) <= 20
    near_light_gray = np.max(np.abs(rgb - 238), axis=2) <= 20
    gray = np.max(rgb, axis=2) - np.min(rgb, axis=2) <= 8
    # The generator used a flat white/light-gray checkerboard. Removing every
    # matching tile also clears enclosed cells between branches.
    background_candidate = near_white | near_light_gray | (gray & (np.min(rgb, axis=2) >= 220))
    alpha = np.where(background_candidate, 0, 255).astype(np.uint8)
    rgb[alpha == 0] = 0
    return Image.fromarray(np.dstack((rgb.astype(np.uint8), alpha)), "RGBA")


def generate_right_tree(visual: Image.Image) -> Image.Image:
    """Use the cohesive generated tree rather than a visibly separate crown patch."""
    if not GENERATED_CANOPY.exists():
        raise FileNotFoundError(f"missing generated canopy source: {GENERATED_CANOPY}")

    generated = clear_checkerboard_background(Image.open(GENERATED_CANOPY))
    generated = resize_premultiplied(generated, visual.size)
    generated_pixels = np.asarray(generated, dtype=np.uint8).copy()
    generated_pixels[generated_pixels[..., 3] == 0, :3] = 0
    return Image.fromarray(generated_pixels, "RGBA")


def save_ground_extension(source_dir: Path, master: Image.Image) -> None:
    """Build the widening strip from the source arena's own grass and rock.

    This deliberately avoids an AI-painted fill: the strip overlaps behind the
    original pieces, so its colours, outlines and terrain texture stay native
    to the source at both joins.
    """
    # A small uninterrupted rock-and-grass run, repeated at native detail.
    # Keeping the fill deliberately plain leaves the original tree stumps,
    # crystals and centre mound as the arena's only large landmarks.
    source_band = master.crop((650, 940, 800, 1088)).convert("RGBA")
    source_band = resize_premultiplied(source_band, (150, 175))
    band_pixels = np.asarray(source_band, dtype=np.float32).copy()
    # Wrap the last pixels into the first ones before tiling, eliminating the
    # vertical square seams that would otherwise appear every 150 pixels.
    blend_width = 28
    original_pixels = band_pixels.copy()
    for i in range(blend_width):
        blend = (i + 1) / blend_width
        band_pixels[-blend_width + i] = (
            original_pixels[-blend_width + i] * (1.0 - blend)
            + original_pixels[i] * blend
        )
    source_band = Image.fromarray(np.clip(band_pixels, 0, 255).astype(np.uint8), "RGBA")
    visual = Image.new("RGBA", (1672, 175), (0, 0, 0, 0))
    for x in range(0, visual.width, source_band.width):
        visual.alpha_composite(source_band, (x, 0))
    pixels = np.asarray(visual, dtype=np.uint8).copy()
    pixels[pixels[..., 3] == 0, :3] = 0
    visual = Image.fromarray(pixels, "RGBA")
    mask = Image.fromarray((pixels[..., 3] >= 128).astype(np.uint8) * 255, "L")
    visual.save(source_dir / "forest_extension_visual.png")
    mask.save(source_dir / "forest_extension_mask.png")


def main() -> None:
    OUTPUT.mkdir(parents=True, exist_ok=True)
    source = Image.open(SOURCE).convert("RGBA")
    # Preserve the complete source composition while conforming to the 16:9 master.
    scale = MASTER_SIZE[0] / source.width
    fitted = resize_premultiplied(source, (MASTER_SIZE[0], round(source.height * scale)))
    master = Image.new("RGBA", MASTER_SIZE, (0, 0, 0, 0))
    top = (MASTER_SIZE[1] - fitted.height) // 2
    master.alpha_composite(fitted, (0, top))
    source_dir = SOURCE_OUTPUT
    source_dir.mkdir(parents=True, exist_ok=True)
    master.save(source_dir / "layout_master.png")
    save_ground_extension(source_dir, master)

    rows = []
    preview = Image.new("RGBA", MASTER_SIZE, (0, 0, 0, 0))
    for piece_id, source_box, sorting_order in REGIONS:
        box = (source_box[0], source_box[1], source_box[2], source_box[3])
        visual, mask, crop = tight_crop(master, box)
        visual_name = f"forest_{piece_id}_visual.png"
        mask_name = f"forest_{piece_id}_mask.png"
        visual.save(source_dir / visual_name)
        mask.save(source_dir / mask_name)
        preview.alpha_composite(visual, crop[:2])
        cx = (crop[0] + crop[2]) / 2
        cy = (crop[1] + crop[3]) / 2
        rows.append({
            "piece_id": piece_id,
            "resource_path": f"Visuals/Arenas/Forest/{piece_id}",
            "source_visual": f"source/{visual_name}",
            "source_mask": f"source/{mask_name}",
            "sorting_order": sorting_order,
            "position_x": (cx - MASTER_SIZE[0] / 2) / 256.0,
            "position_y": (MASTER_SIZE[1] / 2 - cy) / 256.0,
            "scale_x": 1.0,
            "scale_y": 1.0,
            "destructible": True,
        })

    preview.save(OUTPUT / "layout_preview.png")
    (OUTPUT / "layout.json").write_text(json.dumps({
        "arena_id": "arena.forest",
        "source_master": str(SOURCE),
        "layout_master": "source/layout_master.png",
        "master_width_px": MASTER_SIZE[0],
        "master_height_px": MASTER_SIZE[1],
        "master_pixels_per_unit": 256,
        "master_scale": 1.0,
        "master_world_position": [0.0, 0.0],
        "production_scale": 1,
        "production_canvas_width_px": MASTER_SIZE[0],
        "production_canvas_height_px": MASTER_SIZE[1],
        "runtime_pixels_per_unit": 256,
        "notes": "Three original overlapping pieces plus one source-derived ground extension behind their joins."
    }, indent=2) + "\n", encoding="utf-8")
    with (OUTPUT / "pieces.csv").open("w", newline="", encoding="utf-8") as handle:
        handle.write("piece_id,resource_path,source_visual,source_mask,sorting_order,position_x,position_y,scale_x,scale_y,destructible\n")
        for row in rows:
            handle.write(",".join([
                row["piece_id"], row["resource_path"], row["source_visual"], row["source_mask"],
                str(row["sorting_order"]), f"{row['position_x']:.5f}", f"{row['position_y']:.5f}",
                "1.00000", "1.00000", "true"
            ]) + "\n")


if __name__ == "__main__":
    main()
