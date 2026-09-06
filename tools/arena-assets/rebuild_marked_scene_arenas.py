"""Rebuild marked arena layouts into individually enhanced Unity fragments.

Plain resizing prepares a reference only.  Every fragment must have its own
``*_enhanced.png`` in ``source_layers/enhanced_renders`` before ``--promote``
can copy anything to Unity.  The approved reference alpha is retained in the
final visual and in its paired collision mask, which keeps the marked layout
and all fragment seams deterministic.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import shutil
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageChops, ImageFilter


ROOT = Path(__file__).resolve().parents[2]
MASTER_SIZE = (2048, 1152)
SCALE = 2
ALPHA_THRESHOLD = 32


@dataclass(frozen=True)
class Piece:
    name: str
    crop: tuple[int, int, int, int]
    order: int
    direct_source: str | None = None
    source_mask: str | None = None


@dataclass(frozen=True)
class Arena:
    key: str
    runtime_folder: str
    source: str | None
    marked: str | None
    pieces: tuple[Piece, ...]


ARENAS = {
    "city": Arena(
        "city", "City", "production_source/layout_master.png", "production_source/layout_master_секції.png",
        (Piece("left", (0, 60, 874, 1131), -8), Piece("center", (596, 411, 1444, 1138), -7), Piece("right", (1166, 159, 2048, 1138), -6)),
    ),
    "forest": Arena(
        "forest", "Forest", "production_source/ліс.png", "production_source/ліс_секції.png",
        (Piece("left", (80, 285, 760, 1130), -8), Piece("center", (620, 600, 1310, 1135), -7), Piece("right", (1210, 30, 2048, 1135), -6)),
    ),
    "grib": Arena(
        "grib", "Grib", "production_source/гриби.png", "production_source/гриби_секції.png",
        (Piece("left", (0, 0, 825, 1152), -8), Piece("center", (540, 0, 1320, 1152), -7), Piece("right", (1180, 0, 2048, 1152), -6)),
    ),
    "laguna": Arena(
        "laguna", "Laguna", "production_source/острови.png", "production_source/острови_секції.png",
        (Piece("left", (0, 75, 1125, 1100), -8), Piece("center", (490, 285, 1150, 1100), -7), Piece("right", (1080, 55, 2048, 1110), -6)),
    ),
    "monument": Arena(
        "monument", "Monument", "production_source/layout_master.png", "production_source/монумент_секції.png",
        (Piece("left", (0, 160, 844, 1062), -8), Piece("center", (496, 77, 1534, 1058), -7), Piece("right", (1186, 236, 2048, 1063), -6)),
    ),
    "kraken": Arena(
        "kraken", "Kraken", None, None,
        (
            Piece("left", (0, 128, 794, 1143), -8, "production_source/kraken_left_visual.png", "production_source/kraken_left_mask.png"),
            Piece("center", (566, 64, 1394, 1152), -7, "production_source/kraken_center_visual.png", "production_source/kraken_center_mask.png"),
            Piece("right", (1096, 207, 2048, 1152), -6, "production_source/kraken_right_visual.png", "production_source/kraken_right_mask.png"),
        ),
    ),
}


def digest(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def scale_rgba(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    """Resize with premultiplied alpha to avoid transparent-background bleed."""
    red, green, blue, alpha = image.convert("RGBA").split()
    premultiplied = [ImageChops.multiply(channel, alpha).resize(size, Image.Resampling.LANCZOS) for channel in (red, green, blue)]
    output_alpha = alpha.resize(size, Image.Resampling.LANCZOS)
    result = Image.new("RGBA", size)
    destination = result.load()
    channels = [channel.load() for channel in premultiplied]
    alpha_pixels = output_alpha.load()
    for y in range(size[1]):
        for x in range(size[0]):
            value = alpha_pixels[x, y]
            if value < ALPHA_THRESHOLD:
                destination[x, y] = (0, 0, 0, 0)
            else:
                destination[x, y] = tuple(min(255, channel[x, y] * 255 // value) for channel in channels) + (value,)
    return result


def mask_for(reference: Image.Image) -> Image.Image:
    alpha = reference.getchannel("A").point(lambda value: 255 if value >= ALPHA_THRESHOLD else 0, mode="L")
    return Image.merge("RGBA", (alpha, alpha, alpha, Image.new("L", alpha.size, 255)))


def final_from_enhancement(enhanced: Image.Image, reference: Image.Image) -> Image.Image:
    """Use detail from the render but reference RGB/alpha near all boundaries."""
    if enhanced.size != reference.size:
        enhanced = enhanced.resize(reference.size, Image.Resampling.LANCZOS)
    generated = enhanced.convert("RGB")
    reference_rgb = reference.convert("RGB")
    alpha = reference.getchannel("A")
    safe_generated = alpha.point(lambda value: 255 if value >= 250 else 0, mode="L").filter(ImageFilter.MinFilter(31))
    result = Image.new("RGBA", reference.size)
    output = result.load()
    generated_pixels = generated.load()
    reference_pixels = reference_rgb.load()
    alpha_pixels = alpha.load()
    safe_pixels = safe_generated.load()
    for y in range(result.height):
        for x in range(result.width):
            value = alpha_pixels[x, y]
            if value < ALPHA_THRESHOLD:
                output[x, y] = (0, 0, 0, 0)
            elif safe_pixels[x, y] == 255:
                output[x, y] = (*generated_pixels[x, y], value)
            else:
                output[x, y] = (*reference_pixels[x, y], value)
    return result


def paths(arena: Arena) -> tuple[Path, Path, Path, Path, Path]:
    output = ROOT / "art_source/arenas" / arena.key / "production_v2"
    return output, output / "source_layers/reference_crops", output / "source_layers/enhanced_renders", output / "visuals", output / "masks"


def reference_master(arena: Arena) -> Image.Image:
    if arena.source is None:
        raise ValueError(f"{arena.key} uses individual source pieces")
    image = Image.open(ROOT / "art_source/arenas" / arena.key / arena.source).convert("RGBA")
    return scale_rgba(image, MASTER_SIZE) if image.size != MASTER_SIZE else image


def prepare(arena: Arena) -> list[dict[str, int | str]]:
    output, reference_dir, _, _, _ = paths(arena)
    output.mkdir(parents=True, exist_ok=True)
    reference_dir.mkdir(parents=True, exist_ok=True)
    master = reference_master(arena) if arena.source else None
    manifest: list[dict[str, int | str]] = []
    for piece in arena.pieces:
        if piece.direct_source:
            original = Image.open(ROOT / "art_source/arenas" / arena.key / piece.direct_source).convert("RGBA")
            reference = scale_rgba(original, (original.width * SCALE, original.height * SCALE))
        else:
            crop = master.crop(piece.crop)
            reference = scale_rgba(crop, (crop.width * SCALE, crop.height * SCALE))
        reference_path = reference_dir / f"{arena.key}_{piece.name}_reference.png"
        reference.save(reference_path)
        x0, y0, x1, y1 = piece.crop
        manifest.append({"id": piece.name, "x": x0, "y": y0, "width": x1 - x0, "height": y1 - y0, "sorting_order": piece.order, "reference": reference_path.name})
    (output / f"{arena.key}_layout.json").write_text(json.dumps({"master": MASTER_SIZE, "pieces": manifest}, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    return manifest


def apply(arena: Arena, manifest: list[dict[str, int | str]]) -> None:
    output, reference_dir, enhanced_dir, visual_dir, mask_dir = paths(arena)
    visual_dir.mkdir(parents=True, exist_ok=True)
    mask_dir.mkdir(parents=True, exist_ok=True)
    records: list[dict[str, str | int]] = []
    missing: list[str] = []
    for item in manifest:
        piece_id = str(item["id"])
        reference_path = reference_dir / f"{arena.key}_{piece_id}_reference.png"
        enhanced_path = enhanced_dir / f"{arena.key}_{piece_id}_enhanced.png"
        if not enhanced_path.exists():
            missing.append(enhanced_path.name)
            continue
        reference = Image.open(reference_path).convert("RGBA")
        final = final_from_enhancement(Image.open(enhanced_path).convert("RGBA"), reference)
        visual_path = visual_dir / f"{arena.key}_{piece_id}_visual.png"
        mask_path = mask_dir / f"{arena.key}_{piece_id}_mask.png"
        final.save(visual_path)
        mask_for(reference).save(mask_path)
        records.append({"id": piece_id, "quality_pass": "individual_image_to_image_enhancement", "reference_sha256": digest(reference_path), "enhanced_sha256": digest(enhanced_path), "final_sha256": digest(visual_path), "width": final.width, "height": final.height})
    if missing:
        raise FileNotFoundError("No resize fallback is allowed. Missing enhanced renders: " + ", ".join(missing))
    (output / "quality_pass_manifest.json").write_text(json.dumps({"rule": "Every fragment is individually enhanced after crop; plain resize is reference-only.", "pieces": records}, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    rebuild_review(arena, manifest, visual_dir)
    verify(arena, manifest, visual_dir, mask_dir, records)


def rebuild_review(arena: Arena, manifest: list[dict[str, int | str]], visual_dir: Path) -> None:
    output, *_ = paths(arena)
    reconstructed = Image.new("RGBA", (MASTER_SIZE[0] * SCALE, MASTER_SIZE[1] * SCALE))
    for item in manifest:
        visual = Image.open(visual_dir / f"{arena.key}_{item['id']}_visual.png").convert("RGBA")
        reconstructed.alpha_composite(visual, (int(item["x"]) * SCALE, int(item["y"]) * SCALE))
    reconstructed.save(output / "review_reconstructed_transparent.png")
    for name, color in (("dark", (20, 24, 33, 255)), ("light", (244, 246, 248, 255)), ("blue", (22, 90, 160, 255))):
        preview = Image.new("RGBA", reconstructed.size, color)
        preview.alpha_composite(reconstructed)
        preview.convert("RGB").save(output / f"review_reconstructed_{name}.png")


def verify(arena: Arena, manifest: list[dict[str, int | str]], visual_dir: Path, mask_dir: Path, records: list[dict[str, str | int]]) -> None:
    if len(records) != len(manifest):
        raise ValueError(f"{arena.key}: every piece requires a quality record")
    for item in manifest:
        name = str(item["id"])
        visual = Image.open(visual_dir / f"{arena.key}_{name}_visual.png").convert("RGBA")
        mask = Image.open(mask_dir / f"{arena.key}_{name}_mask.png").convert("RGBA")
        expected = (int(item["width"]) * SCALE, int(item["height"]) * SCALE)
        if visual.size != expected or mask.size != expected:
            raise ValueError(f"{arena.key}/{name}: pair dimensions {visual.size}/{mask.size}, expected {expected}")
        if visual.getchannel("A").getextrema()[0] != 0:
            raise ValueError(f"{arena.key}/{name}: visual has no transparent padding")
        if not set(mask.getchannel("R").get_flattened_data()).issubset({0, 255}):
            raise ValueError(f"{arena.key}/{name}: mask is not binary")


def promote(arena: Arena, manifest: list[dict[str, int | str]]) -> None:
    _, _, _, visual_dir, mask_dir = paths(arena)
    runtime = ROOT / "Assets/_Game/Content/Resources/Visuals/Arenas" / arena.runtime_folder
    runtime.mkdir(parents=True, exist_ok=True)
    for item in manifest:
        name = str(item["id"])
        shutil.copy2(visual_dir / f"{arena.key}_{name}_visual.png", runtime / f"{arena.key}_{name}_visual.png")
        shutil.copy2(mask_dir / f"{arena.key}_{name}_mask.png", runtime / f"{arena.key}_{name}_mask.png")


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--arenas", nargs="+", choices=sorted(ARENAS), default=sorted(ARENAS))
    parser.add_argument("--apply-enhancements", action="store_true")
    parser.add_argument("--promote", action="store_true")
    args = parser.parse_args()
    if args.promote and not args.apply_enhancements:
        parser.error("--promote requires --apply-enhancements; resized reference crops are not runtime assets")
    result = {}
    for key in args.arenas:
        arena = ARENAS[key]
        manifest = prepare(arena)
        if args.apply_enhancements:
            apply(arena, manifest)
            if args.promote:
                promote(arena, manifest)
        result[key] = len(manifest)
    print(json.dumps({"prepared": result, "promoted": args.promote}, indent=2))


if __name__ == "__main__":
    main()
