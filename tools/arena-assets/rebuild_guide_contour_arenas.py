"""Build canonical modular arena packages from an artist's red section guide.

The artist supplies exactly one clean RGBA original and one same-size marked
copy. ``--prepare`` validates those inputs and makes plain source crops only.
Place exactly one enhanced RGBA ``*_visual.png`` in ``segments`` for every
crop, then run ``--apply-enhancements``. That command derives the mask from
the new visual, emits one transparent reconstruction, and refuses promotion if
the enhanced sections create a visible gap at a shared seam or move their
overall silhouette beyond the documented bounded drift budget.
"""

from __future__ import annotations

import argparse
import hashlib
import json
import shutil
from dataclasses import dataclass
from pathlib import Path

import numpy as np
from PIL import Image, ImageChops
from scipy import ndimage


ROOT = Path(__file__).resolve().parents[2]
SCALE = 2
GUIDE_DILATION = 10
ALPHA_THRESHOLD = 32
PADDING = 16
SECTION_OVERLAP_RADIUS = 1
ALIGNMENT_TOLERANCE_PX = 2
# Image-to-image enhancement may refine a non-shared outer contour.  Accept a
# bounded 3.5% silhouette drift so long as the separately measured shared seams
# still pass; this does not permit a gap between neighboring fragments.
MAX_EXTERIOR_DRIFT_RATIO = 0.035
MAX_SEAM_GAP_PIXELS = 64
MAX_SEAM_GAP_COMPONENT_PIXELS = 32

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
    source_folder: str
    source_file: str
    guide_file: str
    expected_sections: int


GUIDED_ARENAS = {
    "city": GuidedArena("city", "City", ".", "layout_master.png", "layout_master_секції.png", 5),
    "forest": GuidedArena("forest", "Forest", "production_source", "ліс2.png", "ліс2_ссекції.png", 4),
    "grib": GuidedArena("grib", "Grib", "production_source", "гриби.png", "гриби_секції.png", 5),
    "laguna": GuidedArena("laguna", "Laguna", "production_source", "острови.png", "острови_секції.png", 5),
    "monument": GuidedArena("monument", "Monument", ".", "layout_master.png", "монумент_секції.png", 4),
}


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


def require_true_alpha(image: Image.Image, label: str) -> Image.Image:
    """Reject a baked authoring canvas instead of attempting a destructive cutout."""
    if "A" not in image.getbands():
        raise ValueError(f"{label} has no alpha channel. Supply a true transparent PNG.")
    result = image.convert("RGBA")
    if not np.any(np.asarray(result.getchannel("A")) == 0):
        raise ValueError(f"{label} has no transparent background pixels. Supply a true transparent PNG; opaque black, white, magenta and checkerboard masters are invalid.")
    return result


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


def overlap_sections(sections: list[np.ndarray], visible: np.ndarray) -> list[np.ndarray]:
    """Share a narrow source band with neighbours to prevent transparent seams."""
    structure = np.ones((2 * SECTION_OVERLAP_RADIUS + 1, 2 * SECTION_OVERLAP_RADIUS + 1), dtype=bool)
    return [ndimage.binary_dilation(section, structure=structure) & visible for section in sections]


def mask_for(reference: Image.Image) -> Image.Image:
    alpha = reference.getchannel("A").point(lambda value: 255 if value >= ALPHA_THRESHOLD else 0, mode="L")
    return Image.merge("RGBA", (alpha, alpha, alpha, Image.new("L", alpha.size, 255)))


def constrain_enhanced_alpha_to_layout(enhanced: Image.Image, source: Image.Image) -> Image.Image:
    """Keep the generated alpha within the layout's permitted exterior drift.

    The generator owns the visual finish and initial alpha.  It may only move
    the outer contour by the same anti-aliasing allowance used by final
    validation; unbounded extra branches or ledges cannot become collision.
    """
    pixels = np.asarray(enhanced.convert("RGBA")).copy()
    source_alpha = np.asarray(source.convert("RGBA").getchannel("A")) >= ALPHA_THRESHOLD
    structure = np.ones((2 * ALIGNMENT_TOLERANCE_PX + 1, 2 * ALIGNMENT_TOLERANCE_PX + 1), dtype=bool)
    allowed = ndimage.binary_dilation(source_alpha, structure=structure)
    pixels[~allowed] = (0, 0, 0, 0)
    return Image.fromarray(pixels, "RGBA")


def repair_key_colour_fringe(enhanced: Image.Image, source: Image.Image) -> Image.Image:
    """Remove residual magenta key colour without changing the generated alpha.

    The generation pass uses a magenta chroma background because it can be
    removed deterministically. Some providers premultiply that matte into
    anti-aliased edge RGB (and occasionally leave it in an opaque edge pixel).
    Replace only colours that are key-like in the generated image but not in
    the matching source pixel. Legitimate pink/purple artwork therefore stays
    intact, while the source supplies a halo-free colour at a contaminated
    edge. Alpha remains entirely generated/cleaned alpha.
    """
    pixels = np.asarray(enhanced.convert("RGBA")).copy()
    source_pixels = np.asarray(source.convert("RGBA"), dtype=np.uint8)
    rgb = pixels[:, :, :3].astype(np.int16)
    source_rgb = source_pixels[:, :, :3].astype(np.int16)

    def key_like(values: np.ndarray) -> np.ndarray:
        red, green, blue = values[:, :, 0], values[:, :, 1], values[:, :, 2]
        return (red >= 120) & (blue >= 90) & (green <= 100) & ((np.minimum(red, blue) - green) >= 60)

    generated_key = key_like(rgb)
    source_key = key_like(source_rgb)
    source_alpha = source_pixels[:, :, 3] >= ALPHA_THRESHOLD
    contaminated = generated_key & ~source_key & (pixels[:, :, 3] > 0)
    if not np.any(contaminated):
        return Image.fromarray(pixels, "RGBA")

    _, nearest = ndimage.distance_transform_edt(~source_alpha, return_indices=True)
    nearest_rgb = source_pixels[nearest[0], nearest[1], :3]
    replacement = source_pixels[:, :, :3].copy()
    replacement[~source_alpha] = nearest_rgb[~source_alpha]
    pixels[contaminated, :3] = replacement[contaminated]
    return Image.fromarray(pixels, "RGBA")


def repair_shared_seams(key: str, pieces: list[dict[str, int | str]]) -> int:
    """Restore only missing pixels in the declared source-overlap bands.

    This is deliberately narrower than reapplying an entire source alpha: the
    enhanced visual remains the mask source everywhere except a missing shared
    seam, where a one-pixel overlap is the composition contract.
    """
    paths = output_paths(key)
    canvas_width = max(int(item["x"]) * SCALE + int(item["target_width"]) for item in pieces)
    canvas_height = max(int(item["y"]) * SCALE + int(item["target_height"]) for item in pieces)
    coverage = np.zeros((canvas_height, canvas_width), dtype=np.uint8)
    source_coverage = np.zeros_like(coverage)
    owner = np.full(coverage.shape, -1, dtype=np.int16)
    visuals: list[np.ndarray] = []
    sources: list[np.ndarray] = []

    for index, item in enumerate(pieces):
        visual_path = paths["segments"] / f"{key}_{item['id']}_visual.png"
        visual = np.asarray(Image.open(visual_path).convert("RGBA")).copy()
        source = np.asarray(scale_rgba(Image.open(paths["source"] / str(item["source"])), visual.shape[1::-1]), dtype=np.uint8)
        x, y = int(item["x"]) * SCALE, int(item["y"]) * SCALE
        alpha = visual[:, :, 3] >= ALPHA_THRESHOLD
        source_alpha = source[:, :, 3] >= ALPHA_THRESHOLD
        coverage[y:y + visual.shape[0], x:x + visual.shape[1]] += alpha
        source_coverage[y:y + source.shape[0], x:x + source.shape[1]] += source_alpha
        local_owner = owner[y:y + source.shape[0], x:x + source.shape[1]]
        local_owner[(local_owner < 0) & source_alpha] = index
        visuals.append(visual)
        sources.append(source)

    structure = np.ones((2 * ALIGNMENT_TOLERANCE_PX + 1, 2 * ALIGNMENT_TOLERANCE_PX + 1), dtype=bool)
    missing = (source_coverage > 1) & ~ndimage.binary_dilation(coverage > 0, structure=structure)
    repaired = 0
    for y, x in zip(*np.where(missing)):
        index = int(owner[y, x])
        if index < 0:
            continue
        item = pieces[index]
        local_x, local_y = x - int(item["x"]) * SCALE, y - int(item["y"]) * SCALE
        visuals[index][local_y, local_x] = sources[index][local_y, local_x]
        repaired += 1

    if repaired:
        for index, item in enumerate(pieces):
            visual = Image.fromarray(visuals[index], "RGBA")
            visual.save(paths["segments"] / f"{key}_{item['id']}_visual.png")
            mask_for(visual).save(paths["masks"] / f"{key}_{item['id']}_mask.png")
    return repaired


def output_paths(key: str) -> dict[str, Path]:
    root = ROOT / "art_source/arenas" / key / "production"
    return {
        "root": root,
        "source": root / "source_segments",
        "segments": root / "segments",
        "masks": root / "masks",
    }


def prepare_guided(arena: GuidedArena) -> list[dict[str, int | str]]:
    source_root = ROOT / "art_source/arenas" / arena.key / arena.source_folder
    master = require_true_alpha(Image.open(source_root / arena.source_file), f"{arena.key} original")
    guide = require_true_alpha(Image.open(source_root / arena.guide_file), f"{arena.key} marked layout")
    if guide.size != master.size:
        raise ValueError(f"{arena.key}: original and marked layout must have identical canvas dimensions.")
    visible = np.asarray(master.getchannel("A")) >= ALPHA_THRESHOLD
    sections = overlap_sections(enclosed_sections(red_guide_mask(guide), visible, arena.expected_sections), visible)
    paths = output_paths(arena.key)
    paths["source"].mkdir(parents=True, exist_ok=True)
    pieces: list[dict[str, int | str]] = []
    source_coverage = np.zeros(visible.shape, dtype=np.uint8)
    for index, section in enumerate(sections, 1):
        crop, (left, top, right, bottom) = crop_section(master, section)
        source_coverage[section] += 1
        piece_id = f"section_{index:02d}"
        source_file = f"{arena.key}_{piece_id}_source.png"
        crop.save(paths["source"] / source_file)
        pieces.append({
            "id": piece_id,
            "x": left,
            "y": top,
            "width": right - left,
            "height": bottom - top,
            "target_width": (right - left) * SCALE,
            "target_height": (bottom - top) * SCALE,
            "sorting_order": -10 + index,
            "source": source_file,
            "overlap_radius_px": SECTION_OVERLAP_RADIUS,
        })
    if np.any(visible & (source_coverage == 0)) or np.any((~visible) & (source_coverage > 0)):
        raise ValueError(f"{arena.key}: contour assignments do not cover the clean original exactly.")
    write_prepare_outputs(arena.key, master, pieces)
    return pieces


def write_prepare_outputs(key: str, master: Image.Image, pieces: list[dict[str, int | str]]) -> None:
    paths = output_paths(key)
    paths["root"].mkdir(parents=True, exist_ok=True)
    layout = {"canvas": {"width": master.width, "height": master.height}, "production_scale": SCALE, "source_overlap_radius_px": SECTION_OVERLAP_RADIUS, "pieces": pieces}
    (paths["root"] / f"{key}_layout.json").write_text(json.dumps(layout, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    qa = {
        "arena": key,
        "sections": len(pieces),
        "source_alpha_pixels": int((np.asarray(master.getchannel("A")) >= ALPHA_THRESHOLD).sum()),
        "rule": "Source segments are direct original crops. One enhanced RGBA visual and a new alpha-derived mask are required per section before promotion.",
    }
    (paths["root"] / "qa_prepare.json").write_text(json.dumps(qa, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")


def apply_enhancements(key: str, pieces: list[dict[str, int | str]]) -> None:
    paths = output_paths(key)
    paths["segments"].mkdir(parents=True, exist_ok=True)
    paths["masks"].mkdir(parents=True, exist_ok=True)
    records: list[dict[str, object]] = []
    missing: list[str] = []
    for item in pieces:
        piece_id = str(item["id"])
        source_path = paths["source"] / str(item["source"])
        visual_path = paths["segments"] / f"{key}_{piece_id}_visual.png"
        if not visual_path.is_file():
            missing.append(visual_path.name)
            continue
        enhanced = require_true_alpha(Image.open(visual_path), f"{key}/{piece_id} enhanced visual")
        final = scale_rgba(enhanced, (int(item["target_width"]), int(item["target_height"])))
        source = scale_rgba(Image.open(source_path), final.size)
        final = constrain_enhanced_alpha_to_layout(final, source)
        final = repair_key_colour_fringe(final, source)
        final.save(visual_path)
        mask_path = paths["masks"] / f"{key}_{piece_id}_mask.png"
        mask = mask_for(final)
        mask.save(mask_path)
        records.append({
            "id": piece_id,
            "quality_pass": "individual_image_to_image_enhancement",
            "source_sha256": sha256(source_path),
            "enhanced_visual_sha256": sha256(visual_path),
            "mask_sha256": sha256(mask_path),
            "width": final.width,
            "height": final.height,
            "alpha_source": "enhanced_visual",
        })
    if missing:
        raise FileNotFoundError("No resize-only fallback is allowed. Missing one enhanced RGBA visual per source segment: " + ", ".join(missing))
    repaired_seam_pixels = repair_shared_seams(key, pieces)
    report = reconstruct_and_validate(key, pieces)
    report["repaired_shared_seam_pixels"] = repaired_seam_pixels
    (paths["root"] / "alignment_report.json").write_text(json.dumps(report, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    if not report["passed"]:
        raise ValueError(f"{key}: final enhanced fragments do not align with original; see alignment_report.json")
    manifest = {
        "rule": "Every source segment receives one enhanced RGBA visual; its paired mask is generated from that enhanced visual alpha.",
        "visual_style": STYLE_SPEC,
        "pieces": records,
        "alignment": report,
    }
    (paths["root"] / "quality_pass_manifest.json").write_text(json.dumps(manifest, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")


def largest_component_pixels(mask: np.ndarray) -> int:
    """Return the area of the largest connected true-pixel component."""
    labels, count = ndimage.label(mask)
    if count == 0:
        return 0
    areas = np.bincount(labels.ravel())
    return int(areas[1:].max())


def reconstruct_and_validate(key: str, pieces: list[dict[str, int | str]]) -> dict[str, int | float | bool]:
    """Write the sole final transparent composite and validate placement.

    The source-crop overlap is a hard seam contract: any meaningful hole in it
    is rejected.  Outside that shared band, image-to-image finishing may alter
    a tiny anti-aliased outer contour; a bounded area budget records that
    variation without permitting an arbitrary change of silhouette.
    """
    paths = output_paths(key)
    arena = GUIDED_ARENAS[key]
    master = require_true_alpha(Image.open(ROOT / "art_source/arenas" / key / arena.source_folder / arena.source_file), f"{key} original")
    final_size = (master.width * SCALE, master.height * SCALE)
    reconstructed = Image.new("RGBA", final_size)
    coverage = np.zeros((final_size[1], final_size[0]), dtype=np.uint8)
    source_coverage = np.zeros((final_size[1], final_size[0]), dtype=np.uint8)
    collision = np.zeros((final_size[1], final_size[0]), dtype=np.uint8)
    for item in pieces:
        piece_id = str(item["id"])
        visual = Image.open(paths["segments"] / f"{key}_{piece_id}_visual.png").convert("RGBA")
        mask = Image.open(paths["masks"] / f"{key}_{piece_id}_mask.png").convert("RGBA")
        if visual.size != mask.size:
            raise ValueError(f"{key}/{piece_id}: visual and mask dimensions differ.")
        if not set(mask.getchannel("R").get_flattened_data()).issubset({0, 255}):
            raise ValueError(f"{key}/{piece_id}: collision mask is not binary.")
        reconstructed.alpha_composite(visual, (int(item["x"]) * SCALE, int(item["y"]) * SCALE))
        alpha = np.asarray(visual.getchannel("A")) >= ALPHA_THRESHOLD
        y, x = int(item["y"]) * SCALE, int(item["x"]) * SCALE
        coverage[y:y + visual.height, x:x + visual.width] += alpha
        collision[y:y + mask.height, x:x + mask.width] = np.maximum(
            collision[y:y + mask.height, x:x + mask.width],
            np.asarray(mask.getchannel("R"), dtype=np.uint8))
        source = Image.open(paths["source"] / str(item["source"])).convert("RGBA")
        source = scale_rgba(source, (int(item["target_width"]), int(item["target_height"])))
        source_alpha = np.asarray(source.getchannel("A")) >= ALPHA_THRESHOLD
        source_coverage[y:y + source.height, x:x + source.width] += source_alpha
    reconstructed.save(paths["root"] / f"{key}_reconstructed_transparent.png")
    opaque_mask_alpha = np.full(collision.shape, 255, dtype=np.uint8)
    Image.fromarray(np.dstack((collision, collision, collision, opaque_mask_alpha)), "RGBA").save(
        paths["root"] / f"{key}_reconstructed_mask.png")
    expected = np.asarray(scale_rgba(master, final_size).getchannel("A")) >= ALPHA_THRESHOLD
    actual = np.asarray(reconstructed.getchannel("A")) >= ALPHA_THRESHOLD
    structure = np.ones((2 * ALIGNMENT_TOLERANCE_PX + 1, 2 * ALIGNMENT_TOLERANCE_PX + 1), dtype=bool)
    missing = expected & ~ndimage.binary_dilation(actual, structure=structure)
    out_of_bounds = actual & ~ndimage.binary_dilation(expected, structure=structure)
    # Only pixels that were intentionally shared by two direct source crops are
    # a seam.  Testing this band separately prevents benign outer-edge style
    # refinement from concealing a true gap between sections.
    shared_source_band = source_coverage > 1
    seam_gaps = shared_source_band & ~ndimage.binary_dilation(actual, structure=structure)
    original_alpha_pixels = int(expected.sum())
    exterior_budget_pixels = int(np.floor(original_alpha_pixels * MAX_EXTERIOR_DRIFT_RATIO))
    seam_gap_pixels = int(seam_gaps.sum())
    max_seam_gap_component_pixels = largest_component_pixels(seam_gaps)
    missing_pixels = int(missing.sum())
    out_of_bounds_pixels = int(out_of_bounds.sum())
    report = {
        "passed": (
            seam_gap_pixels <= MAX_SEAM_GAP_PIXELS
            and max_seam_gap_component_pixels <= MAX_SEAM_GAP_COMPONENT_PIXELS
            and missing_pixels <= exterior_budget_pixels
            and out_of_bounds_pixels <= exterior_budget_pixels
        ),
        "tolerance_px": ALIGNMENT_TOLERANCE_PX,
        "original_alpha_pixels": original_alpha_pixels,
        "exterior_drift_budget_ratio": MAX_EXTERIOR_DRIFT_RATIO,
        "exterior_drift_budget_pixels": exterior_budget_pixels,
        "missing_pixels": missing_pixels,
        "out_of_bounds_pixels": out_of_bounds_pixels,
        "seam_gap_pixels": seam_gap_pixels,
        "max_seam_gap_component_pixels": max_seam_gap_component_pixels,
        "seam_gap_budget_pixels": MAX_SEAM_GAP_PIXELS,
        "seam_gap_component_budget_pixels": MAX_SEAM_GAP_COMPONENT_PIXELS,
        "overlap_pixels": int((coverage > 1).sum()),
        "source_overlap_pixels": int(shared_source_band.sum()),
        "max_piece_overlap": int(coverage.max()),
    }
    (paths["root"] / "alignment_report.json").write_text(json.dumps(report, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    return report


def promote(key: str, pieces: list[dict[str, int | str]]) -> None:
    paths = output_paths(key)
    runtime = ROOT / "Assets/_Game/Content/Resources/Visuals/Arenas" / (GUIDED_ARENAS[key].runtime_folder if key in GUIDED_ARENAS else "Kraken")
    runtime.mkdir(parents=True, exist_ok=True)
    for item in pieces:
        piece_id = str(item["id"])
        shutil.copy2(paths["segments"] / f"{key}_{piece_id}_visual.png", runtime / f"{key}_{piece_id}_visual.png")
        shutil.copy2(paths["masks"] / f"{key}_{piece_id}_mask.png", runtime / f"{key}_{piece_id}_mask.png")
    shutil.copy2(paths["root"] / f"{key}_reconstructed_transparent.png", runtime / f"{key}_reconstructed_visual.png")
    shutil.copy2(paths["root"] / f"{key}_reconstructed_mask.png", runtime / f"{key}_reconstructed_mask.png")


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--arenas", nargs="+", choices=list(GUIDED_ARENAS), default=list(GUIDED_ARENAS))
    parser.add_argument("--prepare", action="store_true", help="Validate true-alpha source and write only deterministic source segments plus layout data.")
    parser.add_argument("--apply-enhancements", action="store_true", help="Use one enhanced RGBA visual per section, derive masks, and validate the sole transparent reconstruction.")
    parser.add_argument("--promote", action="store_true", help="Copy verified final pairs into Unity Resources.")
    args = parser.parse_args()
    if not args.prepare and not args.apply_enhancements:
        parser.error("Choose --prepare or --apply-enhancements.")
    if args.promote and not args.apply_enhancements:
        parser.error("--promote requires --apply-enhancements; reference crops are never runtime assets.")
    result: dict[str, int] = {}
    for key in args.arenas:
        if args.prepare:
            pieces = prepare_guided(GUIDED_ARENAS[key])
        else:
            layout = json.loads((output_paths(key)["root"] / f"{key}_layout.json").read_text(encoding="utf-8"))
            pieces = list(layout["pieces"])
        if args.apply_enhancements:
            apply_enhancements(key, pieces)
            if args.promote:
                promote(key, pieces)
        result[key] = len(pieces)
    print(json.dumps({"arenas": result, "promoted": args.promote}, ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()
