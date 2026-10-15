# Review: toxic status vertical slice

Fixed point: `HEAD`; this is a shared dirty worktree, therefore the review is limited to the documented slice in `BattleSimulation.cs`, `BattleSimulationTests.cs`, `tools/run-unity-mcp-editmode-test.mjs` and `elemental-status-runtime-test-scope.md`. The broader pre-existing changes in those files are not reviewed here.

## Standards

Reviewed against `AGENTS.md` and the 12 smell categories in the active review procedure.

- **Hard findings:** none. The code keeps damage resolution in the existing simulation authority, uses the real `DamageEvent.Damage`, and introduces no player-facing localized text.
- **Mysterious name / duplicated code / feature envy / data clumps / primitive obsession / repeated switches / shotgun surgery / divergent change / speculative generality / message chains / middle man / refused bequest:** not seen in the bounded slice. `ProcessPoisonForEndingActiveTurn` accurately names the timing; its three calls mirror three existing independent turn-transition paths, so consolidating them is outside this narrowly tested change.
- **Tooling change:** the test adapter reads Unity's official `TestResults.xml` only after the MCP run has begun and only when the file timestamp changes. This is a bounded workaround for the observed permanently-running MCP job and preserves the selected test name and exit status.

## Spec

Source: `docs/design/weapon-combat-specification.md`, the user requirement for two victim-turn elemental effects, and `elemental-status-runtime-test-scope.md`.

- **Hard finding — duration extension remains:** `BattleSimulation.cs:3048` calculates `ToxicBaseDurationTurns + evolution.ToxicDurationBonusTurns`. The accepted rule is two affected-unit turns; a same-element hit refreshes the duration rather than extending it. A legacy evolution can still extend beyond two turns. **Fix:** set the base elemental poison duration to exactly two for the shared status and move any approved evolution effect to its separately specified mechanic.
- **Judgement finding — incomplete family:** the slice only migrates `weapon.toxic`. The spec also requires fire, cold and electricity, statuses on every initially damaged ally/enemy, and same-element refresh. This is intentionally outside the vertical test slice and must be implemented in follow-up slices before declaring the elemental family complete.

Verdict: standards has 0 findings; spec has 1 hard compliance finding and 1 documented incompleteness. The current focused toxic timing test passes, but the shared elemental feature is not complete.
