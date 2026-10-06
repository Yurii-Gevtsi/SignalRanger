# Grounder test design (weapon 28)

Source: `docs/design/weapon-combat-specification.md:159-163,227-274`, `docs/design/weapon-catalog-canonical.md:155,211`. Existing state: the weapon is in the catalog but `BattleSimulation.IsBattleMechanicAvailable` blocks combat use, progression grants zero radius purchases, and there is no deployed magnet entity.

## Test scope contract

Goal: make the installed grounder intercept hostile physical carriers in combat while preserving explosion damage to the magnet, ordinary nearby unit damage, finite health, upgrade radius and lifetime, and the `grounder` art. In scope: selecting/placing without aim, one-per-battle consumption, the deployed object's turn lifecycle, projectile steering and impact, physical/energy classification, visual spawn/update/remove, four radius and four core upgrades, and localized core text in all existing languages. Out of scope: completing force-field, the future HP-by-level curve, balancing every attack's damage, and changing the underlying flight solver. The proposed 45% H_unlock health is a balancing input to record explicitly, not a license to invent a level-30 HP curve.

Allowed seams: public `BattleSimulation.TryFire`, `Tick`, turn advancement, projectile and deployed-device events; `BattleWeaponProgression`/catalog purchase mapping; `BattleSceneController` visual resource lookup. Existing `BattleSimulationTests` and progression/resource tests are preferred. A narrow test-only inspector can expose active device state only if the public event/state is insufficient; it must not replace behavioral assertions.

Completion: focused Unity EditMode tests pass for placement, hostile physical attraction and magnet damage, friendly and energy non-interception, expiry, radius/core scaling, and sprite selection. PlayMode or an equivalent Unity visual capture must confirm the installed object's visible size and removal; absent that capture, report the visual gate as open.

## Acceptance-to-assertion mapping

| Behavior | Public observation |
|---|---|
| Place without aim; no healing; one use per battle | `TryFire` succeeds with invalid aim once, device spawn event fires, owner's HP unchanged, second fire cannot consume another charge |
| Hostile physical carrier in N0 | Projectile position bends toward installed magnet, impacts at its vicinity, magnet health drops and projectile explosion event remains visible |
| Friendly or energy carrier | Flight/impact follows ordinary path and magnet health stays unchanged |
| Units near magnet's actual blast | Only units inside the weapon's impact radius take damage; owner receives no automatic redirected damage |
| Health and destruction | Multiple hits reduce configured health and removal event fires at zero |
| Lifetime | Device persists for two specified turn boundaries, then removal event fires; four core upgrades extend to six |
| Radius | Four purchases expand base N0 by exactly 10% each, independently of the core upgrades |
| Visual | Resource lookup resolves `grounder` with alpha, and deployed presentation uses that sprite |

Minimum independent vertical slice: placement, one hostile physical projectile entering N0, attraction, explosion, and magnet damage via the public simulation. It is currently expected to fail at `TryFire` because the weapon is blocked, giving a meaningful RED before implementation. No assertion may be removed, skipped, weakened, or replaced by mock-only behavior merely to make the run green.
