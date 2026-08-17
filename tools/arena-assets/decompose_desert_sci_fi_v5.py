#!/usr/bin/env python3
"""Decompose the approved desert sci-fi v5 master into production sprite pairs.

The source master is never modified. Eight unique visual/mask pairs are written
at one shared production scale, while the small floating platform is referenced
twice by the instance manifest.
"""

from __future__ import annotations

import csv
import json
from collections import deque
from dataclasses import dataclass
from pathlib import Path

import numpy as np
from PIL import Image, ImageFilter


ROOT = Path(__file__).resolve().parents[2]
MASTER = ROOT / "art_source/arenas/desert_sci_fi/master/arena_desert_sci_fi_master_v5.png"
OUTPUT = ROOT / "art_source/arenas/desert_sci_fi/production_v1"
SCALE = 2
PADDING_MASTER = 16


@dataclass(frozen=True)
class Cluster:
    number: int
    asset_id: str
    category: str
    polygons: tuple[tuple[tuple[int, int], ...], ...]
    sorting_order: int
    notes: str


# Master-space selection regions. Neighboring terrain pieces overlap by 8-12
# master pixels (32-48 production pixels); the collision union remains solid.
CLUSTERS = (
    Cluster(1, "upper_left", "terrain", (((0, 0), (455, 0), (455, 420), (610, 420), (610, 600), (465, 600), (465, 775), (0, 775)),), 0, "Upper-left cliff, upper bridge and lift."),
    Cluster(2, "inner_left", "terrain", (((405, 360), (735, 360), (735, 960), (405, 960)),), 1, "Inner-left rock formation."),
    Cluster(3, "lower_left", "terrain", (((0, 720), (760, 720), (760, 1152), (0, 1152)),), 2, "Lower-left ground and embedded lower cover."),
    Cluster(4, "center", "terrain", (((565, 445), (1395, 445), (1395, 1152), (565, 1152)),), 3, "Central tower, attached platform, bridge, arch and lower covers."),
    Cluster(5, "floating_large", "floating_platform", (
        ((475, 220), (1045, 220), (1045, 390), (970, 390), (930, 470), (870, 470), (870, 530), (650, 530), (650, 470), (590, 470), (550, 390), (475, 390)),
    ), 5, "Unique large floating platform with full reactor body; excludes neighboring terrain and long tether ends."),
    Cluster(6, "floating_small", "floating_platform", (((1065, 335), (1390, 335), (1390, 625), (1065, 625)),), 5, "Canonical small floating platform; instantiated twice."),
    Cluster(7, "upper_right", "terrain", (((1680, 70), (2048, 70), (2048, 790), (1680, 790)),), 0, "Upper-right cliff and lift."),
    Cluster(8, "lower_right", "terrain", (((1350, 525), (2048, 525), (2048, 1152), (1350, 1152)),), 2, "Lower-right ground, entrance and embedded lower cover."),
)


def polygon_mask(size: tuple[int, int], polygons: tuple[tuple[tuple[int, int], ...], ...]) -> Image.Image:
    from PIL import ImageDraw

    result = Image.new("L", size, 0)
    draw = ImageDraw.Draw(result)
    for polygon in polygons:
        draw.polygon(polygon, fill=255)
    return result


def foreground_alpha(rgb: np.ndarray) -> np.ndarray:
    """Remove the smooth blue concept background without deleting cyan tech."""
    red = rgb[..., 0].astype(np.int16)
    green = rgb[..., 1].astype(np.int16)
    blue = rgb[..., 2].astype(np.int16)
    blue_background = (
        (blue > red + 18)
        & (blue > green + 8)
        & (red > 78)
        & (green > 105)
        & ((blue - red) < 105)
    )
    hard = Image.fromarray((~blue_background).astype(np.uint8) * 255, "L")
    # Close tiny holes in outlined machinery, then retain a soft visual edge.
    closed = hard.filter(ImageFilter.MaxFilter(7)).filter(ImageFilter.MinFilter(7))
    return np.asarray(closed.filter(ImageFilter.GaussianBlur(0.7)), dtype=np.uint8)


def keep_largest_component(alpha: np.ndarray) -> np.ndarray:
    solid = alpha >= 32
    height, width = solid.shape
    seen = np.zeros_like(solid, dtype=bool)
    largest: list[tuple[int, int]] = []
    for y in range(height):
        for x in range(width):
            if not solid[y, x] or seen[y, x]:
                continue
            queue = deque([(x, y)])
            seen[y, x] = True
            component: list[tuple[int, int]] = []
            while queue:
                cx, cy = queue.popleft()
                component.append((cx, cy))
                for nx, ny in ((cx - 1, cy), (cx + 1, cy), (cx, cy - 1), (cx, cy + 1)):
                    if 0 <= nx < width and 0 <= ny < height and solid[ny, nx] and not seen[ny, nx]:
                        seen[ny, nx] = True
                        queue.append((nx, ny))
            if len(component) > len(largest):
                largest = component
    kept = np.zeros_like(alpha)
    for x, y in largest:
        kept[y, x] = alpha[y, x]
    return np.asarray(Image.fromarray(kept, "L").filter(ImageFilter.MaxFilter(3)), dtype=np.uint8)


def tight_bounds(alpha: np.ndarray, padding: int) -> tuple[int, int, int, int]:
    ys, xs = np.nonzero(alpha > 8)
    if not len(xs):
        raise ValueError("Cluster selection produced an empty sprite")
    x0 = max(0, int(xs.min()) - padding)
    y0 = max(0, int(ys.min()) - padding)
    x1 = min(alpha.shape[1], int(xs.max()) + 1 + padding)
    y1 = min(alpha.shape[0], int(ys.max()) + 1 + padding)
    return x0, y0, x1, y1


def resize_rgba(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    rgba = np.asarray(image, dtype=np.float32) / 255.0
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
    straight = np.concatenate((np.clip(rgb, 0, 1), resized_alpha), axis=2)
    return Image.fromarray(np.clip(straight * 255.0, 0, 255).astype(np.uint8), "RGBA")


def resize_mask(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    return image.resize(size, Image.Resampling.NEAREST)


def write_outputs() -> None:
    visuals = OUTPUT / "visuals"
    masks = OUTPUT / "masks"
    review = OUTPUT / "review"
    for folder in (visuals, masks, review):
        folder.mkdir(parents=True, exist_ok=True)

    master = Image.open(MASTER).convert("RGB")
    rgb = np.asarray(master)
    base_alpha = foreground_alpha(rgb)
    rows: list[dict[str, object]] = []
    assets: dict[int, dict[str, object]] = {}

    reconstruction = Image.new("RGBA", master.size, (0, 0, 0, 0))
    for cluster in CLUSTERS:
        selection = np.asarray(polygon_mask(master.size, cluster.polygons), dtype=np.uint8)
        visual_alpha = np.minimum(base_alpha, selection)
        if cluster.category == "floating_platform":
            visual_alpha = keep_largest_component(visual_alpha)
        collision_alpha = ((visual_alpha >= 128) * 255).astype(np.uint8)
        x0, y0, x1, y1 = tight_bounds(visual_alpha, PADDING_MASTER)

        crop_rgb = rgb[y0:y1, x0:x1]
        crop_alpha = visual_alpha[y0:y1, x0:x1]
        rgba = np.dstack((crop_rgb, crop_alpha)).astype(np.uint8)
        visual_master = Image.fromarray(rgba, "RGBA")
        mask_master = Image.fromarray(collision_alpha[y0:y1, x0:x1], "L")
        production_size = (visual_master.width * SCALE, visual_master.height * SCALE)

        stem = f"arena_desert_sci_fi_{cluster.asset_id}_01"
        visual_name = f"{stem}_visual.png"
        mask_name = f"{stem}_mask.png"
        resize_rgba(visual_master, production_size).save(visuals / visual_name)
        resize_mask(mask_master, production_size).save(masks / mask_name)
        reconstruction.alpha_composite(visual_master, (x0, y0))

        production_crop = {
            "x_px": x0 * SCALE,
            "y_px": y0 * SCALE,
            "width_px": production_size[0],
            "height_px": production_size[1],
        }
        assets[cluster.number] = {
            "cluster": cluster,
            "stem": stem,
            "visual_name": visual_name,
            "mask_name": mask_name,
            "master_crop": (x0, y0, x1, y1),
            "production_crop": production_crop,
        }
        rows.append({
            "piece_id": stem,
            "file": f"visuals/{visual_name}",
            "mask_file": f"masks/{mask_name}",
            "category": cluster.category,
            "sorting_order": cluster.sorting_order,
            **production_crop,
            "padding_px": PADDING_MASTER * SCALE,
            "destructible": "true",
            "notes": cluster.notes,
        })

    reconstruction.save(review / "arena_desert_sci_fi_reconstructed_unique_assets.png")

    assembled = reconstruction.copy()
    small_data = assets[6]
    sx0, sy0, sx1, sy1 = small_data["master_crop"]
    small_visual = Image.open(visuals / small_data["visual_name"]).convert("RGBA").resize(
        (sx1 - sx0, sy1 - sy0), Image.Resampling.LANCZOS
    )
    assembled.alpha_composite(small_visual, (1434, 226))
    assembled.save(review / "arena_desert_sci_fi_reconstructed_with_reuse.png")

    layout = {
        "arena_id": "arena.desert-sci-fi",
        "source_master": str(MASTER.relative_to(ROOT)).replace("\\", "/"),
        "master_width_px": master.width,
        "master_height_px": master.height,
        "production_scale": SCALE,
        "production_canvas_width_px": master.width * SCALE,
        "production_canvas_height_px": master.height * SCALE,
        "runtime_pixels_per_unit": 256,
        "pivot": [0.5, 0.5],
        "small_platform_reuse": "cluster_06 is referenced by two instances with identical source visual and mask",
    }
    (OUTPUT / "arena_desert_sci_fi_layout.json").write_text(
        json.dumps(layout, indent=2, ensure_ascii=False) + "\n", encoding="utf-8"
    )

    fieldnames = ["piece_id", "file", "mask_file", "category", "sorting_order", "x_px", "y_px", "width_px", "height_px", "padding_px", "destructible", "notes"]
    with (OUTPUT / "arena_desert_sci_fi_pieces.csv").open("w", newline="", encoding="utf-8") as handle:
        writer = csv.DictWriter(handle, fieldnames=fieldnames)
        writer.writeheader()
        writer.writerows(rows)

    # A separate instance table allows one asset pair to be used more than once.
    instances = []
    for number in (1, 2, 3, 4, 5, 7, 8):
        data = assets[number]
        crop = data["production_crop"]
        instances.append((f"cluster_{number:02d}", data["stem"], crop["x_px"], crop["y_px"], 1.0, 1.0, False))

    small = assets[6]
    small_crop = small["production_crop"]
    instances.append(("cluster_06_a", small["stem"], small_crop["x_px"], small_crop["y_px"], 1.0, 1.0, False))
    # Reuse at the upper-right platform center. Same image, PPU and scale.
    duplicate_master_x = 1434
    duplicate_master_y = 226
    instances.append(("cluster_06_b", small["stem"], duplicate_master_x * SCALE, duplicate_master_y * SCALE, 1.0, 1.0, True))
    with (OUTPUT / "arena_desert_sci_fi_instances.csv").open("w", newline="", encoding="utf-8") as handle:
        writer = csv.writer(handle)
        writer.writerow(("instance_id", "asset_id", "x_px", "y_px", "scale_x", "scale_y", "reused_asset"))
        writer.writerows(instances)


if __name__ == "__main__":
    write_outputs()
