# Desert sci-fi arena production decomposition v1

This folder contains the geometry-locked decomposition of the approved v5
master into eight unique destructible sprite pairs and nine scene instances.

## Accepted runtime source

- `visuals/`: eight RGBA visual sprites at one shared `production_scale` of 2.
- `masks/`: eight matching binary collision masks.
- `arena_desert_sci_fi_pieces.csv`: unique asset crops and sorting orders.
- `arena_desert_sci_fi_instances.csv`: scene instances; the small floating
  platform references one asset twice.
- `arena_desert_sci_fi_layout.json`: canvas, PPU, pivot, and scale metadata.
- `review/arena_desert_sci_fi_reconstructed_with_reuse.png`: accepted
  geometry reconstruction.

The shared scale is 2 rather than the default recommendation of 4 because the
approved decomposition uses large clusters. At scale 2 the eight unique visual
textures occupy about 9.821 megapixels and remain inside the preferred mobile
budget. No individual asset uses a compensating transform scale.

## Rejected detail experiment

`source_layers/production_renders/` contains independent high-detail chroma
renders kept only as source experiments. They are not accepted runtime assets:
their internal landmarks drifted when fitted into the locked cluster masks.
`review/arena_desert_sci_fi_rejected_detail_fit.png` records that failed
reconstruction for comparison. Do not import these candidates into Unity.

Any later detail pass must preserve every accepted mask, crop, platform height,
side-facing entrance, join, and instance coordinate.
