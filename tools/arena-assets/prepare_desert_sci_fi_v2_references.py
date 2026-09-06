#!/usr/bin/env python3
"""Prepare fixed-layout transparent reference crops for desert arena v2 renders."""

from __future__ import annotations

import csv
import json
import shutil
from pathlib import Path

from PIL import Image


ROOT = Path(__file__).resolve().parents[2]
MASTER_DIR = ROOT / "art_source/arenas/desert_sci_fi/master"
SOURCE = ROOT / "art_source/arenas/desert_sci_fi/production_v1"
OUTPUT = ROOT / "art_source/arenas/desert_sci_fi/production_v2"
MASTER_SIZE = (2048, 1152)
PRODUCTION_SCALE = 2
TRANSPARENT_RENDER_CONTRACT = """# Desert Sci-Fi fragment generation contract

Generate each reference crop as an individual PNG with a genuine RGBA alpha channel.

- Preserve the crop's composition, silhouette, placement, proportions and local lighting.
- Improve only material detail and edge clarity; do not add or remove terrain.
- The entire backdrop must be truly transparent (alpha = 0), not magenta, green,
  checkerboard, black, white, or another baked background.
- Do not add a floor, shadow, glow, particles, text, interface, characters or scenery.
- Keep all visible pixels inside the original crop bounds.

The pipeline rejects opaque or colour-keyed images. The immutable collision mask
remains the final geometry authority.
"""


def save_png_atomically(image: Image.Image, path: Path) -> None:
    """Avoid partially written production previews if Unity or a viewer observes them."""
    staged = path.with_name(f"{path.stem}.staged{path.suffix}")
    image.save(staged)
    staged.replace(path)


def main() -> None:
    masters = sorted(MASTER_DIR.glob("*.png"))
    if len(masters) != 1:
        raise RuntimeError(f"Expected exactly one final PNG in {MASTER_DIR}, found {len(masters)}.")

    output_references = OUTPUT / "source_layers/reference_crops"
    output_renders = OUTPUT / "source_layers/transparent_renders"
    output_masks = OUTPUT / "masks"
    output_references.mkdir(parents=True, exist_ok=True)
    output_renders.mkdir(parents=True, exist_ok=True)
    output_masks.mkdir(parents=True, exist_ok=True)

    master = Image.open(masters[0]).convert("RGBA")
    normalized_master = master.resize(MASTER_SIZE, Image.Resampling.LANCZOS)
    save_png_atomically(normalized_master, OUTPUT / "layout_master_normalized.png")

    pieces_path = SOURCE / "arena_desert_sci_fi_pieces.csv"
    with pieces_path.open(newline="", encoding="utf-8") as handle:
        rows = list(csv.DictReader(handle))

    for row in rows:
        piece_id = row["piece_id"]
        x = int(row["x_px"]) // PRODUCTION_SCALE
        y = int(row["y_px"]) // PRODUCTION_SCALE
        width = int(row["width_px"]) // PRODUCTION_SCALE
        height = int(row["height_px"]) // PRODUCTION_SCALE
        reference = normalized_master.crop((x, y, x + width, y + height)).resize(
            (width * PRODUCTION_SCALE, height * PRODUCTION_SCALE),
            Image.Resampling.LANCZOS,
        )
        save_png_atomically(reference, output_references / f"{piece_id}_reference.png")
        shutil.copy2(SOURCE / row["mask_file"], OUTPUT / row["mask_file"])

    shutil.copy2(SOURCE / "arena_desert_sci_fi_pieces.csv", OUTPUT / "arena_desert_sci_fi_pieces.csv")
    shutil.copy2(SOURCE / "arena_desert_sci_fi_instances.csv", OUTPUT / "arena_desert_sci_fi_instances.csv")
    layout = json.loads((SOURCE / "arena_desert_sci_fi_layout.json").read_text(encoding="utf-8"))
    layout["source_master"] = str(masters[0].relative_to(ROOT)).replace("\\", "/")
    layout["source_master_size_px"] = list(master.size)
    layout["normalized_master_size_px"] = list(MASTER_SIZE)
    layout["visual_generation"] = "individual image-to-image enhancement from exact fixed-layout reference crops"
    (OUTPUT / "arena_desert_sci_fi_layout.json").write_text(
        json.dumps(layout, indent=2) + "\n",
        encoding="utf-8",
    )
    (OUTPUT / "source_layers/generation_contract.md").write_text(
        TRANSPARENT_RENDER_CONTRACT,
        encoding="utf-8",
    )


if __name__ == "__main__":
    main()
