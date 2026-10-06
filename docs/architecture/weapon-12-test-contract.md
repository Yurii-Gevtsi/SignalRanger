# Weapon 12: titanium barrier acceptance contract

Date: 2026-10-06. Scope: weapon `weapon.titanium-barrier` only, using card 12 in the combat specification and the owner's final +10% / +50% clarification.

## Accepted behavior and boundaries

- The preview uses the same existing `titanium_barrier` sprite as the fire icon, with white tint and full opacity. Its base size is authored against the 1920x1080 HUD reference and the arena's base camera framing; zoom changes never alter world geometry.
- Preserve direction-button translation and pointer rotation. Two fingers resize the beam, with an explicit minimum of 25% base size to keep a usable cover. Without the bought rank-five `size-unlock`, growth stops at the base size; shrinking and restoring are allowed. The purchase gives +10% maximum size, four later Radius purchases give +20%, +30%, +40%, +50%. Unpurchased ranks do not grant the feature.
- Fire commits precisely the preview's center, rotation, and selected size. There is no flying projectile. One item is consumed, and the existing shared retreat/action rules apply.
- The visible sprite alpha is the collision shape, including its native diagonal orientation. The deployed surface remains stationary, blocks walking/projectiles, participates in normal destruction, and survives moving-arena collision refreshes without resurrecting carved pixels. It is terrain, outside transporter pickup candidates.
- A single rank-five feature is available through the existing paid progression system. Radius purchases are gated until that feature is bought; loadout purchases can reach rank five. Loadout is the existing two base items plus four purchases. No damage/HP balance work is required for this non-damaging weapon.
- Non-goals: changing other weapons, new raster art, or replacing the arena sprite pipeline. Existing user edits remain intact.

## Test mapping and seams

Use public `BattleSimulation.TryFire(AimState)` with center in Origin and relative cover size in Power for this placement weapon. This keeps existing replay/duel fire commands complete without adding local-only placement state. Use the existing WeaponUpgradeService for purchase acceptance and the TerrainGrid for collision observations. Test real assets and the real Battle scene, not simulated rendering.

| Acceptance | Observable assertion |
| --- | --- |
| Preview and placement agree | Event geometry and visible bounds equal requested center/rotation/size; no projectile spawned |
| Resize ownership | Synthetic Input System touch pair changes preview scale while camera size remains constant |
| Feature purchase and limits | Before purchase max=1; initial max=1.1; four Radius steps max=1.5; service rejects early Radius purchase |
| Geometry | Solid pixels collide, transparent corners stay empty at several rotations; projectile ray hits the beam |
| Lifecycle | Deployed collision persists after grid refresh; a crater clears both runtime sprite pixels and collision and remains cleared |
| Turn and stock | Ammo decreases once; action committed; retreat permits movement and blocks second attack; next team starts afterward |
| Rendering | Opaque sprite identity matches HUD; fixed physical bounds across camera zoom and battle frames |

Initial evidence: old implementation tints the preview alpha=0.76, pinch owns camera zoom, cover collision stamps an unrotated Soil rectangle, and the progression catalog has no rank-five feature. The old CoverWeapon_Fired_StampsTerrainMask expectation predates the shared retreat rule; revise it to assert the current retreat lifecycle, retaining its terrain and ammo checks.

Existing candidates: reuse the inventory, paid progression, shared retreat, and destructible terrain grid. Extend the terrain boundary with stationary sprite-mask surfaces rather than deriving collision from sprite bounds. Keep mutable mask ownership in BattleTerrain and expose the matching runtime sprite to the scene adapter.
