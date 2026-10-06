# Weapon 18 — hopping grenade validation

## Result

**Implemented** (mechanics). Card 18, `weapon.hopping-grenade`, TTH limits 4/4/4/4, no purchasable feature.

- First throw uses grenade physics (it ignores droids in flight like the ordinary grenade; before this change it exploded on droid contact).
- A repeated fire press launches a jump, allowed both in flight and after landing; it never spends another item and the weapon stays selected for it.
- Jump budget: one extra jump as standard; the ТТХ "Radius" path buys extra jumps (+1 each, five in total) and no longer enlarges the blast (`ResolveImpactRadiusPercent` is 0 for this weapon; the displayed Radius value is the jump total). A press with no jump left is blocked.
- Fuse and jump counter are separate: the 3 s fuse starts once at the first surface contact, keeps counting through jumps (resting or flying) and detonates the grenade at zero, even in the air.
- Skin: the grenade body `hopping_grenade` in flight/at rest.

## Evidence

- EditMode Battle+Progression: `Captures/weapon12/w18-edit-2.xml`, 194/194 PASS (new: one jump in flight and then blocked; fuse measured 3 s from first contact through a mid-fuse jump with five jumps available; Radius buys jumps not blast, limits 4/4/4/4, no feature). The old post-landing-jump test was adapted: a jump is now allowed already in flight, so it waits for the resting state explicitly.
- PlayMode: `Captures/weapon12/w18-play.xml` PASS (parametrised grenade body skin and detonation for cryo/hopping/incendiary, plus the earlier set).
- Automated editor evidence only.

## Open items

- Loadout base stock is catalog TBD (prototype 3); global HP/damage balance (prototype damage 37).
- The jump launch speeds (5.5 / 3.8) are prototype values.
