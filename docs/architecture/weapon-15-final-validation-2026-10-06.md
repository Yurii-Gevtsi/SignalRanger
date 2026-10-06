# Weapon 15 — teleport sphere validation

## Result

**Implemented** (mechanics). Card 15, `weapon.teleport`, TTH limits 0/4/4/0 (Aim 4, Loadout 4), features `instant-touch` (rank 5) and `early-trigger` (rank 10).

- Base unchanged: arc flight, bounces, teleport when it stops (existing tests).
- `instant-touch`: the first contact with terrain (or a droid other than the owner) removes the sphere and teleports the owner to the last free point. Only physical overlap of the body with the surface is resolved, by the smallest shift towards free space (`ResolveFreeBodyCenter`); no climbing onto an obstacle, no pocket carving. A droid left unsupported is woken (`WakeIfUnsupported`) and falls.
- `early-trigger` (on top of instant-touch): a second Fire press during flight removes the sphere and teleports to its current position; no second charge is spent. Without the feature the second press is blocked. HUD keeps the weapon, aim and Fire button available for that press.
- Skin: the sphere keeps `UI/Weapons/teleport` after both purchases (`teleport_5_10_A` belongs to the swap teleport; the icon resolver no longer maps it onto the sphere).
- Purchases: Core/Radius refused, Aim/Loadout up to 4, each feature exactly one step, gated by rank 5 / 10; JSON round-trip restores both flags; legacy exchange flags are not set.

## Shared fixes found on the way (affect other weapons)

1. `WeaponProgressionDefinition.CountBaseUpgrades`: an unrecorded rank in an explicit save is a paid special step, not an inferred base upgrade (it used to consume Aim/Loadout/... limits; only mortar and barrier were special-cased).
2. `WeaponEvolutionSelectionResolver.TryGetChoice`: the legacy `branch` mirror no longer makes milestone 10 look purchased once any independent choice exists (it blocked buying rank-10 features and switched rank-10 effects, e.g. mine `ally_immune`, on after the rank-5 purchase).

## Evidence

- Battle+Progression EditMode with the new tests: `Captures/weapon12/w15-edit-4.xml` (full EditMode, 713 tests): only the 38 baseline failures remain; no new failure (baseline: `baseline-7e4388f.xml`).
- New EditMode tests: base bounces vs instant touch; ceiling (free side, falls); wall (not onto the obstacle); early trigger (current point, ammo unchanged, turn passes); blocked without feature; paid service + JSON + icon.
- PlayMode: `Captures/weapon12/w15-play-4.xml`, 25/25 PASS, including the real scene throw, sphere body texture, HUD Fire availability and second press.

## Open items

- Loadout base/limits are catalog TBD (sphere stock is 2 in the prototype library).
- Automated editor evidence only; no physical device run.
