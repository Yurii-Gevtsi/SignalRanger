from __future__ import annotations

import argparse
import math
import textwrap
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Render a UTF-8 text document into dense PNG pages optimized for LLM ingestion."
    )
    parser.add_argument("input", type=Path, help="Path to the input UTF-8 text file.")
    parser.add_argument("output_dir", type=Path, help="Directory where PNG pages will be saved.")
    parser.add_argument("--width", type=int, default=1600, help="Page width in pixels.")
    parser.add_argument("--height", type=int, default=2200, help="Page height in pixels.")
    parser.add_argument("--margin", type=int, default=36, help="Outer page margin in pixels.")
    parser.add_argument("--font-size", type=int, default=18, help="Monospace font size.")
    parser.add_argument(
        "--line-spacing",
        type=int,
        default=3,
        help="Extra pixels between lines.",
    )
    parser.add_argument(
        "--font",
        type=Path,
        default=Path(r"C:\Windows\Fonts\DejaVuSansMono.ttf"),
        help="Path to a monospace TTF font.",
    )
    parser.add_argument(
        "--stem",
        default="llm-optimized-page",
        help="Base filename for output pages.",
    )
    return parser.parse_args()


def normalize_text(text: str) -> str:
    text = text.replace("\r\n", "\n").replace("\r", "\n")
    return "\n".join(line.rstrip() for line in text.split("\n"))


def wrap_paragraph(paragraph: str, width_chars: int) -> list[str]:
    if not paragraph:
        return [""]

    indent_width = len(paragraph) - len(paragraph.lstrip(" "))
    indent = " " * indent_width
    wrapper = textwrap.TextWrapper(
        width=width_chars,
        expand_tabs=False,
        replace_whitespace=False,
        drop_whitespace=False,
        break_long_words=True,
        break_on_hyphens=False,
        subsequent_indent=indent,
    )
    return wrapper.wrap(paragraph) or [paragraph]


def paginate_lines(lines: list[str], lines_per_page: int) -> list[list[str]]:
    return [lines[i : i + lines_per_page] for i in range(0, len(lines), lines_per_page)]


def main() -> None:
    args = parse_args()
    text = normalize_text(args.input.read_text(encoding="utf-8"))
    args.output_dir.mkdir(parents=True, exist_ok=True)

    font = ImageFont.truetype(str(args.font), args.font_size)
    probe = Image.new("RGB", (16, 16), "white")
    probe_draw = ImageDraw.Draw(probe)
    char_width = int(math.ceil(probe_draw.textlength("M", font=font)))
    line_height = args.font_size + args.line_spacing

    content_width = args.width - args.margin * 2
    content_height = args.height - args.margin * 2
    width_chars = max(20, content_width // max(1, char_width))
    lines_per_page = max(10, content_height // line_height)

    wrapped_lines: list[str] = []
    for paragraph in text.split("\n"):
        wrapped_lines.extend(wrap_paragraph(paragraph, width_chars))

    pages = paginate_lines(wrapped_lines, lines_per_page)

    for index, page_lines in enumerate(pages, start=1):
        image = Image.new("RGB", (args.width, args.height), "white")
        draw = ImageDraw.Draw(image)
        y = args.margin
        for line in page_lines:
            draw.text((args.margin, y), line, fill="black", font=font)
            y += line_height

        output_path = args.output_dir / f"{args.stem}-{index:02d}.png"
        image.save(output_path, format="PNG", optimize=True)

    manifest_lines = [
        f"input={args.input}",
        f"pages={len(pages)}",
        f"page_size={args.width}x{args.height}",
        f"font={args.font}",
        f"font_size={args.font_size}",
        f"margin={args.margin}",
        f"line_spacing={args.line_spacing}",
        f"wrapped_columns={width_chars}",
        f"lines_per_page={lines_per_page}",
    ]
    (args.output_dir / "render-info.txt").write_text("\n".join(manifest_lines), encoding="utf-8")


if __name__ == "__main__":
    main()
