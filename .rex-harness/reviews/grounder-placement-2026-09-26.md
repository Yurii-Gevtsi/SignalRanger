# Grounder placement review

Fixed point: `HEAD` (`62f03955f5bde09802941c5299044779b5829430`). Scope: grounder-specific working-tree hunks in `BattleSimulation.cs`, `BattleSimulationEvents.cs`, and `BattleSimulationTests.cs`; nonempty `git diff HEAD --` those files, hash `64c5cf98c62dcbcaad9c6f5bc6ab095fc76e3ff2`. The same shared files contain unrelated uncommitted weapon work; that work is outside this verdict. Specification: `docs/design/weapon-combat-specification.md:159-163,227-274`. Standards: `AGENTS.md`, `CLAUDE.md`, and the review skill's 12 smell baseline.

## Standards

No hard standards violation in the scoped placement slice. Smells checked: Mysterious Name — none; Duplicated Code — none; Feature Envy — none; Data Clumps — none; Primitive Obsession — existing string ID convention only; Repeated Switches — none; Shotgun Surgery — none in this slice; Divergent Change — none; Speculative Generality — **one judgement finding**: `BattleSimulation.cs:1770-1800`, `ActiveGrounder` records health and turns but this slice does not consume them yet. Finish the interception/lifetime behavior in the same weapon implementation before committing; if the scope stops at placement, remove unused state. Message Chains, Middle Man, Refused Bequest — none. Standards findings: 1 judgement.

## Spec

Placement, one-ammo consumption, and no healing are observed by `Grounder_PlacesWithoutAimAndDoesNotHealOwner` in Unity EditMode. The full spec says the magnet attracts hostile physical projectiles in `N0`, they explode near it and damage its HP, and it lasts two turns with +10% radius and +1 turn per upgrade (`weapon-combat-specification.md:161-163`). **Finding, hard/incomplete:** `BattleSimulation.cs:1770-1800,4490-4740` has no projectile attraction, contact explosion to magnet, damage or removal, duration ticking, or upgraded radius/core; implement these and test hostile/friendly/energy cases. **Finding, hard/incomplete:** `BattleSceneController.cs` has no `GrounderDeployed` listener or placed visual; render `grounder` and remove it on expiry/destruction. Spec findings: 2 hard incomplete.

Summary: Standards 1 judgement (unused pending state); Spec 2 hard incomplete (combat interception and visual). Verdict: placement slice passes; weapon 28 is not complete.
