# Review: weapon-combat-full-implementation-v2, first vertical slice

Fixed point: `HEAD` working tree. Reviewed files: `Assets/_Game/Runtime/Gameplay/Battle/Simulation/BattleSimulation.cs`, `Assets/_Game/Tests/EditMode/Gameplay/BattleSimulationTests.cs`, `tools/run-unity-mcp-editmode-test.mjs`. The scoped diff is non-empty; unrelated shared worktree changes were not evaluated as this slice's ownership.

## Standards

No hard violation found. The runtime change at `BattleSimulation.cs:1774` reuses the existing central `ApplyDamage` boundary and has no duplicated control path. The regression test enters through public `TryFire`, and the runner change accepts the two observed server result names. The twelve baseline smells were reviewed: mysterious name, duplicated code, feature envy, data clumps, primitive obsession, repeated switches, shotgun surgery, divergent change, speculative generality, message chains, middle man, and refused bequest — none is introduced by this three-line functional slice.

## Spec

One hard finding remains against [weapon-combat-specification.md](../design/weapon-combat-specification.md): the request covers all 41 weapons, while `BattleSimulation.IsBattleMechanicAvailable` still returns `false` for several specified weapons at lines 413–417, including orbital rocket, guardian drone, turn transfer, grounder, force field, self-destruct and nuclear charge. The plasma-sword slice satisfies card 5: one closest forward target now takes fixed configured damage, rather than a percentage of its remaining HP. Continue with the planned runtime catalog and per-group public tests before considering the full request complete.

Standards findings: 0, highest: none. Spec findings: 1, highest: hard incomplete coverage of the 41-card scope.
