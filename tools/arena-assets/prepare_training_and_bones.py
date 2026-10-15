"""Prepare the new training and bones arena raster packages.

The source folder is intentionally read-only from this script's point of view.
Outputs are written to a production folder and to the Unity Resources tree so
the runtime can load the visual and its matching collision mask by resource id.
"""

from __future__ import annotations

import argparse
import json
import uuid
from pathlib import Path

import numpy as np
from PIL import Image, ImageEnhance, ImageFilter, ImageChops
from scipy import ndimage


ROOT = Path(__file__).resolve().parents[2]
TRAINING_SCALE = 2.4
BONES_SCALE = 2
ALPHA_THRESHOLD = 32
EDGE_REPAIR_RADIUS = 3
TRAINING_MASTER_NAME = "arena-transparent.png"
TRAINING_SECTION_GUIDE_NAME = "arena-transparent-sections.png"
TRAINING_PRODUCTION_FOLDER = "production_v2"
TRAINING_GENERATED_FRAGMENT_FILENAMES = {
    1: ("training_section_01_generated.png", "01.png"),
    2: ("training_section_02_generated.png", "training-section-02-crates-hq-transparent.png"),
    3: ("training_section_03_generated.png",),
}


def read_rgba(path: Path) -> Image.Image:
    return Image.open(path).convert("RGBA")


def remove_connected_white_matte(image: Image.Image) -> Image.Image:
    """Remove only white components connected to the crop border."""
    rgba = np.asarray(image.convert("RGBA"), dtype=np.uint8).copy()
    rgb = rgba[:, :, :3]
    white = (rgb[:, :, 0] >= 246) & (rgb[:, :, 1] >= 246) & (rgb[:, :, 2] >= 246)
    # Use a connected-component pass so internal white highlights are preserved.
    labels, count = ndimage.label(white, structure=np.ones((3, 3), dtype=bool))
    border_labels = set(labels[0, :]) | set(labels[-1, :]) | set(labels[:, 0]) | set(labels[:, -1])
    border_labels.discard(0)
    matte = np.isin(labels, list(border_labels))
    rgba[matte, 3] = 0
    rgba[matte, :3] = 0
    return Image.fromarray(rgba, "RGBA")


def repair_edge_colors(source: Image.Image, generated: Image.Image) -> Image.Image:
    """Replace light matte RGB on antialiased edges with nearby object colour."""
    source_alpha = np.asarray(source.getchannel("A"), dtype=np.uint8)
    source_rgb = np.asarray(source.convert("RGBA"), dtype=np.uint8)[:, :, :3]
    rgba = np.asarray(generated.convert("RGBA"), dtype=np.uint8).copy()
    alpha = rgba[:, :, 3]
    source_solid = source_alpha >= ALPHA_THRESHOLD
    solid = source_solid & (alpha >= 220)
    if not np.any(solid):
        return generated.convert("RGBA")

    distances, indices = ndimage.distance_transform_edt(~solid, return_indices=True)
    edge = source_solid & (alpha > 0) & (alpha < 220) & (distances <= EDGE_REPAIR_RADIUS)
    nearest_y = indices[0][edge]
    nearest_x = indices[1][edge]
    for channel in range(3):
        channel_pixels = rgba[:, :, channel]
        channel_pixels[edge] = channel_pixels[nearest_y, nearest_x]
        rgba[:, :, channel] = channel_pixels

    # The supplied fragment candidates contain a white matte on portions of
    # their antialiased contour. Prefer the clean master RGB on that contour
    # so the enlarged render does not reintroduce a bright halo during
    # compositing or texture filtering.
    source_distance = ndimage.distance_transform_edt(source_solid)
    source_boundary = source_solid & (source_distance <= EDGE_REPAIR_RADIUS)
    rgba[source_boundary, :3] = source_rgb[source_boundary]
    rgba[alpha < ALPHA_THRESHOLD, :3] = 0
    return Image.fromarray(rgba, "RGBA")


def allowed_alpha(source: Image.Image, generated: Image.Image) -> Image.Image:
    source_mask = np.asarray(source.getchannel("A"), dtype=np.uint8) >= ALPHA_THRESHOLD
    generated = repair_edge_colors(source, generated)
    generated_rgba = np.asarray(generated, dtype=np.uint8).copy()
    permitted = ndimage.binary_dilation(source_mask, iterations=2)
    generated_rgba[~permitted, 3] = 0
    # Avoid leaving opaque RGB in transparent pixels; it makes later mip edges
    # sample a dark/white matte into the sprite.
    generated_rgba[generated_rgba[:, :, 3] < ALPHA_THRESHOLD, :3] = 0
    return Image.fromarray(generated_rgba, "RGBA")


def enhance_transparent_fragment(source: Image.Image, candidate: Image.Image) -> Image.Image:
    if candidate.size != source.size:
        candidate = candidate.resize(source.size, Image.Resampling.LANCZOS)
    candidate = allowed_alpha(source, candidate)
    rgb = candidate.convert("RGB")
    rgb = ImageEnhance.Color(rgb).enhance(1.035)
    rgb = ImageEnhance.Contrast(rgb).enhance(1.025)
    rgb = rgb.filter(ImageFilter.UnsharpMask(radius=0.8, percent=38, threshold=4))
    alpha = candidate.getchannel("A")
    return Image.merge("RGBA", (*rgb.split(), alpha))


def compose_existing_generated_fragment(source: Image.Image, candidate: Image.Image) -> Image.Image:
    """Prepare a stored generated fragment for assembly without any new image pass."""
    if candidate.size != source.size:
        candidate = candidate.resize(source.size, Image.Resampling.LANCZOS)
    return allowed_alpha(source, candidate)


def scale_premultiplied(image: Image.Image, size: tuple[int, int]) -> Image.Image:
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


def mask_for(image: Image.Image) -> Image.Image:
    alpha = image.getchannel("A").point(lambda value: 255 if value >= ALPHA_THRESHOLD else 0, mode="L")
    return Image.merge("RGB", (alpha, alpha, alpha))


def write_training_alignment_report(master: Image.Image, final: Image.Image, production: Path) -> None:
    """Block promotion if the assembled enhanced alpha drifts beyond the arena contract."""
    expected_alpha = master.getchannel("A").resize(final.size, Image.Resampling.LANCZOS)
    expected = np.asarray(expected_alpha, dtype=np.uint8) >= ALPHA_THRESHOLD
    actual = np.asarray(final.getchannel("A"), dtype=np.uint8) >= ALPHA_THRESHOLD
    missing = expected & ~ndimage.binary_dilation(actual, iterations=2)
    growth = actual & ~ndimage.binary_dilation(expected, iterations=2)
    source_opaque = max(1, int(expected.sum()))
    drift_percent = 100.0 * (int(missing.sum()) + int(growth.sum())) / source_opaque
    report = {
        "source_opaque_pixels": source_opaque,
        "reconstructed_opaque_pixels": int(actual.sum()),
        "missing_after_two_pixel_allowance": int(missing.sum()),
        "growth_after_two_pixel_allowance": int(growth.sum()),
        "outer_contour_drift_percent": round(drift_percent, 4),
        "max_allowed_outer_contour_drift_percent": 3.5,
        "status": "pass" if drift_percent <= 3.5 else "fail",
    }
    (production / "alignment_report.json").write_text(
        json.dumps(report, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    if drift_percent > 3.5:
        raise ValueError(f"training reconstruction alpha drift {drift_percent:.2f}% exceeds 3.5%")


def write_meta(template: Path, target: Path, *, visual: bool) -> None:
    text = template.read_text(encoding="utf-8")
    text = text.replace(text.split("guid:", 1)[1].splitlines()[0].strip(), uuid.uuid4().hex, 1)
    text = text.replace("maxTextureSize: 2048", "maxTextureSize: 4096")
    if visual:
        text = text.replace("spritePixelsToUnits: 100", "spritePixelsToUnits: 256")
        text = text.replace("spriteMeshType: 0", "spriteMeshType: 1")
    target.with_suffix(target.suffix + ".meta").write_text(text, encoding="utf-8")


def save_asset(path: Path, image: Image.Image, *, meta_template: Path, visual: bool) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    image.save(path, format="PNG", optimize=False)
    write_meta(meta_template, path, visual=visual)


def detected_section_boundaries(marker: Image.Image) -> tuple[int, ...]:
    """Read the artist's two red divider lines from the same-canvas guide."""
    pixels = np.asarray(marker.convert("RGBA"), dtype=np.uint8)
    red = (
        (pixels[:, :, 0] >= 200)
        & (pixels[:, :, 1] <= 100)
        & (pixels[:, :, 2] <= 100)
        & (pixels[:, :, 3] >= 128)
    )
    columns = np.flatnonzero(red.sum(axis=0) >= 16)
    if columns.size == 0:
        raise ValueError("training section guide does not contain two readable red divider lines")

    groups = np.split(columns, np.flatnonzero(np.diff(columns) > 1) + 1)
    centers = [
        round(float(group.mean()))
        for group in groups
        if group.size >= 2 and int(red[:, group].sum()) >= 128
    ]
    if len(centers) != 2:
        raise ValueError(f"expected exactly two red divider lines, found {len(centers)}")
    return tuple(centers)


def training_sections(master: Image.Image, marker: Image.Image, production: Path) -> list[dict[str, object]]:
    if master.size != marker.size:
        raise ValueError("training master and red section guide must have identical dimensions")

    dividers = detected_section_boundaries(marker)
    overlap = 16
    boundaries = (0, *dividers, master.width)
    source_segments = production / "source_segments"
    source_segments.mkdir(parents=True, exist_ok=True)
    sections: list[dict[str, object]] = []
    for index in range(3):
        left = max(0, boundaries[index] - (overlap if index > 0 else 0))
        right = min(master.width, boundaries[index + 1] + (overlap if index < 2 else 0))
        direct_crop = master.crop((left, 0, right, master.height))
        crop_path = source_segments / f"training_section_{index + 1:02d}_source.png"
        direct_crop.save(crop_path)
        sections.append({
            "id": f"section_{index + 1:02d}",
            "source_x": left,
            "source_width": right - left,
            "source_height": master.height,
            "source": str(crop_path.relative_to(production)).replace("\\", "/"),
        })
    return sections


def prepare_training_source_segments() -> list[dict[str, object]]:
    source_root = ROOT / "art_source" / "arenas" / "training"
    production = source_root / TRAINING_PRODUCTION_FOLDER
    references = production / "source_layers" / "reference_inputs"
    references.mkdir(parents=True, exist_ok=True)
    master = read_rgba(source_root / TRAINING_MASTER_NAME)
    marker = read_rgba(source_root / TRAINING_SECTION_GUIDE_NAME)
    master.save(references / TRAINING_MASTER_NAME)
    marker.save(references / TRAINING_SECTION_GUIDE_NAME)
    sections = training_sections(master, marker, production)
    (production / "training_source_layout.json").write_text(json.dumps({
        "canvas": {"width": master.width, "height": master.height},
        "source_master": TRAINING_MASTER_NAME,
        "section_guide": TRAINING_SECTION_GUIDE_NAME,
        "fragment_overlap_pixels": 16,
        "sections": sections,
    }, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    return sections


def find_training_generated_fragment(generated: Path, index: int) -> Path:
    for filename in TRAINING_GENERATED_FRAGMENT_FILENAMES[index]:
        candidate = generated / filename
        if candidate.exists():
            return candidate
    expected = ", ".join(TRAINING_GENERATED_FRAGMENT_FILENAMES[index])
    raise FileNotFoundError(
        f"missing image-to-image result for training section {index}: expected one of {expected} in {generated}")


def prepare_training(*, preserve_existing_mask: bool = False) -> dict[str, object]:
    source_root = ROOT / "art_source" / "arenas" / "training"
    production = source_root / TRAINING_PRODUCTION_FOLDER
    enhanced = production / "source_layers" / "enhanced_renders"
    generated = production / "source_layers" / "generated_fragments"
    visuals = production / "visuals"
    masks = production / "masks"
    for folder in (enhanced, generated, visuals, masks):
        folder.mkdir(parents=True, exist_ok=True)

    master = read_rgba(source_root / TRAINING_MASTER_NAME)
    sections = prepare_training_source_segments()
    final_size = (round(master.width * TRAINING_SCALE), round(master.height * TRAINING_SCALE))
    final_pixels = np.zeros((final_size[1], final_size[0], 4), dtype=np.uint8)
    promoted_sections: list[dict[str, object]] = []
    for index, section in enumerate(sections, start=1):
        source_path = production / str(section["source"])
        generated_path = find_training_generated_fragment(generated, index)
        source_fragment = read_rgba(source_path)
        generated_fragment = read_rgba(generated_path)
        enhanced_fragment = (
            compose_existing_generated_fragment(source_fragment, generated_fragment)
            if preserve_existing_mask
            else enhance_transparent_fragment(source_fragment, generated_fragment)
        )
        enhanced_path = enhanced / f"training_section_{index:02d}_enhanced.png"
        enhanced_fragment.save(enhanced_path)

        output_width = round(source_fragment.width * TRAINING_SCALE)
        scaled_fragment = scale_premultiplied(enhanced_fragment, (output_width, final_size[1]))
        scaled_pixels = np.asarray(scaled_fragment, dtype=np.uint8)
        output_left = round(int(section["source_x"]) * TRAINING_SCALE)
        output_right = min(final_size[0], output_left + output_width)
        scaled_pixels = scaled_pixels[:, :output_right - output_left]
        existing_alpha = final_pixels[:, output_left:output_right, 3]
        replace = scaled_pixels[:, :, 3] >= existing_alpha
        final_pixels[:, output_left:output_right][replace] = scaled_pixels[replace]
        promoted_sections.append({
            **section,
            "generated": str(generated_path.relative_to(production)).replace("\\", "/"),
            "enhanced": str(enhanced_path.relative_to(production)).replace("\\", "/"),
            "output_x": output_left,
            "output_width": output_right - output_left,
        })

    final = Image.fromarray(final_pixels, "RGBA")
    final.save(enhanced / "training_master_enhanced.png")
    final.save(visuals / "training_reconstructed_visual.png")
    mask_path = masks / "training_reconstructed_mask.png"
    if preserve_existing_mask:
        if not mask_path.exists():
            raise FileNotFoundError(f"missing preserved training mask: {mask_path}")
        mask = Image.open(mask_path).convert("RGB")
        if mask.size != final.size:
            raise ValueError(f"preserved training mask size {mask.size} does not match visual {final.size}")
    else:
        mask = mask_for(final)
        mask.save(mask_path)
    write_training_alignment_report(master, final, production)

    (production / "training_layout.json").write_text(json.dumps({
        "canvas": {"width": final.width, "height": final.height},
        "production_scale": TRAINING_SCALE,
        "source_master": TRAINING_MASTER_NAME,
        "runtime_import": "single_reconstructed_surface",
        "enhanced_reference": "source_layers/enhanced_renders/training_master_enhanced.png",
        "sections": promoted_sections,
    }, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    (production / "quality_pass_manifest.json").write_text(json.dumps({
        "arena": "training",
        "visual_alpha_source": "per_fragment_enhanced_alpha_constrained_to_transparent_master",
        "mask_source": "preserved_training_reconstructed_mask" if preserve_existing_mask else "training_reconstructed_visual_alpha",
        "enhancement_mode": "existing_generated_fragments_composed_without_regeneration" if preserve_existing_mask else "image_to_image_per_fragment",
        "fragment_overlap_pixels": 16,
        "runtime_import": "single_reconstructed_surface",
        "sections": promoted_sections,
        "source_inputs": [TRAINING_MASTER_NAME, TRAINING_SECTION_GUIDE_NAME],
    }, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    return {"visual": final, "mask": mask, "sections": promoted_sections}


def prepare_bones() -> dict[str, Image.Image]:
    source_root = ROOT / "art_source" / "arenas" / "bones"
    production = source_root / "production_v1"
    enhanced = production / "source_layers" / "enhanced_renders"
    visuals = production / "visuals"
    masks = production / "masks"
    for folder in (enhanced, visuals, masks):
        folder.mkdir(parents=True, exist_ok=True)
    master = read_rgba(source_root / "master.png")
    enhanced_master = enhance_transparent_fragment(master, master)
    enhanced_master.save(enhanced / "bones_master_enhanced.png")
    final = scale_premultiplied(enhanced_master, (master.width * BONES_SCALE, master.height * BONES_SCALE))
    final.save(visuals / "bones_reconstructed_visual.png")
    mask = mask_for(final)
    mask.save(masks / "bones_reconstructed_mask.png")
    (production / "bones_layout.json").write_text(json.dumps({
        "canvas": {"width": final.width, "height": final.height},
        "production_scale": BONES_SCALE,
        "source_master": "master.png",
        "enhanced_reference": "source_layers/enhanced_renders/bones_master_enhanced.png",
        "source_sections": ["section_1.png", "section_2.png", "section_3.png"],
    }, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    return {"visual": final, "mask": mask}


def copy_training_to_resources(training: dict[str, object], *, preserve_runtime_mask: bool = False) -> None:
    resources = ROOT / "Assets" / "_Game" / "Content" / "Resources" / "Visuals" / "Arenas"
    template_visual = resources / "Forest" / "forest_reconstructed_visual.png.meta"
    template_mask = resources / "Forest" / "forest_reconstructed_mask.png.meta"
    training_dir = resources / "Training"
    training_dir.mkdir(parents=True, exist_ok=True)
    # The marked sections are production-only. Unity imports just the assembled
    # reconstructed visual/mask pair so it behaves as one destructible surface.
    for stale in training_dir.glob("training_section_*_*.png*"):
        stale.unlink()
    training_visual = training_dir / "training_reconstructed_visual.png"
    training_mask = training_dir / "training_reconstructed_mask.png"
    training["visual"].save(training_visual)
    write_meta(template_visual, training_visual, visual=True)
    if preserve_runtime_mask:
        if not training_mask.exists():
            raise FileNotFoundError(f"missing preserved runtime training mask: {training_mask}")
        with Image.open(training_mask) as existing_mask:
            if existing_mask.size != training["visual"].size:
                raise ValueError(
                    f"preserved runtime training mask size {existing_mask.size} does not match visual {training['visual'].size}")
    else:
        training["mask"].save(training_mask)
        write_meta(template_mask, training_mask, visual=False)


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("--prepare-training-crops", action="store_true")
    parser.add_argument("--rebuild-bones", action="store_true")
    parser.add_argument("--recompose-training-existing-fragments", action="store_true")
    args = parser.parse_args()
    if args.prepare_training_crops:
        sections = prepare_training_source_segments()
        print(json.dumps({"training_source_segments": sections}, ensure_ascii=False, indent=2))
        return
    if args.rebuild_bones:
        prepare_bones()
        print(json.dumps({"bones": "art_source/arenas/bones/production_v1"}, ensure_ascii=False, indent=2))
        return
    preserve_mask = args.recompose_training_existing_fragments
    training = prepare_training(preserve_existing_mask=preserve_mask)
    copy_training_to_resources(training, preserve_runtime_mask=preserve_mask)
    print(json.dumps({
        "training": f"art_source/arenas/training/{TRAINING_PRODUCTION_FOLDER}",
        "runtime_training": "Assets/_Game/Content/Resources/Visuals/Arenas/Training",
        "runtime_import": "training_reconstructed_visual/mask",
    }, ensure_ascii=False, indent=2))


if __name__ == "__main__":
    main()
