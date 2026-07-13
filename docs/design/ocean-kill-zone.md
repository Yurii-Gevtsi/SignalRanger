# Ocean kill-zone

Status: implemented (2026-07-13).

The training meadow is assembled at runtime, rather than stored as a separate Unity scene. Its visual theme now instantiates `KillZone_Ocean` only for `arena.training-meadow`.

- Assets live under `Assets/_Game/Content/Resources/Visuals/Arenas/KillZones/Ocean/` so the runtime battle composition can load the prefab with the same resource convention as the other arena visuals.
- `OceanBase` is rendered at order `-6`, `LightRays` at `-5`, and `SurfaceWaves` at `-4`; terrain is at `-8` and units are at `20`.
- `KillZone_Ocean` contains only the seven-frame `SurfaceWaves` animation from `Lights_and_waves`, with no ocean backdrop, mask, collider, or light-ray layer. It is horizontally fitted to the widest available battle camera width and positioned at world Y `-10.5`. The wave clip is a five-second loop that animates only the `SpriteRenderer.sprite` property.
- Runtime battle simulation does not use Unity 2D colliders for unit movement; it applies a deterministic kill-zone check to every unit's feet at the wave line (`-5.75`), including units that settled inside a crater.

This keeps kill-zone behavior deterministic and prevents a second, competing death system.
