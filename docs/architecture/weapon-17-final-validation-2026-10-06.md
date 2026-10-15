# Weapon 17 — cryo grenade validation

## Result

**Implemented** (mechanics). Card 17, `weapon.cryo-grenade`, TTH limits 4/4/2/4, no purchasable feature.

- Arc throw and blast as a grenade; every droid (enemy or ally) inside the primary radius gets the shared cold status. No instant tick.
- Shared elemental cycle (`ElementalStatus`, new, reused later by the other elements): the tick is 20% of the direct hit that droid took; it is applied at the end of each of the victim team's next two turns, then the status is removed.
- A repeat hit of the same element refreshes the duration to the full two turns (no stacking, `1 + 2 -> 2`, two ticks in total, not three), replaces the tick with 20% of the new hit and boosts only its own direct damage by the shared +25% (`ElementalRepeatBonusPercent`).
- The old behaviour (cryo grenade = one-turn freeze/lock) is removed for the grenade; the cryo cannon keeps its legacy freeze path until card 39.
- Skin: flight/held body `cryo_grenade`; chilled droids reuse the existing freeze overlay (`IsUnitChilled` is OR-ed into it).

## Evidence

- EditMode Battle+Progression: `Captures/weapon12/w17-edit-4.xml`, 191/191 PASS (new: two-turn cycle with exact tick timing on enemy and ally, no movement ban; repeat hit refresh/+25%/two ticks in total; catalog limits, no feature, skin id; the old one-turn-freeze test was replaced by a chill test).
- PlayMode: `Captures/weapon12/w17-play.xml`, 29/29 PASS (parametrised thrown-body skin and detonation for cryo, hopping and incendiary grenades).
- Automated editor evidence only; the overlay on a chilled droid was not inspected on screen.

## Open items

- Whether base cold should also forbid movement/aim is the open owner question for `full_freeze`/`aim_jam` (spec question 2); the grenade currently adds no ban.
- Global HP/damage balance (prototype damage 34); Loadout base stock TBD.
