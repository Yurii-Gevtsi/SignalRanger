from __future__ import annotations

import argparse
import math
import textwrap
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Render compact text into one dense PNG.")
    parser.add_argument("input", type=Path)
    parser.add_argument("output", type=Path)
    parser.add_argument("--width", type=int, default=1573)
    parser.add_argument("--height", type=int, default=1248)
    parser.add_argument("--margin", type=int, default=8)
    parser.add_argument("--font-size", type=int, default=8)
    parser.add_argument("--line-spacing", type=int, default=1)
    parser.add_argument("--font", type=Path, default=Path(r"C:\Windows\Fonts\DejaVuSansMono.ttf"))
    parser.add_argument(
        "--reflow-newlines",
        action="store_true",
        help="Replace source newlines with inline markers before wrapping, pxpipe-style.",
    )
    return parser.parse_args()


def normalize(text: str) -> str:
    text = text.replace("\r\n", "\n").replace("\r", "\n")
    return "\n".join(line.rstrip() for line in text.split("\n"))


def wrap_lines(text: str, chars_per_line: int) -> list[str]:
    wrapped: list[str] = []
    wrapper = textwrap.TextWrapper(
        width=chars_per_line,
        expand_tabs=True,
        replace_whitespace=False,
        drop_whitespace=False,
        break_long_words=True,
        break_on_hyphens=False,
    )
    for raw_line in normalize(text).split("\n"):
        if not raw_line:
            wrapped.append("")
            continue
        wrapped.extend(wrapper.wrap(raw_line) or [raw_line])
    return wrapped


def main() -> None:
    args = parse_args()
    text = normalize(args.input.read_text(encoding="utf-8"))
    if args.reflow_newlines:
        text = text.replace("\n", " ↵ ")
    args.output.parent.mkdir(parents=True, exist_ok=True)

    font = ImageFont.truetype(str(args.font), args.font_size)
    probe = Image.new("RGB", (16, 16), "white")
    draw = ImageDraw.Draw(probe)
    char_width = max(1, math.ceil(draw.textlength("M", font=font)))
    line_height = args.font_size + args.line_spacing
    chars_per_line = max(40, (args.width - args.margin * 2) // char_width)
    max_lines = max(1, (args.height - args.margin * 2) // line_height)
    lines = wrap_lines(text, chars_per_line)

    if len(lines) > max_lines:
        raise SystemExit(
            f"Text does not fit: {len(lines)} wrapped lines, capacity {max_lines}. "
            "Use a smaller font or larger canvas."
        )

    image = Image.new("RGB", (args.width, args.height), "white")
    draw = ImageDraw.Draw(image)
    y = args.margin
    for line in lines:
        draw.text((args.margin, y), line, fill=(18, 18, 18), font=font)
        y += line_height

    image.save(args.output, format="PNG", optimize=True)

    info = [
        f"input={args.input}",
        f"output={args.output}",
        f"size={args.width}x{args.height}",
        f"font={args.font}",
        f"font_size={args.font_size}",
        f"line_height={line_height}",
        f"chars_per_line={chars_per_line}",
        f"wrapped_lines={len(lines)}",
        f"line_capacity={max_lines}",
    ]
    args.output.with_suffix(".txt").write_text("\n".join(info), encoding="utf-8")


if __name__ == "__main__":
    main()
