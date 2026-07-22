from __future__ import annotations

import argparse
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter


CRATE_POLYGONS = (
    ((2006, 1022), (2024, 995), (2208, 989), (2248, 1016), (2258, 1121), (2229, 1156), (2044, 1157), (2008, 1128)),
    ((1867, 1172), (1892, 1144), (2041, 1129), (2104, 1143), (2135, 1174), (2132, 1279), (2097, 1310), (1901, 1307), (1868, 1277)),
    ((2125, 1170), (2153, 1140), (2308, 1140), (2360, 1168), (2375, 1275), (2344, 1307), (2161, 1308), (2128, 1284)),
)


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("input", type=Path)
    parser.add_argument("output", type=Path)
    parser.add_argument("--radius", type=float, default=1.6)
    parser.add_argument("--percent", type=int, default=175)
    parser.add_argument("--threshold", type=int, default=4)
    args = parser.parse_args()

    source = Image.open(args.input).convert("RGBA")
    rgb = source.convert("RGB")
    sharpened = rgb.filter(
        ImageFilter.UnsharpMask(
            radius=args.radius,
            percent=args.percent,
            threshold=args.threshold,
        )
    )

    mask = Image.new("L", source.size, 0)
    draw = ImageDraw.Draw(mask)
    for polygon in CRATE_POLYGONS:
        draw.polygon(polygon, fill=255)
    mask = mask.filter(ImageFilter.GaussianBlur(radius=2.0))

    result_rgb = Image.composite(sharpened, rgb, mask)
    result = Image.merge("RGBA", (*result_rgb.split(), source.getchannel("A")))
    args.output.parent.mkdir(parents=True, exist_ok=True)
    result.save(args.output, format="PNG", optimize=True)


if __name__ == "__main__":
    main()
