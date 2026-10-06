# Weapon 23 — guardian drone validation

## Result

**Implemented** (mechanics). Card 23, `weapon.guardian-drone`, TTH limits 4/0/2/4, feature `stealth` (rank 5, one step).

- Placed like a mine, no aim; it drops briefly, lands and stays until it fires or the battle ends. It fires when an enemy comes within the shared `N0`.
- On activation it is revealed, rises, and fires a fast small bullet (`mortira_shot`, small) with a guaranteed hit: the triggered droid cannot dodge by moving away. The hit deals the base damage and ends the active droid's turn. If the target has died before the shot lands nothing is hit and the hit is not moved to another droid.
- A hostile magnet intercepts the bullet instead of the guaranteed hit (the dedicated defence wins; the bullet is a physical carrier).
- Visibility before this closure: an enemy drone was always hidden from the player, even without the purchase. Now the drone is visible to everyone by default; the purchased `stealth` only hides it from the enemy observer (`MineDeployedEvent.ConcealedFromEnemy`, `IsGuardianConcealedFrom`) — the owner always sees it and it is revealed when it fires; the object remains in the shared simulation and still triggers. Body skin `guardian_drone` (evolved variant `guardian_drone_5A` through the shared icon resolver).

## Evidence

- EditMode Battle+Progression: `Captures/weapon12/w23-edit-2.xml`, 212/212 PASS (new: placed without aim and the target cannot run away, dead target hurts nobody else, stealth is an observer property and a hidden drone still fires, magnet interception, registry 4/0/2/4 + one stealth step + art present, resolver flag).
- PlayMode: `Captures/weapon12/w23-play.xml`, 33/33 PASS (new: in the real scene an ordinary enemy drone is shown, a stealth enemy drone is hidden from the player and the owner's drone is always shown).
- Automated editor evidence only.

## Open items

- Damage equals the prototype value (42), not yet tied to the mine's final damage (global balance dependency).
- Purchases of Core/Loadout/Radius are plain TTH steps; there is no force-field interaction yet (card 29).
