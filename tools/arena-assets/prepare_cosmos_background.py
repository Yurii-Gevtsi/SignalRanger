#!/usr/bin/env python3
"""Prepare the generated Cosmos background at the battle camera's native canvas size."""

from pathlib import Path

from PIL import Image


ROOT = Path(__file__).resolve().parents[2]
SOURCE = ROOT / "art_source/arenas/forest/production_source/cosmos_generated.png"
OUTPUT = ROOT / "Assets/_Game/Content/Resources/Visuals/Arenas/Backgrounds/Cosmos/cosmos.png"
TARGET_SIZE = (2048, 1152)


def main() -> None:
    image = Image.open(SOURCE).convert("RGB")
    fitted = Image.new("RGB", TARGET_SIZE)
    scale = max(TARGET_SIZE[0] / image.width, TARGET_SIZE[1] / image.height)
    resized = image.resize((round(image.width * scale), round(image.height * scale)), Image.Resampling.LANCZOS)
    offset = ((resized.width - TARGET_SIZE[0]) // 2, (resized.height - TARGET_SIZE[1]) // 2)
    fitted.paste(resized.crop((offset[0], offset[1], offset[0] + TARGET_SIZE[0], offset[1] + TARGET_SIZE[1])))
    OUTPUT.parent.mkdir(parents=True, exist_ok=True)
    fitted.save(OUTPUT)


if __name__ == "__main__":
    main()
