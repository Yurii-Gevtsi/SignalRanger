# Weapon 33 — virus validation

## Result

**Implemented** (mechanics and the status icon). Card 33, `weapon.virus`, TTH limits 4/0/0/0 (Core), one device per battle.

- Use and attaching are those of the repair jammer: it needs a hostile droid nearby (otherwise nothing is spent), a short throw attaches the body, and 1.5 s later the body vanishes without a blast or crater and the status appears. The device itself deals no damage (before this closure it fell through the generic projectile path and dealt plain damage).
- The status lasts three of the carrier's own turn-ends. The first two add nothing (no hidden periodic ticks, it is not the shared poison). At the end of the third, a living carrier takes the final damage — the base grenade's damage (the prototype library value was 48 and is now the grenade's 42; Core steps scale it through the usual modifier) — and the status ends.
- Healing from the deficit: when the status appears, the carrier's missing health (`MaxHP − HP`) is saved. If the carrier dies at any moment of the three turns — by any damage, including the final damage — the owner recovers exactly that saved amount, capped by the owner's maximum (60/100 carrier → 40, not 100; a healthy carrier → 0). A dead owner is not revived and nobody else is healed; a repair jammer on the owner blocks this healing too (spec of the jammer). Deaths are noticed every tick, so every damage source is covered.
- A carrier that already has a virus keeps it; a second device does not overwrite or refresh it.
- Icon: `UI/Weapons/virus` next to the health number (left side; the jammer icon sits on the right), about as wide as the jammer icon (0.55 world units), shown while infected.
- A hostile grounder destroys the device in flight (physical carrier).

## Evidence

- EditMode Gameplay: `Captures/weapon12/w33-edit-2.xml` — the 6 new virus tests and the new view test PASS (attach/1.5 s/no damage/no crater and unavailability without an enemy; no ticks in turns one and two and the grenade damage at the end of the third; deficit saved at infection and not the later damage; cap by the owner's maximum and a zero reserve; death from the final damage, dead owner, jammed owner; registry 4/0/0/0 + one per battle + grenade damage + art + paid Core steps + JSON; icon position, size, state). The other failures in that run are the known baseline ones.
- PlayMode: `Captures/weapon12/w33-play.xml`, 44/44 PASS (new: the real scene shows exactly one virus icon, on the infected droid).
- Full EditMode: `Captures/weapon12/w33-edit-full.xml`, 816 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test; the icon size was derived from the art size, not judged on screen.

## Open items

- Final damage equals the base grenade's prototype 42 (global balance pending).
- Whether a second virus on an infected carrier should refresh or replace the first is not in the card (kept the first).
- The brain-chip art is large and detailed; at 0.55 units it may need a look on a device.
