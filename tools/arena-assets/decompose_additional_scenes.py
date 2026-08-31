#!/usr/bin/env python3
"""Prepare the City, Mushroom, and Lagoon arenas as large destructible pieces."""
from pathlib import Path
import json
import numpy as np
from PIL import Image

ROOT = Path(__file__).resolve().parents[2]
SOURCE_ROOT = Path(r"C:\Users\Yurii\Desktop\Game\UI\Arenas\Scenes")
MASTER = (2048, 1152)
PADDING = 24
SCENES = {
    "city": (("left", (0, 0, 850, 1152)), ("center", (620, 390, 1420, 1152)), ("right", (1190, 0, 2048, 1152))),
    "grib": (("left", (0, 0, 720, 1152)), ("center", (530, 310, 1320, 1152)), ("right", (1080, 0, 1800, 1152)), ("far_right", (1600, 360, 2048, 1152))),
    "laguna": (("left", (0, 150, 850, 1152)), ("center", (620, 430, 1420, 1152)), ("right", (1180, 130, 2048, 1152))),
}

def source_rgba(path: Path) -> Image.Image:
    image = np.asarray(Image.open(path).convert("RGBA"), dtype=np.uint8).copy()
    # City is delivered with a flat black RGB backdrop; remove only exterior
    # black so interior shadows/windows remain part of the destructible visual.
    candidate = np.max(image[..., :3], axis=2) < 18
    exterior = np.zeros(candidate.shape, dtype=bool)
    stack = [(x, 0) for x in range(image.shape[1])] + [(x, image.shape[0] - 1) for x in range(image.shape[1])]
    stack += [(0, y) for y in range(image.shape[0])] + [(image.shape[1] - 1, y) for y in range(image.shape[0])]
    while stack:
        x, y = stack.pop()
        if x < 0 or y < 0 or x >= image.shape[1] or y >= image.shape[0] or exterior[y, x] or not candidate[y, x]: continue
        exterior[y, x] = True
        stack += [(x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)]
    image[exterior, 3] = 0
    image[image[..., 3] == 0, :3] = 0
    return Image.fromarray(image, "RGBA")

def main() -> None:
    for arena_id, regions in SCENES.items():
        source = source_rgba(SOURCE_ROOT / f"{arena_id}.png")
        master = source.resize(MASTER, Image.Resampling.LANCZOS)
        source_dir = ROOT / "art_source/arenas" / arena_id / "production_source"
        runtime_dir = ROOT / "Assets/_Game/Content/Resources/Visuals/Arenas" / arena_id.title()
        output_dir = ROOT / "tools/arena-assets" / arena_id
        for directory in (source_dir, runtime_dir, output_dir): directory.mkdir(parents=True, exist_ok=True)
        master.save(source_dir / "layout_master.png")
        pieces = []
        for order, (piece_id, box) in enumerate(regions):
            alpha = np.asarray(master)[box[1]:box[3], box[0]:box[2], 3]
            ys, xs = np.nonzero(alpha > 8)
            if not len(xs): raise ValueError(f"empty piece {arena_id}/{piece_id}")
            crop = tuple(int(value) for value in (max(0, box[0]+xs.min()-PADDING), max(0, box[1]+ys.min()-PADDING), min(MASTER[0], box[0]+xs.max()+1+PADDING), min(MASTER[1], box[1]+ys.max()+1+PADDING)))
            visual = master.crop(crop)
            mask = Image.fromarray((np.asarray(visual)[...,3] >= 128).astype(np.uint8)*255, "L")
            for directory in (source_dir, runtime_dir):
                visual.save(directory / f"{arena_id}_{piece_id}_visual.png")
                mask.save(directory / f"{arena_id}_{piece_id}_mask.png")
            pieces.append({"id":piece_id,"crop":crop,"sorting_order":-8+order})
        (output_dir / "layout.json").write_text(json.dumps({"arena_id":f"arena.{arena_id}","master":MASTER,"pieces":pieces}, indent=2)+"\n", encoding="utf-8")

if __name__ == "__main__": main()
