from __future__ import annotations

import argparse
import re
from pathlib import Path


BOX_CHARS = set("│─└┘┌┐├┤┬┴┼▼▲↓↑→←↵")


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(description="Compact chat-like text before dense LLM image rendering.")
    parser.add_argument("input", type=Path)
    parser.add_argument("output", type=Path)
    return parser.parse_args()


def is_separator(line: str) -> bool:
    stripped = line.strip()
    return len(stripped) >= 20 and set(stripped) <= {"="}


def is_table_line(line: str) -> bool:
    stripped = line.strip()
    return stripped.startswith("|") and stripped.endswith("|")


def is_graphic_line(line: str) -> bool:
    stripped = line.strip()
    if not stripped:
        return False
    graphic_count = sum(1 for char in stripped if char in BOX_CHARS)
    return graphic_count > 0 and graphic_count >= max(1, len(stripped) // 3)


def compact_table(lines: list[str]) -> str:
    rows: list[str] = []
    for line in lines:
        cells = [cell.strip() for cell in line.strip().strip("|").split("|")]
        if not cells or all(re.fullmatch(r"[-: ]*", cell) for cell in cells):
            continue
        rows.append(" = ".join(cell for cell in cells if cell))
    return "TABLE: " + "; ".join(rows) if rows else ""


def compact_graphic(lines: list[str]) -> str:
    words: list[str] = []
    for line in lines:
        cleaned = "".join(" " if char in BOX_CHARS else char for char in line)
        cleaned = re.sub(r"\s+", " ", cleaned).strip()
        if cleaned:
            words.append(cleaned)
    if not words:
        return ""
    return "SCHEMA: " + " -> ".join(words)


def clean_inline_graphics(text: str) -> str:
    replacements = {
        "→": "->",
        "←": "<-",
        "↓": "->",
        "↑": "<-",
        "▼": "->",
        "▲": "<-",
    }
    for old, new in replacements.items():
        text = text.replace(old, new)
    for char in "│─└┘┌┐├┤┬┴┼":
        text = text.replace(char, " ")
    return re.sub(r"\s+", " ", text).strip()


def flush_paragraph(output: list[str], paragraph: list[str]) -> None:
    if not paragraph:
        return
    text = " ".join(clean_inline_graphics(part) for part in paragraph if part.strip())
    text = re.sub(r"\s+", " ", text).strip()
    if text:
        output.append(text)
    paragraph.clear()


def compact_text(text: str) -> str:
    lines = text.replace("\r\n", "\n").replace("\r", "\n").split("\n")
    output: list[str] = []
    paragraph: list[str] = []
    index = 0

    while index < len(lines):
        line = lines[index].rstrip()
        stripped = line.strip()

        if is_separator(line):
            flush_paragraph(output, paragraph)
            output.append("---")
            index += 1
            continue

        if is_table_line(line):
            flush_paragraph(output, paragraph)
            block: list[str] = []
            while index < len(lines) and is_table_line(lines[index]):
                block.append(lines[index].rstrip())
                index += 1
            compacted = compact_table(block)
            if compacted:
                output.append(compacted)
            continue

        if is_graphic_line(line):
            flush_paragraph(output, paragraph)
            block = []
            while index < len(lines) and (is_graphic_line(lines[index]) or not lines[index].strip()):
                if lines[index].strip():
                    block.append(lines[index].rstrip())
                index += 1
            compacted = compact_graphic(block)
            if compacted:
                output.append(compacted)
            continue

        if stripped in {"КОРИСТУВАЧ:", "АСИСТЕНТ:"}:
            flush_paragraph(output, paragraph)
            output.append(stripped)
            index += 1
            continue

        if not stripped:
            flush_paragraph(output, paragraph)
            index += 1
            continue

        paragraph.append(line)
        index += 1

    flush_paragraph(output, paragraph)
    compacted = "\n".join(output)
    compacted = re.sub(r"\n{3,}", "\n\n", compacted)
    compacted = compacted.replace("✔", "+").replace("✖", "x").replace("🔴", "[CRITICAL]")
    return compacted.strip() + "\n"


def main() -> None:
    args = parse_args()
    args.output.parent.mkdir(parents=True, exist_ok=True)
    args.output.write_text(compact_text(args.input.read_text(encoding="utf-8")), encoding="utf-8")


if __name__ == "__main__":
    main()
