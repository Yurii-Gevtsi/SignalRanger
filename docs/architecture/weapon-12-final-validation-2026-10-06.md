# Weapon 12 — titanium barrier final validation

## Scope and result

Implemented. Original specification: `docs/design/weapon-combat-specification.md`, card 12, plus the approved rank-five feature clarification. Base stock is 2; four Loadout purchases produce 6. Base scale can shrink to 25% and restore to 100%. Buying size-unlock at weapon rank 5 raises the maximum to 110%; four subsequent Radius purchases raise it to 150%.

The existing opaque icon is the preview and installed skin. Translation and rotation remain available; two touches resize the beam without zooming or rotating it. Installation uses exactly the preview center, angle and dimensions, consumes one charge and enters the existing retreat window. The installed beam is stationary destructible terrain whose collision follows opaque sprite pixels. Invalid placement spends neither ammo nor action. Terrain refresh preserves it and preserves its craters. There is no projectile or damage balance dependency.

## Evidence

- RED: `Captures/weapon12/red.xml`, 3 failures before implementation.
- Model/progression/terrain: `Captures/weapon12/green.xml`, 40/40 PASS, receipt `7fe7b372-daf3-4df0-9328-b6d57b31d24a`.
- Final focused model: `Captures/weapon12/green-final.xml`, 15/15 PASS, receipt `6d59d796-6ff7-49d8-a1de-1859e81e6dc5`.
- Production battle scene, actual queued two-touch input and HUD Fire: `Captures/weapon12/play-success.xml`, 3/3 PASS, receipt `276798b5-14ab-4889-9174-08e10ca6e50c`.
- Weapons 1–12 battle scene regression including the three resize configurations: `Captures/weapon12/regression.xml`, 15/15 PASS, receipt `062bc5ad-0364-43d9-9507-41ad4824268e`.
- `node tools/localization-audit.mjs --strict`: PASS; `node tools/localization-integrity-gate.mjs --staged`: PASS, 13 dictionaries.

The interrupted MCP jobs and initial failing fixtures are retained separately. Their errors were not counted as successful validation. Tests ran in Unity 6000.5.1f1 through the open editor. This is automated editor scene evidence, not a physical mobile-device run.

## Standards review

User-approved fixed point: `62f03955f5bde09802941c5299044779b5829430`; implementation commit: `1a0c4b9aee79f81d64a8101666b2137f41197496`. Verified the fixed point resolves, `git log <fixed-point>..HEAD` contains the snapshot commit, and `git diff <fixed-point>...HEAD` is non-empty. Bounded review diff: `Captures/weapon12/review.diff`; the existing shared snapshot contains unrelated work, which is not claimed to have received a full review here.

Reviewed weapon-twelve changes in rules, simulation, terrain ownership, rendering/input, catalog, paid upgrade service, workshop gating, tests and thirteen dictionaries against AGENTS.md/CLAUDE.md. Gameplay and UI responsibilities remain separate. Runtime sprites/textures are released with the arena. No new player-visible literal or external dependency was added. Alpha mask readability is explicitly enabled. UTF-8 and key parity are checked. The baseline smell checks (naming, duplication, feature envy, data clumps, primitive obsession, repeated switches, shotgun surgery, divergent change, speculative generality, message chains and middle man/refused inheritance) reveal no actionable new finding in this bounded change. Findings: 0 hard, 0 judgement.

## Specification review

Checked center and dimensions, opacity and sprite identity, base shrinking/restoration, purchased +10% and four further +10% steps, disabled pre-feature Radius purchases, stock, stationary terrain, rotated mask collision, crater/refresh persistence and invalid-placement semantics. All are implemented and constrained by tests. Minimum scale 25% is the recorded usable placement floor. Findings: 0. Existing movement/rotation controls are preserved; no second feature or damage mechanic was introduced.

VERDICT:

FILES_REVIEWED:

- TitaniumBarrierRules.cs: lines 1–33 (added); PlacedTerrainSpriteSurface.cs: lines 1–126 (added).
- BattleSimulation.cs: lines 2963–3012 (changed); BattleTerrain.cs: lines 109–229 (changed).
- BattleSceneController.cs: lines 371–405, 871–934, 4617–4722 (changed); WeaponProgressionCatalog.cs: lines 198–243 (changed).
- BattleSimulationTests.cs: lines 1090–1208; WeaponProgressionCatalogTests.cs: lines 172–208; StarterWeaponsBattleSmokeTests.cs: lines 174–281 (changed).
- Paid upgrade/workshop/resolver integration, barrier texture metadata, canonical catalog and all localization dictionaries (changed).

CHECKS:

- Unity compile and focused model: PASS (15/15); neighboring model/progression/terrain: PASS (40/40).
- Unity production scene regression: PASS (15/15).
- Strict localization audit and staged integrity: PASS (13/13 dictionaries).
- Focused `git diff --check`: PASS.

CODE:

> return sizeUnlocked ? 1.1f + Mathf.Clamp(radiusPurchases, 0, 4) * 0.1f : 1f;

VALIDATION:

APPROVED — weapon twelve meets the recorded combat specification. The review does not certify unrelated changes in the all-changes snapshot. Physical-device performance remains outside this editor verification.
