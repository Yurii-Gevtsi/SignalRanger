# Weapon 27 — swap jump validation

## Result

**Implemented** (mechanics). Card 27, `weapon.swap-jump`, TTH limits 0/4/0/4 (Aim, Radius), one charge per battle, no evolution features. Skin: sphere body `teleport_5_10_A`.

- The sphere uses the base teleport physics: it flies, bounces off terrain like the teleport sphere and lands. Before this closure it exploded on the first terrain contact; it now shares the teleport bounce rules.
- Without an enemy in the shared nearby distance `N0` (1.4) at the landing point the owner is simply moved to the sphere. With an enemy nearby the owner still goes to the sphere while the nearest living enemy goes, at the same moment, to the owner's position captured at the throw (not to the owner's later position). Ties are resolved deterministically by unit id. The previous exchange distance was the teleport's own 2.5; it is now the shared `N0`.
- Radius purchases add 10% of the base `N0` each (1.4 → 1.96 at four) — the only radius this weapon has, since it has no explosion.
- Aim purchases use the shared aim-accuracy step. There is no Core or Loadout step: one transfer per battle, the charge is spent on the throw.
- The transfer ends the turn; a hostile grounder intercepts the sphere (physical carrier) and the transfer does not run.

## Evidence

- EditMode `BattleSimulationTests`: `Captures/weapon12/w27-edit-1.xml`, 216/216 PASS (new: bounce like the teleport sphere, shared `N0` and +10% per Radius purchase (1.8 outside the base N0, inside at four purchases, 1.2 inside the base), turn passes and the charge is gone, enemy lands on the throw origin, registry 0/4/0/4 + no features + art + paid Aim/Radius steps and limits + JSON; the existing farther-enemy test was moved to a point that is really inside the new `N0`).
- PlayMode: `Captures/weapon12/w27-play.xml`, 37/37 PASS (the real scene shows the `teleport_5_10_A` body for the thrown sphere and the sphere ends).
- Full EditMode: `Captures/weapon12/w27-edit-full.xml`, 755 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test.

## Open items

- Radius → `N0` is an interpretation (the card lists a Radius scale but the weapon has no blast); the owner should confirm.
- Aim purchases only affect the aim preview/accuracy like on other weapons.
- Loadout is absent by the card; stock stays the single "one per battle" charge.
- Visuals were not checked on screen.
