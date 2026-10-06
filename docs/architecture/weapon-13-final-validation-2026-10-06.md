# Weapon 13 — heavy mortar final validation

## Scope and result

Mechanics: **implemented**. Global HP balance (the shared `H_cap` curve) remains an open owner decision; the prototype primary damage 48 is not a final value. Specification: `docs/design/weapon-combat-specification.md` card 13; catalog row `mortar` (`weapon.mortira`, TTH limits 4/4/2/4).

Behaviour confirmed by tests:

- Base: the primary charge flies on an arc at the shared start speed 12 and detonates; exactly two side shards follow (half effective damage, full effective radius, smaller `mortira_shot`).
- One feature only (`cascade-mortar` = canonical `crater_breach`, rank 5). The first purchase keeps two shards but they become physical, roll into the crater and detonate one after another; four further purchases give 3/4/5/6 shards. Each shard re-checks support after every crater (it descends into newly destroyed ground; impact interval above 0.7 s).
- Sixth purchase and the second feature are rejected; the five paid steps survive a real `JsonUtility` save round-trip and do not invent TTH purchases.
- Radius TTH: +10% per purchase. Shot skin `mortira_shot` for primary and every shard; feature icon `mortira_5_10_B`.
- Hostile sequential shards are pulled/intercepted by a magnet (grounder); the sequential queue clears and no orphaned shard remains.

### Correction of an earlier claim

The checkpoint note said the Loadout TTH raised stock 2→6. The canonical catalog allows **two** Loadout purchases for this weapon (`4/4/2/4`; "two paid kit items, not a confirmed starting stock"). The failing paid test was wrong for buying Loadout four times. It now buys 2×Loadout + 2×Aim to reach rank 5, asserts a third Loadout purchase is refused, and then buys the five feature steps. Stock therefore ranges 2→4.

## Evidence

- Focused progression: `Captures/weapon12/w13-edit-progression.xml`, 10/10 PASS.
- Wide EditMode (BattleSimulationTests + WeaponProgressionCatalogTests): `Captures/weapon12/w13-edit-9.xml`, 175/175 PASS (includes the new magnet-interception test).
- Full EditMode suite: `Captures/weapon12/w13-edit-final.xml` (result recorded in the session report).
- PlayMode production scene `StarterWeaponsBattleSmokeTests`: `Captures/weapon12/w13-play.xml`, 22/22 PASS, including `Mortar_ProductionSceneCreatesAllChargesWithTheirShotSkin` for 0…5 purchases (shard count, shot skin on every projectile, all shards resolve, turn advances).
- `node tools/localization-audit.mjs --strict` and `node tools/localization-integrity-gate.mjs`: PASS (13 dictionaries). No player text changed in this closure.
- Cleanup: unused mortar constants and `GetMortarCascadeStage` removed; no references remained.

Automated editor evidence only; not a physical-device run. Note for the workflow: after editing C# files the Unity MCP can run a stale assembly until `refresh_unity` (force, compile request) is called.

## Open items

- Global HP curve and final damage numbers (shared dependency for all damaging weapons).
- Droid contact for waiting shards is intentionally grenade-like (no contact detonation); a timeout/fall-out path exists (`RemainingLifetime`, fall-death Y) but has no dedicated test.
