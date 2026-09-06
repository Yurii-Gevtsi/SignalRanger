"""Build the six DesertSciFi fragments from the marked production layout.

The red layout master is only an authoring guide. This tool first produces an
alpha-safe, full-resolution reference crop for every cell. Each reference crop
must then receive its own image-to-image enhancement render. A final runtime
visual is never allowed to be a plain resized master crop: ``--promote`` refuses
to run until all six enhancement renders have been supplied and verified.

The reference alpha remains authoritative when applying an enhancement. That
protects the approved crop, placement, silhouette, transparent padding and
collision mask while allowing the enhancement pass to add pixel detail within
the visible artwork.
"""

from __future__ import annotations

import argparse
import csv
import hashlib
import json
import shutil
from collections import deque
from dataclasses import dataclass
from pathlib import Path

from PIL import Image, ImageChops, ImageFilter


ROOT = Path(__file__).resolve().parents[2]
ARCHIVE_SOURCE_DIR = ROOT / "art_source/_archive/desert_sci_fi_20260905_cleanup"
MARKED_MASTER_PATH = ARCHIVE_SOURCE_DIR / "production_v2/layout_master_normalized.png"
CLEAN_REFERENCE_PATH = ARCHIVE_SOURCE_DIR / "production_source/layout_master.png"
OUTPUT_DIR = ROOT / "art_source/arenas/desert_sci_fi/production_v3"
RUNTIME_DIR = ROOT / "Assets/_Game/Content/Resources/Visuals/Arenas/DesertSciFi"
BASE_CANVAS_SIZE = (2048, 1152)
PRODUCTION_SCALE = 2
GUIDE_COLOR = (237, 28, 36, 255)
VISIBLE_ALPHA_THRESHOLD = 32


@dataclass(frozen=True)
class Piece:
    asset_id: str
    base_crop: tuple[int, int, int, int]
    sorting_order: int

    @property
    def production_crop(self) -> tuple[int, int, int, int]:
        return tuple(value * PRODUCTION_SCALE for value in self.base_crop)


# Bounds are the interior-inclusive cells defined by the marked red grid. A
# one-cell overlap at shared guides intentionally prevents filtering seams.
PIECES = (
    Piece("arena_desert_sci_fi_upper_left_01", (0, 0, 768, 729), 0),
    Piece("arena_desert_sci_fi_floating_large_01", (896, 128, 1234, 341), 5),
    Piece("arena_desert_sci_fi_upper_right_01", (1566, 33, 2048, 638), 0),
    Piece("arena_desert_sci_fi_lower_left_01", (0, 723, 768, 1129), 2),
    Piece("arena_desert_sci_fi_center_01", (763, 488, 1360, 1133), 3),
    Piece("arena_desert_sci_fi_lower_right_01", (1354, 632, 2048, 1126), 2),
)


def sha256(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def load_master(path: Path) -> Image.Image:
    image = Image.open(path).convert("RGBA")
    if image.size != BASE_CANVAS_SIZE:
        raise ValueError(f"{path.name}: expected {BASE_CANVAS_SIZE}, received {image.size}")
    return image


def remove_red_guides(marked: Image.Image) -> tuple[Image.Image, set[tuple[int, int]]]:
    result = marked.copy()
    pixels = result.load()
    guide_pixels = {
        (x, y)
        for y in range(result.height)
        for x in range(result.width)
        if pixels[x, y] == GUIDE_COLOR
    }
    if len(guide_pixels) < 10_000:
        raise ValueError(f"Expected marked red guide pixels, received {len(guide_pixels)}")
    for x, y in guide_pixels:
        red, green, blue, _ = pixels[x, y]
        pixels[x, y] = (red, green, blue, 0)
    return result, guide_pixels


def restore_guides_over_adjacent_art(
    cleaned_marked: Image.Image,
    clean_reference: Image.Image,
    guide_pixels: set[tuple[int, int]],
) -> Image.Image:
    """Restore guides that cover retained art, but not removed authoring canvas."""
    result = cleaned_marked.copy()
    pixels = result.load()
    reference = clean_reference.load()
    for x, y in guide_pixels:
        if reference[x, y][3] < VISIBLE_ALPHA_THRESHOLD:
            continue
        has_neighboring_art = any(
            0 <= next_x < result.width
            and 0 <= next_y < result.height
            and pixels[next_x, next_y][3] >= VISIBLE_ALPHA_THRESHOLD
            for next_x, next_y in ((x - 4, y), (x + 4, y), (x, y - 4), (x, y + 4))
        )
        if has_neighboring_art:
            pixels[x, y] = reference[x, y]
    return result


def clear_white_canvas_components(image: Image.Image) -> Image.Image:
    """Remove only large connected white authoring-canvas regions."""
    width, height = image.size
    pixels = image.load()

    def is_canvas_white(x: int, y: int) -> bool:
        red, green, blue, alpha = pixels[x, y]
        return alpha > 0 and red >= 235 and green >= 235 and blue >= 235

    visited: set[tuple[int, int]] = set()
    canvas_pixels: set[tuple[int, int]] = set()
    for start_y in range(height):
        for start_x in range(width):
            if (start_x, start_y) in visited or not is_canvas_white(start_x, start_y):
                continue
            queue: deque[tuple[int, int]] = deque(((start_x, start_y),))
            component: list[tuple[int, int]] = []
            while queue:
                x, y = queue.popleft()
                if (x, y) in visited or not is_canvas_white(x, y):
                    continue
                visited.add((x, y))
                component.append((x, y))
                for next_x, next_y in ((x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1)):
                    if 0 <= next_x < width and 0 <= next_y < height:
                        queue.append((next_x, next_y))
            if len(component) >= 512:
                canvas_pixels.update(component)

    result = image.copy()
    result_pixels = result.load()
    for x, y in canvas_pixels:
        red, green, blue, _ = result_pixels[x, y]
        result_pixels[x, y] = (red, green, blue, 0)
    return result


def scale_reference_rgba(image: Image.Image) -> Image.Image:
    """Create only the image-to-image input, with premultiplied-alpha sampling."""
    red, green, blue, alpha = image.split()
    premultiplied = tuple(ImageChops.multiply(channel, alpha) for channel in (red, green, blue))
    target_size = (image.width * PRODUCTION_SCALE, image.height * PRODUCTION_SCALE)
    resized_red, resized_green, resized_blue = (
        channel.resize(target_size, Image.Resampling.LANCZOS) for channel in premultiplied
    )
    resized_alpha = alpha.resize(target_size, Image.Resampling.LANCZOS)
    result = Image.new("RGBA", target_size)
    result_pixels = result.load()
    red_pixels = resized_red.load()
    green_pixels = resized_green.load()
    blue_pixels = resized_blue.load()
    alpha_pixels = resized_alpha.load()
    for y in range(target_size[1]):
        for x in range(target_size[0]):
            output_alpha = alpha_pixels[x, y]
            if output_alpha < VISIBLE_ALPHA_THRESHOLD:
                result_pixels[x, y] = (0, 0, 0, 0)
                continue
            result_pixels[x, y] = (
                min(255, red_pixels[x, y] * 255 // output_alpha),
                min(255, green_pixels[x, y] * 255 // output_alpha),
                min(255, blue_pixels[x, y] * 255 // output_alpha),
                output_alpha,
            )
    return result


def force_reference_alpha(image: Image.Image, reference: Image.Image) -> Image.Image:
    """Keep generated detail while restoring the approved silhouette exactly.

    Image-to-image providers can return an opaque checkerboard canvas despite
    a transparency request. Generated RGB is accepted only inside an eroded
    opaque area. The reference owns the antialiased edge band, so a baked
    canvas cannot create white fringes in the final RGBA visual.
    """
    if image.size != reference.size:
        image = image.resize(reference.size, Image.Resampling.LANCZOS)
    generated_rgb = image.convert("RGB")
    reference_rgb = reference.convert("RGB")
    alpha = reference.getchannel("A")
    opaque = alpha.point(lambda value: 255 if value >= 250 else 0, mode="L")
    # The provider's checkerboard can bleed 8-12 pixels inside a regenerated
    # silhouette. Keep a conservative 15-pixel reference edge band.
    safe_generated_area = opaque.filter(ImageFilter.MinFilter(31))
    result = Image.new("RGBA", reference.size)
    pixels = result.load()
    generated_pixels = generated_rgb.load()
    reference_pixels = reference_rgb.load()
    safe_pixels = safe_generated_area.load()
    alpha_pixels = alpha.load()
    for y in range(result.height):
        for x in range(result.width):
            output_alpha = alpha_pixels[x, y]
            if output_alpha < VISIBLE_ALPHA_THRESHOLD:
                pixels[x, y] = (0, 0, 0, 0)
            elif safe_pixels[x, y] == 255:
                pixels[x, y] = (*generated_pixels[x, y], output_alpha)
            else:
                pixels[x, y] = (*reference_pixels[x, y], output_alpha)
    return result


def make_mask(reference: Image.Image) -> Image.Image:
    alpha = reference.getchannel("A").point(
        lambda value: 255 if value >= VISIBLE_ALPHA_THRESHOLD else 0, mode="L"
    )
    opaque = Image.new("L", alpha.size, 255)
    return Image.merge("RGBA", (alpha, alpha, alpha, opaque))


def manifest_piece(piece: Piece) -> dict[str, int | str]:
    left, top, right, bottom = piece.production_crop
    return {
        "asset_id": piece.asset_id,
        "x": left,
        "y": top,
        "width": right - left,
        "height": bottom - top,
        "sorting_order": piece.sorting_order,
    }


def write_manifest(manifest_pieces: list[dict[str, int | str]]) -> None:
    manifest = {
        "canvas": {"width": BASE_CANVAS_SIZE[0] * PRODUCTION_SCALE, "height": BASE_CANVAS_SIZE[1] * PRODUCTION_SCALE},
        "pieces": manifest_pieces,
    }
    (OUTPUT_DIR / "arena_desert_sci_fi_marked_layout.json").write_text(
        json.dumps(manifest, ensure_ascii=False, indent=2) + "\n", encoding="utf-8"
    )
    with (OUTPUT_DIR / "arena_desert_sci_fi_marked_layout.csv").open("w", newline="", encoding="utf-8") as file:
        writer = csv.DictWriter(file, fieldnames=["asset_id", "x", "y", "width", "height", "sorting_order"])
        writer.writeheader()
        writer.writerows(manifest_pieces)


def create_reference_crops(master: Image.Image, reference_dir: Path) -> list[dict[str, int | str]]:
    reference_dir.mkdir(parents=True, exist_ok=True)
    manifest_pieces: list[dict[str, int | str]] = []
    for piece in PIECES:
        reference = scale_reference_rgba(master.crop(piece.base_crop))
        reference.save(reference_dir / f"{piece.asset_id}_reference.png")
        manifest_pieces.append(manifest_piece(piece))
    return manifest_pieces


def apply_enhancements(
    manifest_pieces: list[dict[str, int | str]],
    reference_dir: Path,
    enhanced_dir: Path,
    visual_dir: Path,
    mask_dir: Path,
) -> list[dict[str, object]]:
    visual_dir.mkdir(parents=True, exist_ok=True)
    mask_dir.mkdir(parents=True, exist_ok=True)
    quality_records: list[dict[str, object]] = []
    missing: list[str] = []
    for piece in manifest_pieces:
        asset_id = str(piece["asset_id"])
        reference_path = reference_dir / f"{asset_id}_reference.png"
        enhanced_path = enhanced_dir / f"{asset_id}_enhanced.png"
        if not enhanced_path.is_file():
            missing.append(enhanced_path.name)
            continue
        reference = Image.open(reference_path).convert("RGBA")
        enhanced = Image.open(enhanced_path).convert("RGBA")
        visual = force_reference_alpha(enhanced, reference)
        visual_path = visual_dir / f"{asset_id}_visual.png"
        mask_path = mask_dir / f"{asset_id}_mask.png"
        visual.save(visual_path)
        make_mask(reference).save(mask_path)
        quality_records.append(
            {
                "asset_id": asset_id,
                "quality_pass": "individual_image_to_image_enhancement",
                "reference_file": reference_path.name,
                "reference_sha256": sha256(reference_path),
                "enhanced_file": enhanced_path.name,
                "enhanced_sha256": sha256(enhanced_path),
                "final_visual_sha256": sha256(visual_path),
                "canvas_width": reference.width,
                "canvas_height": reference.height,
                "alpha_source": "reference_crop",
            }
        )
    if missing:
        raise FileNotFoundError(
            "No fallback resize is permitted. Missing individual enhancement renders: " + ", ".join(missing)
        )
    return quality_records


def write_quality_manifest(quality_records: list[dict[str, object]]) -> None:
    quality_manifest = {
        "rule": "Every runtime fragment must be individually image-to-image enhanced after cropping; plain master resizing is input-only and can never be promoted.",
        "alpha_policy": "The accepted reference crop alpha is reapplied after enhancement to preserve crop alignment, transparent padding and the paired mask.",
        "pieces": quality_records,
    }
    (OUTPUT_DIR / "quality_pass_manifest.json").write_text(
        json.dumps(quality_manifest, ensure_ascii=False, indent=2) + "\n", encoding="utf-8"
    )


def write_review(master: Image.Image, manifest_pieces: list[dict[str, int | str]], visual_dir: Path) -> None:
    canvas_size = (BASE_CANVAS_SIZE[0] * PRODUCTION_SCALE, BASE_CANVAS_SIZE[1] * PRODUCTION_SCALE)
    reconstructed = Image.new("RGBA", canvas_size)
    for piece in manifest_pieces:
        visual = Image.open(visual_dir / f"{piece['asset_id']}_visual.png").convert("RGBA")
        reconstructed.alpha_composite(visual, (int(piece["x"]), int(piece["y"])))
    reconstructed.save(OUTPUT_DIR / "review_reconstructed_transparent.png")

    reference = scale_reference_rgba(master)
    ImageChops.difference(reference, reconstructed).save(OUTPUT_DIR / "review_reconstruction_difference.png")
    for name, color in (("dark", (20, 24, 33, 255)), ("light", (244, 246, 248, 255)), ("blue", (22, 90, 160, 255))):
        preview = Image.new("RGBA", canvas_size, color)
        preview.alpha_composite(reconstructed)
        preview.convert("RGB").save(OUTPUT_DIR / f"review_reconstructed_{name}.png")


def verify_output(
    manifest_pieces: list[dict[str, int | str]], visual_dir: Path, mask_dir: Path, quality_records: list[dict[str, object]]
) -> None:
    if len(quality_records) != len(manifest_pieces):
        raise ValueError("Every fragment requires its own verified image-to-image quality record.")
    for piece in manifest_pieces:
        asset_id = str(piece["asset_id"])
        visual = Image.open(visual_dir / f"{asset_id}_visual.png").convert("RGBA")
        mask = Image.open(mask_dir / f"{asset_id}_mask.png").convert("RGBA")
        expected = (int(piece["width"]), int(piece["height"]))
        if visual.size != expected or mask.size != expected:
            raise ValueError(f"{asset_id}: expected paired size {expected}, got {visual.size}/{mask.size}")
        if mask.getchannel("A").getextrema() != (255, 255):
            raise ValueError(f"{asset_id}: mask alpha must be fully opaque")
        if not set(mask.getchannel("R").get_flattened_data()).issubset({0, 255}):
            raise ValueError(f"{asset_id}: mask contains non-binary values")
        if visual.getchannel("A").getextrema()[0] != 0:
            raise ValueError(f"{asset_id}: visual is missing transparent padding")


def promote(manifest_pieces: list[dict[str, int | str]], visual_dir: Path, mask_dir: Path) -> None:
    for piece in manifest_pieces:
        asset_id = str(piece["asset_id"])
        shutil.copy2(visual_dir / f"{asset_id}_visual.png", RUNTIME_DIR / f"{asset_id}_visual.png")
        shutil.copy2(mask_dir / f"{asset_id}_mask.png", RUNTIME_DIR / f"{asset_id}_mask.png")


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--apply-enhancements", action="store_true", help="Apply the six individual image-to-image enhanced renders.")
    parser.add_argument("--promote", action="store_true", help="Copy verified enhanced pairs into Unity runtime assets.")
    args = parser.parse_args()
    if args.promote and not args.apply_enhancements:
        parser.error("--promote requires --apply-enhancements; raw resized crops are never runtime outputs.")

    marked = load_master(MARKED_MASTER_PATH)
    clean_reference = load_master(CLEAN_REFERENCE_PATH)
    guide_free_master, guide_pixels = remove_red_guides(marked)
    master = restore_guides_over_adjacent_art(
        clear_white_canvas_components(guide_free_master), clean_reference, guide_pixels
    )
    reference_dir = OUTPUT_DIR / "source_layers/reference_crops"
    enhanced_dir = OUTPUT_DIR / "source_layers/enhanced_renders"
    visual_dir = OUTPUT_DIR / "visuals"
    mask_dir = OUTPUT_DIR / "masks"
    manifest_pieces = create_reference_crops(master, reference_dir)
    write_manifest(manifest_pieces)
    if not args.apply_enhancements:
        print(json.dumps({"prepared_references": len(manifest_pieces), "output": str(reference_dir)}, indent=2))
        return

    quality_records = apply_enhancements(manifest_pieces, reference_dir, enhanced_dir, visual_dir, mask_dir)
    write_quality_manifest(quality_records)
    write_review(master, manifest_pieces, visual_dir)
    verify_output(manifest_pieces, visual_dir, mask_dir, quality_records)
    if args.promote:
        promote(manifest_pieces, visual_dir, mask_dir)
    print(json.dumps({"pieces": manifest_pieces, "promoted": args.promote}, indent=2))


if __name__ == "__main__":
    main()
