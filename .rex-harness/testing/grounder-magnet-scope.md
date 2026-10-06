# Grounder magnet combat test scope

Source: weapon 28 and the carrier classification in `docs/design/weapon-combat-specification.md:159-163,227-274`. Existing verified placement: `Grounder_PlacesWithoutAimAndDoesNotHealOwner` passed in Unity EditMode (`receipt:0086bbce-4cf8-4420-b23e-b97e974ee9b9`). Placement is not the completion target for this work item.

Goal: hostile physical carriers entering the grounder's base `N0` bend toward it, contact and detonate with their normal blast, reduce device HP, and eventually destroy it; friendly and energy carriers remain unaffected. The deployed device persists two hostile turns and scales to six through four core upgrades. Four radius upgrades add 10% each. A placed `grounder` sprite is shown and removed with the device. No global health-by-level curve is introduced; record the initial magnet HP as a local balance value pending that curve.

Out of scope: force-field absorption, drone or trap redesign beyond their projectile carrier classification, balance changes to all other weapons, an invented 60-level HP progression, and rewriting the shared projectile integrator.

Allowed seams: public `BattleSimulation.TryFire`, `Tick`, `Spawn`, turn events, projectile and grounder events; `BattleWeaponProgression` and catalog; `BattleSceneController` resource mapping and spawned visual hierarchy. Prefer focused Unity EditMode tests and PlayMode visual capture. Completion requires Unity passes for physical, friendly, energy, health, expiry, upgrades, and sprite; a failure in any one keeps weapon 28 incomplete. Tests must assert outcomes, not merely private helper calls or mocks; no skipping or weakened assertions.

| Acceptance behavior | Observable assertion |
|---|---|
| Hostile physical shot | `ProjectileExploded` near device after a shot otherwise passing by; `GrounderDamaged` reports reduced HP and normal blast still harms an in-radius unit |
| Friendly and energy shots | Same shot path remains unredirected and no `GrounderDamaged` |
| Destruction | Repeated contacts emit exactly one `GrounderRemoved` and later shots are unredirected |
| Lifetime | Two completed hostile turns remove the base device; four core purchases persist to six |
| Radius | A shot just outside N0 stays free at base and is captured at +40% radius |
| Visual | Placed renderer uses alpha-backed `grounder` icon at bounded size; removal destroys renderer |

Minimum RED: player places grounder, hostile physical projectile enters base N0, assert contact damage event. Current implementation has no interception loop or damage event, so the test must fail by an observed combat outcome, independently from placement.
