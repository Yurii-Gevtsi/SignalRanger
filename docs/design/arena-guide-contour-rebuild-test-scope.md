# Canonical arena rebuild — verification scope

## Observable acceptance boundary

An accepted arena package starts from one artist-provided RGBA original and
one red-marked copy of that same original. The original must contain real
transparent background pixels. An opaque master is a hard preparation error;
the rebuild must stop rather than guess a black, white, magenta, or checkerboard
background. A separate background image is optional and is never part of the
destructible arena composite.

The output is intentionally minimal: one deterministic source crop, one
individually enhanced RGBA visual, and one mask for each marked section; one
transparent reconstructed arena; and one machine-readable alignment report.
Dark/light/blue QA renders, alternate full-arena renders, and raw generation
variants are not accepted deliverables.

## In-scope checks

1. `source-alpha`: the original is RGBA and has alpha-zero pixels; the red
   marked layout has the same canvas as the original. A background image, if
   present, is reported as a non-destructible independent layer.
2. `guide-sections`: the selected closed red regions match the artist's marked
   section count. Every visible original pixel is assigned to one or more
   source sections. Adjacent sections have a fixed 2-pixel production overlap
   where their alpha silhouettes meet, so no empty seam can be exposed.
3. `source-crops`: every `source_segments/*_source.png` is a simple pixel crop
   of the original only. No resize or generation is allowed at this stage.
4. `enhanced-pairs`: every source section has exactly one image-to-image
   enhanced RGBA visual with a true alpha channel and exactly one same-canvas
   binary collision mask derived from that enhanced visual. The mask must not
   silently reuse the original crop alpha.
5. `single-composite`: exactly one `*_reconstructed_transparent.png` is
   emitted. It is an RGBA composition of final enhanced visuals only; no
   coloured-background preview variants are emitted.
6. `alignment`: reconstructing final visuals from the emitted layout produces
   a silhouette that agrees with the original within the documented 2-pixel
   overlap tolerance. The report must identify unexpected gaps, out-of-bounds
   pixels, and overlaps larger than that tolerance. It must fail the build if
   an unintended hole or displacement is found.
7. `runtime-pairing`: each final visual/mask pair has identical dimensions and
   the manifest records source, enhancement, mask, coordinates, overlap, and
   validation result.

## Test seam

`tools/arena-assets/rebuild_guide_contour_arenas.py` is the file-system public
entry point. Its `--prepare` mode validates source alpha and writes only
deterministic source crops plus a layout manifest. Its enhancement/promote
mode consumes one enhanced RGBA image per section, derives masks from those
new visuals, performs the full reconstruction/alignment check, and refuses to
promote an arena without a passing report. Verification reads only the manifest
and emitted PNG files.

## Out of scope

This slice does not alter Unity physics, camera behaviour, runtime placement,
or device rendering. It does not repair an opaque source master. It does not
retain legacy multi-background QA composites merely for visual inspection.

## Completion criterion

City passes every applicable check above using its cleaned source original and
the user's marked layout. Its source package contains no redundant full-arena
generation variants, and its runtime assets are promoted only after a passing
alignment report.
