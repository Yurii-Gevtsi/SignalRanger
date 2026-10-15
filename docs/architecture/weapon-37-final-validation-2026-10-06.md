# Weapon 37 — nuclear charge validation

## Result

**Implemented** (mechanics, skin). Card 37, `weapon.nuclear-charge`, TTH limits 4/0/0/4 (Core, Radius), one use per battle.

- It runs the orbital rocket's scenario and shares its code (`IsMarkedStrikeWeapon`): double-tap marks a red target, the fire button confirms (spending the single charge and the action), a draft marker can be cancelled without spending anything, and the strike falls at the end of the opponent's turn and resolves after it; if the battle has already ended nothing falls.
- Stronger than the orbital: damage 74 = 55% against the orbital rocket's 48% (65), radius `2 × R0` (3.1, against 2.1) — a droid 2.8 away is hurt by the nuclear blast and not by the orbital one. It does not kill a healthy 100 HP droid outright (a test). One blast only: the old side explosions and radiation zone are not added.
- A hostile grounder catches the falling bomb (a physical carrier) like any rocket.
- Skin: the `nuclear_charge` art, rotated so that the nose points down along the fall (the art's nose looks towards the lower left), about 0.8 units long; the held weapon is not drawn in hand (a marked-strike weapon).
- The mechanic is no longer gated off.

## Evidence

- EditMode Gameplay: `Captures/weapon12/w37-edit-2.xml` — the 3 new nuclear tests and the extended grounder matrix case PASS (strike deferred until the enemy turn, harder and wider than the orbital with one blast and nothing lingering; one use, cancellable draft, refused without a marker; registry 4/0/0/4 + 55/48 proportion + 2 R0 + not a one-hit kill + art + paid steps). The other failures of that run are the known baseline ones.
- PlayMode: `Captures/weapon12/w37-play.xml`, 48/48 PASS (real scene: the strike is deferred, the single charge is spent, the bomb falls after the enemy turn nose down at the expected scale and blasts).
- Full EditMode: `Captures/weapon12/w37-edit-full.xml`, 839 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test; the bomb's size was not judged on screen.

## Open items

- Damage 74 is the prototype scaled from the orbital's 65 by 55/48; the real value depends on the global `H_cap` balance.
- The explosion has no dedicated "nuclear" effect art; the ordinary impact flash is used.
- Radius purchases scale the blast through the usual +8% step (the card does not give a nuclear-specific step).
