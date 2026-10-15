# Moving destructible arena surfaces

## Decision

Moving arena props are represented as `ArenaSpriteSurface` entries with an
optional horizontal motion profile. They remain part of the same sprite-backed
`BattleTerrain` as static arena fragments, so their runtime texture and mask
participate in crater destruction and shared collision queries.

## Runtime rules

- The moving transform is advanced by `BattleSceneController` during the battle
  update, not by a decorative background component.
- `BattleTerrain` refreshes the shared `TerrainGrid` after a moving surface
  changes position. The refresh reads the mutable runtime texture and mask, so
  destroyed pixels stay destroyed and the previous position is cleared.
- Motion is currently horizontal, sinusoidal, and bounded around the authored
  start position. A zero range or speed keeps a surface static.
- The desert floating platform uses a two-world-unit range and `0.7` angular
  speed. Its authored layout position remains the motion origin.

## Asset boundary

The reconstructed desert review image is composition reference only. The
floating platform remains a separate visual/mask pair in Unity so it can move
and be destroyed independently from the static fragments.
