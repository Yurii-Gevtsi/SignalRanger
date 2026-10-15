# Grounder health and turns review

Fixed point: `62f03955f5bde09802941c5299044779b5829430` (`HEAD`). Working-tree diff in `BattleSimulation.cs` and `BattleSimulationTests.cs` is nonempty; this review is scoped to the new expiry method and its one test because the files also contain unrelated uncommitted weapon work. `git diff HEAD...HEAD` cannot show uncommitted edits, so the working-tree diff was used.

## Standards

Checked `AGENTS.md` and the 12 smell categories in `rex-code-review`; no direct violation is present in the expiry slice. `ExpireGroundersForEndingTurn` has one cohesive purpose and uses the existing removal event. The three calls are a judgement level instance of Shotgun Surgery at `BattleSimulation.cs:1010,1278,4255`: future turn paths could omit expiry. Recommended follow-up: centralize common end-turn processing when broad turn-flow work is scheduled. All other smell categories: not seen in this slice. The test adds no player-visible text or localization change.

## Spec

Source: `docs/design/weapon-combat-specification.md:159-163`. The base two-turn expiry now matches the spec under the public simulation test; placement and attraction regressions also pass. The scoped goal remains incomplete: `BattleSimulation.cs:5507` deducts magnet health but has no `GrounderDamaged` event or test proving the 45 HP prototype survives one 24-damage hit, dies on the second, and preserves the normal explosion. Severity: judgement. Recommended fix: expose damage through a typed event, add a focused behavior test, then implement and verify. The four core upgrades (+1 turn each) are a separate outstanding slice.

Verdict: the duration slice passes; weapon 28 and this broader health-and-turns item are incomplete. Standards: one judgement finding. Spec: one missing test/observable damage behavior finding.
