"""Build high-resolution arena fragments from the artist's red contour guides.

The guide is an authoring-only boundary, never a shipped source.  Each closed
red outline identifies a real alpha-shaped fragment; unlike the retired v2
tool, this tool does not turn guides into broad rectangular layout crops.

Usage:
  python tools/arena-assets/rebuild_guide_contour_arenas.py --arenas city --prepare
  # Place one generated ``*_enhanced.png`` beside every reference crop, then:
  python tools/arena-assets/rebuild_guide_contour_arenas.py --arenas city --apply-enhancements --promote
"""

from __future__ import annotations

import argparse
import hashlib
import json
import shutil
from dataclasses import dataclass
from pathlib import Path

import numpy as np
from PIL import Image, ImageChops, ImageFilter
from scipy import ndimage


ROOT = Path(__file__).resolve().parents[2]
CANVAS = (2048, 1152)
SCALE = 2
GUIDE_DILATION = 10
ALPHA_THRESHOLD = 32
PADDING = 16

STYLE_SPEC = (
    "Stylized hand-painted 2D sci-fi game illustration; large clean shapes; "
    "soft cel-shading; light coloured shadows without deep black; moderate "
    "contrast; warm diffused light; restrained fine texture, scratches and "
    "micro-cracks; clear silhouettes and large readable colour planes; slightly "
    "muted but lively pastel turquoise, green, sand, cream and warm orange "
    "palette; background simpler, lighter and lower contrast than foreground "
    "with gentle atmospheric perspective; clean lines without excessive "
    "sharpening, HDR or coloured halos; hand-painted and expressive, not "
    "childish or overly cartoonish; no photorealism or excessive material "
    "micro-detail."
)


@dataclass(frozen=True)
class GuidedArena:
    key: str
    runtime_folder: str
    source_file: str
    guide_file: str
    expected_sections: int


@dataclass(frozen=True)
class KrakenPiece:
    name: str
    crop: tuple[int, int, int, int]
    sorting_order: int


GUIDED_ARENAS = {
    "city": GuidedArena("city", "City", "layout_master.png", "layout_master_секції.png", 5),
    "forest": GuidedArena("forest", "Forest", "ліс.png", "ліс_секції.png", 4),
    "grib": GuidedArena("grib", "Grib", "гриби.png", "гриби_секції.png", 5),
    "laguna": GuidedArena("laguna", "Laguna", "острови.png", "острови_секції.png", 5),
    "monument": GuidedArena("monument", "Monument", "layout_master.png", "монумент_секції.png", 4),
}

KRAKEN_PIECES = (
    KrakenPiece("left", (0, 128, 794, 1143), -8),
    KrakenPiece("center", (566, 64, 1394, 1152), -7),
    KrakenPiece("right", (1096, 207, 2048, 1152), -6),
)


def sha256(path: Path) -> str:
    return hashlib.sha256(path.read_bytes()).hexdigest()


def scale_rgba(image: Image.Image, size: tuple[int, int]) -> Image.Image:
    """Resize RGBA without sampling transparent/background RGB into its edge."""
    image = image.convert("RGBA")
    red, green, blue, alpha = image.split()
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


def clean_alpha(image: Image.Image, arena_key: str) -> Image.Image:
    """Convert opaque authoring masters with black/dark canvas into true alpha.

    Forest has a charcoal rather than literal-black canvas, so only its neutral
    large background components are removed.  The other two opaque masters use
    a literal black canvas; removing only near-zero pixels keeps ink outlines.
    """
    result = image.convert("RGBA")
    array = np.asarray(result).copy()
    rgb = array[:, :, :3].astype(np.int16)
    if arena_key in {"grib", "laguna"}:
        # These masters were exported against black rather than alpha. The
        # black canvas also appears as detached islands inside arches and glow
        # surrounds, so clear it globally. The cutoff stays below painted
        # brown/blue shading, preserving illustrated structure and outlines.
        background = rgb.max(axis=2) <= 30
    elif arena_key == "forest":
        neutral = (rgb.max(axis=2) <= 62) & ((rgb.max(axis=2) - rgb.min(axis=2)) <= 10)
        labels, count = ndimage.label(neutral)
        edge_labels = set(np.unique(np.concatenate((labels[0], labels[-1], labels[:, 0], labels[:, -1]))))
        background = np.isin(labels, list(edge_labels))
        # Authoring holes (doors/caves) are also canvas when large and neutral.
        sizes = np.bincount(labels.ravel(), minlength=count + 1)
        background |= neutral & (sizes[labels] >= 512)
    else:
        background = array[:, :, 3] < ALPHA_THRESHOLD
    array[:, :, 3][background] = 0
    array[:, :, :3][background] = 0
    return Image.fromarray(array, "RGBA")


def red_guide_mask(image: Image.Image) -> np.ndarray:
    array = np.asarray(image.convert("RGBA"))
    red, green, blue, alpha = (array[:, :, index] for index in range(4))
    guide = (red >= 175) & (green <= 125) & (blue <= 135) & (alpha >= 32) & ((red.astype(np.int16) - green.astype(np.int16)) >= 75)
    if int(guide.sum()) < 500:
        raise ValueError("The marked master contains too few red guide pixels.")
    return guide


def enclosed_sections(guide: np.ndarray, visible: np.ndarray, expected_count: int) -> list[np.ndarray]:
    """Return one source-alpha mask per guide-enclosed face.

    Dilating only the guide closes minor freehand gaps.  The resulting guide
    band is then assigned to the nearest selected face so no source pixel is
    lost as a visual or collision seam.
    """
    structure = np.ones((2 * GUIDE_DILATION + 1, 2 * GUIDE_DILATION + 1), dtype=bool)
    boundary = ndimage.binary_dilation(guide, structure=structure)
    boundary[[0, -1], :] = True
    boundary[:, [0, -1]] = True
    labels, count = ndimage.label(~boundary)
    area = np.bincount(labels.ravel(), minlength=count + 1)
    visible_area = np.bincount(labels[visible].ravel(), minlength=count + 1)
    candidates = [label for label in range(1, count + 1) if int(visible_area[label]) >= 60_000]
    if len(candidates) != expected_count:
        detail = sorted((int(visible_area[label]), int(area[label]), label) for label in range(1, count + 1) if visible_area[label])
        raise ValueError(f"Guide selected {len(candidates)} sections; expected {expected_count}. Components: {detail}")

    selected_seed = np.isin(labels, candidates)
    _, nearest = ndimage.distance_transform_edt(~selected_seed, return_indices=True)
    nearest_label = labels[nearest[0], nearest[1]]
    assigned = visible & np.isin(nearest_label, candidates)
    if int(assigned.sum()) != int(visible.sum()):
        raise ValueError("The guide leaves source-art pixels unassigned.")
    sections = [(nearest_label == label) & visible for label in candidates]
    # Sort by original guide-face order: top-to-bottom, then left-to-right.
    def sort_key(section: np.ndarray) -> tuple[int, int]:
        ys, xs = np.where(section)
        return int(ys.min()), int(xs.min())
    return sorted(sections, key=sort_key)


def crop_section(master: Image.Image, section: np.ndarray) -> tuple[Image.Image, tuple[int, int, int, int]]:
    ys, xs = np.where(section)
    left = max(0, int(xs.min()) - PADDING)
    top = max(0, int(ys.min()) - PADDING)
    right = min(master.width, int(xs.max()) + 1 + PADDING)
    bottom = min(master.height, int(ys.max()) + 1 + PADDING)
    array = np.asarray(master).copy()
    array[~section] = (0, 0, 0, 0)
    crop = Image.fromarray(array, "RGBA").crop((left, top, right, bottom))
    return crop, (left, top, right, bottom)


def mask_for(reference: Image.Image) -> Image.Image:
    alpha = reference.getchannel("A").point(lambda value: 255 if value >= ALPHA_THRESHOLD else 0, mode="L")
    return Image.merge("RGBA", (alpha, alpha, alpha, Image.new("L", alpha.size, 255)))


def final_from_enhancement(enhanced: Image.Image, reference: Image.Image) -> Image.Image:
    """Retain generated micro-detail while keeping master colour continuous.

    Every section is enhanced separately. Replacing its full RGB would therefore
    make slightly different generator lighting visible at the contour join. Use
    only the generated high-frequency residual over the reference colour field;
    this increases close-zoom texture without turning the master into a patchwork.
    """
    if enhanced.size != reference.size:
        enhanced = enhanced.resize(reference.size, Image.Resampling.LANCZOS)
    generated = enhanced.convert("RGB")
    reference_rgb = reference.convert("RGB")
    alpha = reference.getchannel("A")
    safe = alpha.point(lambda value: 255 if value >= 250 else 0, mode="L").filter(ImageFilter.MinFilter(31))
    generated_array = np.asarray(generated, dtype=np.int16)
    generated_blur = np.asarray(generated.filter(ImageFilter.GaussianBlur(radius=8)), dtype=np.int16)
    reference_array = np.asarray(reference_rgb, dtype=np.int16)
    # Structural changes from a generative renderer are not safe near an edge
    # or where its colour diverges from the approved master. Admit only a small
    # high-frequency residual on close-matching, visually flat material. That
    # is enough to improve close zoom while guaranteeing a seamless master
    # colour field across independently generated neighbours.
    edge_x = np.abs(np.diff(reference_array, axis=1, prepend=reference_array[:, :1]))
    edge_y = np.abs(np.diff(reference_array, axis=0, prepend=reference_array[:1, :]))
    flat_reference = np.maximum(edge_x, edge_y).max(axis=2) <= 20
    close_colour = np.abs(generated_array - reference_array).max(axis=2) <= 35
    accepted_detail = flat_reference & close_colour
    detailed_rgb = reference_array.copy()
    residual = (generated_array - generated_blur) * 0.20
    detailed_rgb[accepted_detail] = np.clip(reference_array[accepted_detail] + residual[accepted_detail], 0, 255)
    detailed_rgb = detailed_rgb.astype(np.uint8)
    result = Image.new("RGBA", reference.size)
    output, generated_pixels, reference_pixels = result.load(), generated.load(), reference_rgb.load()
    alpha_pixels, safe_pixels = alpha.load(), safe.load()
    for y in range(result.height):
        for x in range(result.width):
            value = alpha_pixels[x, y]
            if value < ALPHA_THRESHOLD:
                output[x, y] = (0, 0, 0, 0)
            elif safe_pixels[x, y] == 255:
                output[x, y] = (*detailed_rgb[y, x], value)
            else:
                output[x, y] = (*reference_pixels[x, y], value)
    return result


def output_paths(key: str) -> dict[str, Path]:
    root = ROOT / "art_source/arenas" / key / "production_v3"
    return {
        "root": root,
        "reference": root / "source_layers/reference_crops",
        "enhanced": root / "source_layers/enhanced_renders",
        "visuals": root / "visuals",
        "masks": root / "masks",
    }


def prepare_guided(arena: GuidedArena) -> list[dict[str, int | str]]:
    source_root = ROOT / "art_source/arenas" / arena.key / "production_source"
    master = clean_alpha(scale_rgba(Image.open(source_root / arena.source_file), CANVAS), arena.key)
    guide = scale_rgba(Image.open(source_root / arena.guide_file), CANVAS)
    visible = np.asarray(master.getchannel("A")) >= ALPHA_THRESHOLD
    sections = enclosed_sections(red_guide_mask(guide), visible, arena.expected_sections)
    paths = output_paths(arena.key)
    paths["reference"].mkdir(parents=True, exist_ok=True)
    pieces: list[dict[str, int | str]] = []
    base_reconstructed = Image.new("RGBA", CANVAS)
    for index, section in enumerate(sections, 1):
        crop, (left, top, right, bottom) = crop_section(master, section)
        base_reconstructed.alpha_composite(crop, (left, top))
        piece_id = f"section_{index:02d}"
        reference = scale_rgba(crop, (crop.width * SCALE, crop.height * SCALE))
        reference.save(paths["reference"] / f"{arena.key}_{piece_id}_reference.png")
        pieces.append({
            "id": piece_id,
            "x": left,
            "y": top,
            "width": right - left,
            "height": bottom - top,
            "sorting_order": -10 + index,
            "reference": f"{arena.key}_{piece_id}_reference.png",
        })
    if ImageChops.difference(master, base_reconstructed).getbbox() is not None:
        raise ValueError(f"{arena.key}: contour assignments do not reconstruct the clean master.")
    write_prepare_outputs(arena.key, master, pieces)
    return pieces


def prepare_kraken() -> list[dict[str, int | str]]:
    source = ROOT / "art_source/arenas/kraken/production_source"
    paths = output_paths("kraken")
    paths["reference"].mkdir(parents=True, exist_ok=True)
    pieces: list[dict[str, int | str]] = []
    source_checksums: dict[str, str] = {}
    master = clean_alpha(Image.open(source / "layout_master.png").convert("RGBA"), "kraken")
    for piece in KRAKEN_PIECES:
        visual = Image.open(source / f"kraken_{piece.name}_visual.png").convert("RGBA")
        mask = Image.open(source / f"kraken_{piece.name}_mask.png").convert("RGBA")
        if visual.size != mask.size:
            raise ValueError(f"Kraken {piece.name} visual and mask dimensions differ.")
        reference = scale_rgba(visual, (visual.width * SCALE, visual.height * SCALE))
        reference.save(paths["reference"] / f"kraken_{piece.name}_reference.png")
        source_checksums[piece.name] = sha256(source / f"kraken_{piece.name}_mask.png")
        x0, y0, x1, y1 = piece.crop
        pieces.append({"id": piece.name, "x": x0, "y": y0, "width": x1 - x0, "height": y1 - y0, "sorting_order": piece.sorting_order, "reference": f"kraken_{piece.name}_reference.png"})
    write_prepare_outputs("kraken", master, pieces, {"source_mask_sha256": source_checksums})
    return pieces


def write_prepare_outputs(key: str, master: Image.Image, pieces: list[dict[str, int | str]], extra: dict[str, object] | None = None) -> None:
    paths = output_paths(key)
    layout = {"canvas": {"width": CANVAS[0], "height": CANVAS[1]}, "production_scale": SCALE, "pieces": pieces}
    if extra:
        layout.update(extra)
    (paths["root"]).mkdir(parents=True, exist_ok=True)
    (paths["root"] / f"{key}_guide_contour_layout.json").write_text(json.dumps(layout, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    reconstruct_reference(key, master, pieces)
    qa = {
        "arena": key,
        "sections": len(pieces),
        "source_alpha_pixels": int((np.asarray(master.getchannel("A")) >= ALPHA_THRESHOLD).sum()),
        "rule": "Guide contour faces partition every visible source pixel. Reference crops are two-times resolution only; they cannot be promoted before an individual enhancement exists.",
    }
    if extra:
        qa.update(extra)
    (paths["root"] / "qa_prepare.json").write_text(json.dumps(qa, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")


def reconstruct_reference(key: str, master: Image.Image, pieces: list[dict[str, int | str]]) -> None:
    paths = output_paths(key)
    reconstructed = Image.new("RGBA", (CANVAS[0] * SCALE, CANVAS[1] * SCALE))
    for item in pieces:
        visual = Image.open(paths["reference"] / str(item["reference"])).convert("RGBA")
        reconstructed.alpha_composite(visual, (int(item["x"]) * SCALE, int(item["y"]) * SCALE))
    reconstructed.save(paths["root"] / "review_reference_reconstructed_transparent.png")
    for name, color in (("dark", (20, 24, 33, 255)), ("light", (244, 246, 248, 255)), ("blue", (22, 90, 160, 255))):
        preview = Image.new("RGBA", reconstructed.size, color)
        preview.alpha_composite(reconstructed)
        preview.convert("RGB").save(paths["root"] / f"review_reference_reconstructed_{name}.png")


def apply_enhancements(key: str, pieces: list[dict[str, int | str]]) -> None:
    paths = output_paths(key)
    paths["visuals"].mkdir(parents=True, exist_ok=True)
    paths["masks"].mkdir(parents=True, exist_ok=True)
    records: list[dict[str, object]] = []
    missing: list[str] = []
    for item in pieces:
        piece_id = str(item["id"])
        reference_path = paths["reference"] / str(item["reference"])
        enhanced_path = paths["enhanced"] / f"{key}_{piece_id}_enhanced.png"
        if not enhanced_path.is_file():
            missing.append(enhanced_path.name)
            continue
        reference = Image.open(reference_path).convert("RGBA")
        final = final_from_enhancement(Image.open(enhanced_path).convert("RGBA"), reference)
        visual_path = paths["visuals"] / f"{key}_{piece_id}_visual.png"
        mask_path = paths["masks"] / f"{key}_{piece_id}_mask.png"
        final.save(visual_path)
        if key == "kraken":
            source_mask = Image.open(ROOT / "art_source/arenas/kraken/production_source" / f"kraken_{piece_id}_mask.png").convert("RGBA")
            mask = source_mask.resize(final.size, Image.Resampling.NEAREST)
        else:
            mask = mask_for(reference)
        mask.save(mask_path)
        records.append({
            "id": piece_id,
            "quality_pass": "individual_image_to_image_enhancement",
            "reference_sha256": sha256(reference_path),
            "enhanced_sha256": sha256(enhanced_path),
            "final_visual_sha256": sha256(visual_path),
            "width": final.width,
            "height": final.height,
            "alpha_source": "reference_crop",
        })
    if missing:
        raise FileNotFoundError("No resize fallback is allowed. Missing individual enhanced renders: " + ", ".join(missing))
    (paths["root"] / "quality_pass_manifest.json").write_text(json.dumps({"rule": "Every fragment is individually enhanced after the guide-contour crop; resize-only output cannot be promoted.", "visual_style": STYLE_SPEC, "pieces": records}, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    verify_final(key, pieces, records)
    write_final_review(key, pieces)


def verify_final(key: str, pieces: list[dict[str, int | str]], records: list[dict[str, object]]) -> None:
    paths = output_paths(key)
    if len(records) != len(pieces):
        raise ValueError(f"{key}: every fragment needs one quality record.")
    for item in pieces:
        piece_id = str(item["id"])
        visual = Image.open(paths["visuals"] / f"{key}_{piece_id}_visual.png").convert("RGBA")
        mask = Image.open(paths["masks"] / f"{key}_{piece_id}_mask.png").convert("RGBA")
        if visual.size != mask.size:
            raise ValueError(f"{key}/{piece_id}: visual and mask dimensions differ.")
        if not set(mask.getchannel("R").get_flattened_data()).issubset({0, 255}):
            raise ValueError(f"{key}/{piece_id}: collision mask is not binary.")
        # The authoritative reference crop is built from the clean master, so
        # it has no guide pixels. Generated RGB may legitimately contain warm
        # painted materials; a red-colour heuristic here would falsely reject
        # those details even though reference alpha owns every edge and mask.


def write_final_review(key: str, pieces: list[dict[str, int | str]]) -> None:
    """Produce the actual quality-pass composite, not merely the references."""
    paths = output_paths(key)
    reconstructed = Image.new("RGBA", (CANVAS[0] * SCALE, CANVAS[1] * SCALE))
    for item in pieces:
        piece_id = str(item["id"])
        visual = Image.open(paths["visuals"] / f"{key}_{piece_id}_visual.png").convert("RGBA")
        reconstructed.alpha_composite(visual, (int(item["x"]) * SCALE, int(item["y"]) * SCALE))
    reconstructed.save(paths["root"] / "review_quality_reconstructed_transparent.png")
    for name, color in (("dark", (20, 24, 33, 255)), ("light", (244, 246, 248, 255)), ("blue", (22, 90, 160, 255))):
        preview = Image.new("RGBA", reconstructed.size, color)
        preview.alpha_composite(reconstructed)
        preview.convert("RGB").save(paths["root"] / f"review_quality_reconstructed_{name}.png")


def promote(key: str, pieces: list[dict[str, int | str]]) -> None:
    paths = output_paths(key)
    runtime = ROOT / "Assets/_Game/Content/Resources/Visuals/Arenas" / (GUIDED_ARENAS[key].runtime_folder if key in GUIDED_ARENAS else "Kraken")
    runtime.mkdir(parents=True, exist_ok=True)
    for item in pieces:
        piece_id = str(item["id"])
        shutil.copy2(paths["visuals"] / f"{key}_{piece_id}_visual.png", runtime / f"{key}_{piece_id}_visual.png")
        shutil.copy2(paths["masks"] / f"{key}_{piece_id}_mask.png", runtime / f"{key}_{piece_id}_mask.png")


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--arenas", nargs="+", choices=[*GUIDED_ARENAS, "kraken"], default=[*GUIDED_ARENAS, "kraken"])
    parser.add_argument("--prepare", action="store_true", help="Write deterministic, two-times-resolution reference crops and QA previews.")
    parser.add_argument("--apply-enhancements", action="store_true", help="Require individual enhanced render inputs and create final visual/mask pairs.")
    parser.add_argument("--promote", action="store_true", help="Copy verified final pairs into Unity Resources.")
    args = parser.parse_args()
    if not args.prepare and not args.apply_enhancements:
        parser.error("Choose --prepare or --apply-enhancements.")
    if args.promote and not args.apply_enhancements:
        parser.error("--promote requires --apply-enhancements; reference crops are never runtime assets.")
    result: dict[str, int] = {}
    for key in args.arenas:
        if args.prepare:
            pieces = prepare_kraken() if key == "kraken" else prepare_guided(GUIDED_ARENAS[key])
        else:
            layout = json.loads((output_paths(key)["root"] / f"{key}_guide_contour_layout.json").read_text(encoding="utf-8"))
            pieces = list(layout["pieces"])
        if args.apply_enhancements:
            apply_enhancements(key, pieces)
            if args.promote:
                promote(key, pieces)
        result[key] = len(pieces)
    print(json.dumps({"arenas": result, "promoted": args.promote}, ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()
