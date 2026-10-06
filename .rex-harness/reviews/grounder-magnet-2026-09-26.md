# Grounder magnet review

Fixed point: `HEAD` `62f03955f5bde09802941c5299044779b5829430`; current work is uncommitted in a shared dirty tree. Nonempty `git diff HEAD --` the three simulation/test files has hash `65f4ab0adb0832299443b6a405392b8bedf278ac`. Review scope: grounder hunks. Spec: `docs/design/weapon-combat-specification.md:159-163,227-274`. Standards: `AGENTS.md`, `CLAUDE.md`, and all 12 review smells.

## Standards

No hard rule violation in the scoped hunks. Smells: Mysterious Name — none; Duplicated Code — none in scoped behavior; Feature Envy — none; Data Clumps — none; Primitive Obsession — existing weapon ID convention; Repeated Switches — **judgement finding** at `BattleSimulation.cs:5475-5505`: the physical-carrier weapon IDs are hardcoded in a separate predicate from the canonical carrier table, so future weapons may drift. Move this policy into a shared carrier classification with an explicit per-projectile override for mixed cases, and test all 41 entries. Shotgun Surgery — no new avoidable spread; Divergent Change — none in scope; Speculative Generality — none; Message Chains, Middle Man, Refused Bequest — none. Standards findings: 1 judgement.

## Spec

`Grounder_PullsHostilePhysicalShotIntoDevice` proves one hostile physical round enters N0 and detonates near the magnet in Unity EditMode (`receipt:f5d7bb2d-6da1-42fc-a22c-ca0b1c9f6311`). `BattleSimulation.cs:4606-4642` steers and resolves normal impact; `:5507-5516` deducts device HP and removes at zero. **Hard/incomplete finding 1:** `DamageGrounder` has no public damage state event/test, so HP amount and destruction after multiple shots are unverified; add outcome tests and a damage event/visual update. **Hard/incomplete finding 2:** no turn expiry, 10% radius or +1 turn core scaling, and device HP is a placeholder 45 independent of unlock HP; implement and test. **Hard/incomplete finding 3:** no placed `grounder` visual listener/removal, and no friendly/energy negative tests. Test those and inspect PlayMode size. The contact substep now uses its actual position; normal blast still executes. Spec findings: 3 hard incomplete.

Summary: Standards 1 judgement (classifier drift); Spec 3 hard incomplete (HP/expiry/upgrades/visual and exclusions). Verdict: first magnet attraction slice passes, weapon 28 remains incomplete.
