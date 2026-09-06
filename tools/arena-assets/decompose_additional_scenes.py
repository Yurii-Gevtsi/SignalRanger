#!/usr/bin/env python3
"""Prepare standalone scene illustrations as large destructible arena pieces."""
from pathlib import Path
import json
import numpy as np
from PIL import Image

ROOT = Path(__file__).resolve().parents[2]
SOURCE_ROOT = Path(r"C:\Users\Yurii\Desktop\Game\UI\Arenas\Scenes")
SOURCE_FILES = {
    "city": "вокзал.png",
    "grib": "гриби.png",
    "laguna": "острови.png",
    "monument": "монумент.png",
    "kraken": "кракен.png",
    "vines": "ліани.png",
    "robots": "роботи.png",
}
MASTER = (2048, 1152)
PADDING = 24
SCENES = {
    "city": (("left", (0, 0, 850, 1152)), ("center", (620, 390, 1420, 1152)), ("right", (1190, 0, 2048, 1152))),
    "grib": (("left", (0, 0, 720, 1152)), ("center", (530, 310, 1320, 1152)), ("right", (1080, 0, 1800, 1152)), ("far_right", (1600, 360, 2048, 1152))),
    "laguna": (("left", (0, 150, 850, 1152)), ("center", (620, 430, 1420, 1152)), ("right", (1180, 130, 2048, 1152))),
    "monument": (("left", (0, 280, 820, 1152)), ("center", (520, 0, 1510, 1152)), ("right", (1210, 260, 2048, 1152))),
    "kraken": (("left", (0, 120, 770, 1152)), ("center", (590, 0, 1370, 1152)), ("right", (1120, 120, 2048, 1152))),
    "vines": (("left", (0, 0, 660, 1152)), ("center", (470, 0, 1230, 1152)), ("right", (1000, 0, 1800, 1152)), ("far_right", (1460, 0, 2048, 1152))),
    "robots": (("left", (0, 180, 850, 1152)), ("center", (590, 120, 1430, 1152)), ("right", (1160, 180, 2048, 1152))),
}

def sanitize_rgba(image: Image.Image) -> Image.Image:
    pixels = np.asarray(image.convert("RGBA"), dtype=np.uint8).copy()
    pixels[pixels[..., 3] == 0, :3] = 0
    return Image.fromarray(pixels, "RGBA")

def source_rgba(path: Path) -> Image.Image:
    image = np.asarray(Image.open(path).convert("RGBA"), dtype=np.uint8).copy()
    # Some delivered scenes use flat black both outside the illustration and
    # inside open arches/caves. Those black regions are a backdrop, not art.
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
    # The backdrop may be enclosed by painted terrain, so a border-only flood
    # fill leaves black holes. Remove only substantial exact-black islands:
    # thin dark linework and shaded machinery remain untouched.
    candidate &= ~exterior
    seen = np.zeros(candidate.shape, dtype=bool)
    height, width = candidate.shape
    for y in range(height):
        for x in range(width):
            if seen[y, x] or not candidate[y, x]:
                continue
            stack = [(x, y)]
            seen[y, x] = True
            component = []
            while stack:
                current_x, current_y = stack.pop()
                component.append((current_x, current_y))
                for next_x, next_y in ((current_x - 1, current_y), (current_x + 1, current_y), (current_x, current_y - 1), (current_x, current_y + 1)):
                    if 0 <= next_x < width and 0 <= next_y < height and candidate[next_y, next_x] and not seen[next_y, next_x]:
                        seen[next_y, next_x] = True
                        stack.append((next_x, next_y))
            if len(component) >= 64:
                for current_x, current_y in component:
                    image[current_y, current_x, 3] = 0
    image[image[..., 3] == 0, :3] = 0
    return sanitize_rgba(Image.fromarray(image, "RGBA"))

def main() -> None:
    for arena_id, regions in SCENES.items():
        source = source_rgba(SOURCE_ROOT / SOURCE_FILES[arena_id])
        master = sanitize_rgba(source.resize(MASTER, Image.Resampling.LANCZOS))
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
            visual = sanitize_rgba(master.crop(crop))
            mask = Image.fromarray((np.asarray(visual)[...,3] >= 128).astype(np.uint8)*255, "L")
            for directory in (source_dir, runtime_dir):
                visual.save(directory / f"{arena_id}_{piece_id}_visual.png")
                mask.save(directory / f"{arena_id}_{piece_id}_mask.png")
            pieces.append({"id":piece_id,"crop":crop,"sorting_order":-8+order})
        (output_dir / "layout.json").write_text(json.dumps({"arena_id":f"arena.{arena_id}","master":MASTER,"pieces":pieces}, indent=2)+"\n", encoding="utf-8")

if __name__ == "__main__": main()
