#!/usr/bin/env python3
"""Prepare generated arena visual pieces for Unity.

The tool keeps the original visual-project exports untouched. It copies them
into a local source snapshot, keeps or restores alpha, dilates edge colors,
writes clean runtime PNG files, and emits layout/pieces metadata plus a
reconstructed preview.
"""

from __future__ import annotations

import argparse
import csv
import json
import re
import shutil
from collections import deque
from dataclasses import dataclass
from pathlib import Path

import numpy as np
from PIL import Image


DEFAULT_LAYOUT = {
    "arena_id": "arena.training-meadow",
    "layout_master": "source/layout_master.png",
    "master_width_px": 4096,
    "master_height_px": 2304,
    "master_pixels_per_unit": 245.0,
    "master_scale": 1.8,
    "master_world_position": [0.0, -2.42],
    "runtime_pixels_per_unit": 256.0,
    "notes": "Initial single-ground evaluation. Runtime positions preserve the approved layout master composition.",
}


TRAINING_SEED_PIECES = {
    "ground": (-8, -0.09862, -4.93959, 3.47552, 3.91558),
    "ship": (-7, -9.66498, -0.53610, 2.05578, 1.88287),
    "boxes": (-7, 0.83379, -2.39658, 0.97620, 1.00449),
    "rock": (-7, 9.56094, -2.18896, 1.85961, 1.80162),
}


@dataclass(frozen=True)
class Piece:
    piece_id: str
    source_visual: Path
    source_mask: Path | None
    sorting_order: int
    position_x: float
    position_y: float
    scale_x: float
    scale_y: float
    destructible: bool = True


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument(
        "--source",
        type=Path,
        default=Path(r"C:\Users\Yurii\Desktop\Game\UI\Arenas\Scenes\Training"),
        help="Folder containing layout_master.png and *_visual.png files.",
    )
    parser.add_argument("--arena-key", default="training", help="Local generated folder key.")
    parser.add_argument("--unity-folder-name", default="Training", help="Unity Resources arena folder name.")
    parser.add_argument(
        "--work-output",
        type=Path,
        default=Path("tools/arena-assets/training"),
        help="Generated metadata and source snapshot folder.",
    )
    parser.add_argument(
        "--unity-output",
        type=Path,
        default=Path("Assets/_Game/Content/Resources/Visuals/Arenas/Training"),
        help="Unity Resources folder for clean runtime PNG files.",
    )
    parser.add_argument("--dilation-passes", type=int, default=6)
    parser.add_argument(
        "--use-masks",
        action="store_true",
        help="Legacy mode: use matching *_mask.png files to restore alpha.",
    )
    parser.add_argument(
        "--key-color",
        default="",
        help="Optional chroma key color to remove, for example #ff00ff.",
    )
    parser.add_argument(
        "--key-tolerance",
        type=int,
        default=46,
        help="Maximum RGB distance from --key-color that becomes transparent.",
    )
    parser.add_argument(
        "--key-softness",
        type=int,
        default=18,
        help="Additional RGB distance used for antialiased alpha falloff.",
    )
    return parser.parse_args()


def normalize_piece_id(stem: str) -> str:
    name = re.sub(r"_visual(?:_\d+)?$", "", stem.lower())
    name = re.sub(r"(?:_\d+)$", "", name)
    if name == "graund":
        return "ground"
    if name == "box":
        return "boxes"
    return name


def discover_pairs(source: Path) -> list[tuple[str, Path, Path | None]]:
    pairs_by_id: dict[str, tuple[str, Path, Path | None]] = {}
    candidates = []
    candidates.extend(sorted(source.glob("*_visual.png")))
    candidates.extend(sorted(source.glob("*_visual_*.png")))
    for piece_id in TRAINING_SEED_PIECES:
        candidates.extend(sorted(source.glob(f"{piece_id}.png")))
        candidates.extend(sorted(source.glob(f"{piece_id}_*.png")))
    candidates.extend(sorted(source.glob("graund.png")))
    candidates.extend(sorted(source.glob("graund_*.png")))

    for visual in candidates:
        if visual.name == "layout_master.png" or visual.stem.endswith("_mask"):
            continue
        raw_name = re.sub(r"_visual(?:_\d+)?$", "", visual.stem, flags=re.IGNORECASE)
        raw_name = re.sub(r"(?:_\d+)$", "", raw_name)
        mask = source / f"{raw_name}_mask.png"
        piece_id = normalize_piece_id(visual.stem)
        pairs_by_id[piece_id] = (piece_id, visual, mask if mask.exists() else None)
    pairs = [pairs_by_id[key] for key in sorted(pairs_by_id)]
    if not pairs:
        raise FileNotFoundError(f"No *_visual.png files found in {source}")
    return pairs


def copy_source_snapshot(source: Path, output_source: Path) -> None:
    output_source.mkdir(parents=True, exist_ok=True)
    for path in sorted(source.glob("*.png")):
        shutil.copy2(path, output_source / path.name)


def mask_to_alpha(mask: Image.Image) -> np.ndarray:
    gray = np.asarray(mask.convert("L"), dtype=np.uint8)
    return gray


def parse_key_color(value: str) -> np.ndarray | None:
    text = value.strip()
    if not text:
        return None
    if text.startswith("#"):
        text = text[1:]
    if len(text) != 6:
        raise ValueError(f"Key color must be hex RGB like #ff00ff, got '{value}'")
    return np.array([int(text[0:2], 16), int(text[2:4], 16), int(text[4:6], 16)], dtype=np.float32)


def alpha_from_chroma_key(rgb: np.ndarray, key_color: np.ndarray, tolerance: int, softness: int) -> np.ndarray:
    rgb_f32 = rgb.astype(np.float32)
    distance = np.linalg.norm(rgb_f32 - key_color.reshape(1, 1, 3), axis=2)
    hard = max(0.0, float(tolerance))
    soft = max(1.0, float(softness))
    alpha = np.clip((distance - hard) / soft, 0.0, 1.0)
    return (alpha * 255.0).astype(np.uint8)


def suppress_light_checker_background(rgb: np.ndarray, alpha: np.ndarray) -> np.ndarray:
    """Drop only border-connected nearly-white baked checker pixels.

    Real arena objects can have bright white panels. Those must stay opaque.
    The generated checker background, however, is connected to the transparent
    outside of the cutout, so a small flood fill removes the background without
    punching holes through white object details.
    """
    rgb_i16 = rgb.astype(np.int16)
    channel_range = rgb_i16.max(axis=2) - rgb_i16.min(axis=2)
    brightness = rgb_i16.mean(axis=2)
    checker_like = (brightness >= 235) & (channel_range <= 14)
    height, width = checker_like.shape
    removable = np.zeros_like(checker_like, dtype=bool)
    queue: deque[tuple[int, int]] = deque()

    def enqueue_if_checker(x: int, y: int) -> None:
        if checker_like[y, x] and not removable[y, x]:
            removable[y, x] = True
            queue.append((x, y))

    for x in range(width):
        enqueue_if_checker(x, 0)
        enqueue_if_checker(x, height - 1)
    for y in range(height):
        enqueue_if_checker(0, y)
        enqueue_if_checker(width - 1, y)

    while queue:
        x, y = queue.popleft()
        if x > 0:
            enqueue_if_checker(x - 1, y)
        if x + 1 < width:
            enqueue_if_checker(x + 1, y)
        if y > 0:
            enqueue_if_checker(x, y - 1)
        if y + 1 < height:
            enqueue_if_checker(x, y + 1)

    result = alpha.copy()
    result[removable] = 0
    return result


def dilate_colors(rgba: np.ndarray, alpha: np.ndarray, passes: int) -> np.ndarray:
    result = rgba.copy()
    opaque = alpha > 0
    result[..., 3] = alpha

    known = opaque.copy()
    for _ in range(max(0, passes)):
        missing = ~known
        if not missing.any():
            break

        filled = result.copy()
        filled_known = known.copy()
        for dy, dx in ((-1, 0), (1, 0), (0, -1), (0, 1)):
            shifted_known = np.roll(known, shift=(dy, dx), axis=(0, 1))
            shifted_rgb = np.roll(result[..., :3], shift=(dy, dx), axis=(0, 1))

            if dy < 0:
                shifted_known[dy:, :] = False
            elif dy > 0:
                shifted_known[:dy, :] = False
            if dx < 0:
                shifted_known[:, dx:] = False
            elif dx > 0:
                shifted_known[:, :dx] = False

            take = missing & shifted_known & ~filled_known
            filled[..., :3][take] = shifted_rgb[take]
            filled_known[take] = True

        result = filled
        known = filled_known

    result[..., 3] = alpha
    result[..., :3][alpha == 0] = result[..., :3][alpha == 0]
    return result


def clean_visual(
    visual_path: Path,
    mask_path: Path | None,
    output_path: Path,
    dilation_passes: int,
    use_masks: bool,
    key_color: np.ndarray | None,
    key_tolerance: int,
    key_softness: int,
) -> None:
    visual = Image.open(visual_path).convert("RGBA")
    rgba = np.asarray(visual, dtype=np.uint8)
    if key_color is not None:
        alpha = alpha_from_chroma_key(rgba[..., :3], key_color, key_tolerance, key_softness)
    elif use_masks and mask_path is not None:
        mask = Image.open(mask_path)
        if visual.size != mask.size:
            raise ValueError(f"Visual/mask size mismatch: {visual_path.name} {visual.size} vs {mask_path.name} {mask.size}")
        alpha = suppress_light_checker_background(rgba[..., :3], mask_to_alpha(mask))
    else:
        alpha = rgba[..., 3]
    cleaned = dilate_colors(rgba, alpha, dilation_passes)
    output_path.parent.mkdir(parents=True, exist_ok=True)
    Image.fromarray(cleaned, "RGBA").save(output_path)


def read_or_seed_pieces(work_output: Path, pairs: list[tuple[str, Path, Path | None]], unity_folder_name: str) -> list[Piece]:
    pair_by_id = {piece_id: (visual, mask) for piece_id, visual, mask in pairs}
    csv_path = work_output / "pieces.csv"
    if csv_path.exists():
        pieces: list[Piece] = []
        with csv_path.open("r", newline="", encoding="utf-8") as handle:
            for row in csv.DictReader(handle):
                piece_id = row["piece_id"]
                discovered = pair_by_id.get(piece_id)
                source_visual = Path("source") / discovered[0].name if discovered is not None else normalize_metadata_path(work_output, row["source_visual"])
                source_mask_value = row.get("source_mask", "").strip()
                source_mask = Path("source") / discovered[1].name if discovered is not None and discovered[1] is not None else normalize_metadata_path(work_output, source_mask_value) if source_mask_value else None
                pieces.append(
                    Piece(
                        piece_id=piece_id,
                        source_visual=source_visual,
                        source_mask=source_mask,
                        sorting_order=int(row["sorting_order"]),
                        position_x=float(row["position_x"]),
                        position_y=float(row["position_y"]),
                        scale_x=float(row["scale_x"]),
                        scale_y=float(row["scale_y"]),
                        destructible=row["destructible"].lower() == "true",
                    )
                )
        return pieces

    pieces = []
    for piece_id, visual, mask in pairs:
        if piece_id not in TRAINING_SEED_PIECES:
            raise ValueError(
                f"No seed coordinates for piece '{piece_id}'. Add it to pieces.csv once, then rerun this tool."
            )
        sorting_order, position_x, position_y, scale_x, scale_y = TRAINING_SEED_PIECES[piece_id]
        pieces.append(
            Piece(
                piece_id=piece_id,
                source_visual=Path("source") / visual.name,
                source_mask=Path("source") / mask.name if mask is not None else None,
                sorting_order=sorting_order,
                position_x=position_x,
                position_y=position_y,
                scale_x=scale_x,
                scale_y=scale_y,
            )
        )
    return pieces


def normalize_metadata_path(work_output: Path, value: str) -> Path:
    path = Path(value)
    if not path.is_absolute():
        return path

    try:
        return path.resolve().relative_to(work_output.resolve())
    except ValueError:
        return path


def metadata_to_filesystem_path(work_output: Path, value: Path) -> Path:
    return value if value.is_absolute() else work_output / value


def write_layout(work_output: Path) -> dict:
    layout_path = work_output / "layout.json"
    if layout_path.exists():
        return json.loads(layout_path.read_text(encoding="utf-8"))
    layout_path.write_text(json.dumps(DEFAULT_LAYOUT, indent=2) + "\n", encoding="utf-8")
    return DEFAULT_LAYOUT


def write_pieces_csv(work_output: Path, pieces: list[Piece], unity_folder_name: str) -> None:
    csv_path = work_output / "pieces.csv"
    with csv_path.open("w", newline="", encoding="utf-8") as handle:
        writer = csv.writer(handle)
        writer.writerow(
            [
                "piece_id",
                "resource_path",
                "source_visual",
                "source_mask",
                "sorting_order",
                "position_x",
                "position_y",
                "scale_x",
                "scale_y",
                "destructible",
            ]
        )
        for piece in pieces:
            writer.writerow(
                [
                    piece.piece_id,
                    f"Visuals/Arenas/{unity_folder_name}/{piece.piece_id}",
                    piece.source_visual.as_posix(),
                    piece.source_mask.as_posix() if piece.source_mask is not None else "",
                    piece.sorting_order,
                    f"{piece.position_x:.5f}",
                    f"{piece.position_y:.5f}",
                    f"{piece.scale_x:.5f}",
                    f"{piece.scale_y:.5f}",
                    str(piece.destructible).lower(),
                ]
            )


def world_to_master_px(layout: dict, world_x: float, world_y: float) -> tuple[float, float]:
    master_w = float(layout["master_width_px"])
    master_h = float(layout["master_height_px"])
    ppu = float(layout["master_pixels_per_unit"])
    scale = float(layout["master_scale"])
    pos_x, pos_y = layout["master_world_position"]
    x = (world_x - float(pos_x)) / scale * ppu + master_w * 0.5
    y = master_h * 0.5 - (world_y - float(pos_y)) / scale * ppu
    return x, y


def render_preview(work_output: Path, unity_output: Path, pieces: list[Piece], layout: dict) -> None:
    master_size = (int(layout["master_width_px"]), int(layout["master_height_px"]))
    preview = Image.new("RGBA", master_size, (0, 0, 0, 0))
    runtime_ppu = float(layout["runtime_pixels_per_unit"])

    for piece in sorted(pieces, key=lambda item: item.sorting_order):
        image = Image.open(unity_output / f"{piece.piece_id}.png").convert("RGBA")
        world_w = image.width * piece.scale_x / runtime_ppu
        world_h = image.height * piece.scale_y / runtime_ppu
        center_x, center_y = world_to_master_px(layout, piece.position_x, piece.position_y)
        left_x, top_y = world_to_master_px(layout, piece.position_x - world_w * 0.5, piece.position_y + world_h * 0.5)
        right_x, bottom_y = world_to_master_px(layout, piece.position_x + world_w * 0.5, piece.position_y - world_h * 0.5)
        width = max(1, int(round(abs(right_x - left_x))))
        height = max(1, int(round(abs(bottom_y - top_y))))
        resized = image.resize((width, height), Image.Resampling.LANCZOS)
        paste_x = int(round(center_x - width * 0.5))
        paste_y = int(round(center_y - height * 0.5))
        preview.alpha_composite(resized, (paste_x, paste_y))

    preview.save(work_output / "reconstructed_preview.png")


def main() -> None:
    args = parse_args()
    source = args.source.resolve()
    work_output = args.work_output.resolve()
    unity_output = args.unity_output.resolve()
    source_snapshot = work_output / "source"

    pairs = discover_pairs(source)
    key_color = parse_key_color(args.key_color)
    copy_source_snapshot(source, source_snapshot)
    layout = write_layout(work_output)
    pieces = read_or_seed_pieces(work_output, pairs, args.unity_folder_name)
    if not args.use_masks:
        pieces = [
            Piece(
                piece_id=piece.piece_id,
                source_visual=piece.source_visual,
                source_mask=None,
                sorting_order=piece.sorting_order,
                position_x=piece.position_x,
                position_y=piece.position_y,
                scale_x=piece.scale_x,
                scale_y=piece.scale_y,
                destructible=piece.destructible,
            )
            for piece in pieces
        ]

    for piece in pieces:
        clean_visual(
            metadata_to_filesystem_path(work_output, piece.source_visual),
            metadata_to_filesystem_path(work_output, piece.source_mask) if piece.source_mask is not None else None,
            unity_output / f"{piece.piece_id}.png",
            args.dilation_passes,
            args.use_masks,
            key_color,
            args.key_tolerance,
            args.key_softness,
        )

    write_pieces_csv(work_output, pieces, args.unity_folder_name)
    render_preview(work_output, unity_output, pieces, layout)
    print(f"Prepared {len(pieces)} arena pieces from {source}")
    print(f"Unity PNG output: {unity_output}")
    print(f"Metadata output: {work_output}")


if __name__ == "__main__":
    main()
