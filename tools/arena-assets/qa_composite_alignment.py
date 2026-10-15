"""Check that runtime sprite placement reconstructs the source canvas."""
from pathlib import Path
import json
import numpy as np
from PIL import Image

ROOT = Path(__file__).resolve().parents[2]
MASTER = (2048, 1152)
BOXES = {
    "city": {"left": (0, 60, 874, 1131), "center": (596, 411, 1444, 1138), "right": (1166, 159, 2048, 1138)},
    "grib": {"left": (0, 0, 744, 1151), "center": (506, 286, 1344, 1152), "right": (1056, 119, 1824, 1152), "far_right": (1576, 336, 2048, 1141)},
    "laguna": {"left": (0, 179, 874, 1048), "center": (596, 406, 1444, 1050), "right": (1156, 143, 2048, 1055)},
    "monument": {"left": (0, 256, 844, 1062), "center": (496, 77, 1534, 1058), "right": (1186, 236, 2048, 1063)},
    "kraken": {"left": (0, 128, 794, 1143), "center": (566, 64, 1394, 1152), "right": (1096, 207, 2048, 1152)},
    "vines": {"left": (0, 109, 684, 985), "center": (446, 165, 1254, 990), "right": (976, 165, 1824, 990), "far_right": (1436, 194, 2048, 956)},
    "robots": {"left": (72, 159, 874, 1049), "center": (566, 159, 1454, 1049), "right": (1136, 221, 1999, 1050)},
}


def main():
    for arena_id, boxes in BOXES.items():
        source_dir = ROOT / "art_source/arenas" / arena_id / "production_source"
        layout = json.loads((ROOT / "tools/arena-assets" / arena_id / "layout.json").read_text())
        source = np.asarray(Image.open(source_dir / "layout_master.png").convert("RGBA"))
        canvas = np.zeros_like(source)
        covered = np.zeros(MASTER[::-1], dtype=bool)
        overlap = np.zeros(MASTER[::-1], dtype=np.uint8)
        for piece in layout["pieces"]:
            image = np.asarray(Image.open(source_dir / f"{arena_id}_{piece['id']}_visual.png").convert("RGBA"))
            x0, y0, x1, y1 = boxes[piece["id"]]
            tx = round((x0 + x1) / 2 - image.shape[1] / 2)
            ty = round((y0 + y1) / 2 - image.shape[0] / 2)
            xa, ya = max(0, tx), max(0, ty)
            xb, yb = min(MASTER[0], tx + image.shape[1]), min(MASTER[1], ty + image.shape[0])
            sx, sy = xa - tx, ya - ty
            ex, ey = xb - tx, yb - ty
            alpha = image[sy:ey, sx:ex, 3] > 8
            overlap[ya:yb, xa:xb] += alpha
            destination = canvas[ya:yb, xa:xb]
            destination[alpha] = image[sy:ey, sx:ex][alpha]
            covered[ya:yb, xa:xb] |= alpha
        source_alpha = source[..., 3] > 8
        opaque_black = (source[..., 3] > 8) & (np.max(source[..., :3], axis=2) < 12)
        border_black = np.zeros_like(opaque_black)
        stack = [(x, 0) for x in range(MASTER[0])] + [(x, MASTER[1] - 1) for x in range(MASTER[0])]
        stack += [(0, y) for y in range(MASTER[1])] + [(MASTER[0] - 1, y) for y in range(MASTER[1])]
        while stack:
            x, y = stack.pop()
            if x < 0 or y < 0 or x >= MASTER[0] or y >= MASTER[1] or border_black[y, x] or not opaque_black[y, x]:
                continue
            border_black[y, x] = True
            stack.extend(((x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)))
        missing = int(np.sum(source_alpha & ~covered))
        extra = int(np.sum(covered & ~source_alpha))
        changed = int(np.sum(np.any(canvas[covered] != source[covered], axis=1)))
        seams = int(np.sum(overlap > 1))
        print(f"{arena_id}: missing={missing} extra={extra} changed={changed} overlap={seams} opaque_black={int(np.sum(opaque_black))} border_black={int(np.sum(border_black))}")


if __name__ == "__main__":
    main()
