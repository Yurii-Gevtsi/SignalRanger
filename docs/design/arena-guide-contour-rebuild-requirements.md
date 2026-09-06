# Guide-contour arena rebuild — requirements decision

## Accepted behaviour

- City, Forest, Grib, Laguna and Monument are rebuilt from their clean master
  image and the matching red-guide image in `art_source/arenas/*/production_source`.
- Each closed red outline is one separate destructible fragment.  It is not a
  rectangular crop: its alpha and collision mask follow the source artwork up
  to the guide boundary.
- The fragments reconstruct the clean master without unassigned pixels, gaps,
  or guide-colour pixels.  Every final visual is a transparent RGBA PNG and
  every paired mask has the exact same dimensions.
- Every resulting fragment is rendered at two times the master-pixel density
  and receives an individual image-to-image quality pass.  Final alpha and
  silhouette still come from the reference fragment, so the quality pass cannot
  introduce a background or move an edge.
- Kraken keeps its existing segmentation and collision masks.  Only its three
  visual fragments receive the two-times resolution quality pass.

## First independently verifiable slice

City is the first slice: derive its five guide-enclosed fragments, reconstruct
the master from those fragments, and run alpha, mask-dimension and three
background composite checks before applying the same algorithm to the other
arenas.

## Explicit non-goals

- Desert Sci-Fi is excluded.
- Existing `production_v2` output is retained for audit and is not promoted.
- The red guide images are never shipped or used as final visual source.
- No gameplay geometry, camera scale, scene layout or Kraken collision mask is
  changed by this art-pipeline pass.

## Assumptions and input boundary

- A clean, unmarked master is required to derive a reliable visual and mask.
  Robots and Vines currently expose only red-marked inputs.  They remain
  unmodified until their clean masters are supplied; attempting to erase guide
  strokes would make both the art and collision masks unverifiable.
- Red guide strokes may be freehand and incomplete at a local point.  The
  pipeline closes them with a fixed 10-pixel dilation, then assigns every
  source-art pixel to its nearest enclosed guide region.  This eliminates a
  line-width gap while preserving the user-selected region boundaries.
