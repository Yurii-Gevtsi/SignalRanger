# Weapon 22 — orbital rocket validation

## Result

**Implemented** (mechanics). Card 22, `weapon.orbital-rocket`, TTH limits 4/0/2/4, no feature.

- Selecting the weapon shows only its fire-button icon; no hull is held. A double tap places a red marker. The marker is now drawn as the same bold red "×" used by the modal close buttons (two bars at ±45°, colour 1/0.18/0.16); before this closure it was a plus sign.
- The marker is a draft: choosing another weapon (or the timer ending) removes it. Pressing Fire without a marker needs a target and spends nothing.
- Fire fixes the call (the marker is consumed), spends one charge and ends the own turn. At the end of the next opponent turn, before the own turn starts, the rocket spawns above the arena and falls on the marked place; a fixed call is not cancelled by a later weapon change.
- Battle end is handled explicitly: if the battle has already finished, the pending strike is dropped and nothing falls on the result screen (new guard in `LaunchPendingOrbitalStrikeIfDue`).
- Damage and radius are above the homing rocket (65/2.1 against 52/1.4); the nuclear charge (card 37) must be set above these. Skin: in flight `missle_fly`; fire-button icon `missle_5_10_A`.

## Evidence

- EditMode Battle+Progression: `Captures/weapon12/w22-edit-1.xml`, 207/207 PASS (existing deferred-impact test; new: Fire without a marker, no strike after the battle ended, stronger than the homing rocket and the 4/0/2/4 registry).
- PlayMode: `Captures/weapon12/w22-play-2.xml`, 32/32 PASS (new: marker shape and colour, draft dropped on weapon change, confirm consumes the marker and ends the turn, rocket falls after the opponent turn and hits the marked place).
- Automated editor evidence only: the real double-tap input path was not driven (the controller's marker/confirm entry points were).

## Open items

- Shape/size of the marker was set from the code style of the close glyph, not compared on screen with a modal.
- Global HP/damage balance (shared `H_cap` dependency).
