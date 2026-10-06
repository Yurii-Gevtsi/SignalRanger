# Weapon 19 — incendiary grenade validation

## Result

**Implemented** (mechanics). Card 19, `weapon.incendiary-grenade`, TTH limits 4/4/2/4, no purchasable feature.

- Throw and blast apply the shared two-turn fire cycle (`ElementalStatus`, same rules as cold: no instant tick, 20% of the direct hit at the end of each of the victim team's next two turns, refresh without stacking, +25% to the next direct hit) to every droid in the primary radius, ally or enemy.
- The ground flames (`fire_shot_1…4` blobs, four puddles) are kept as presentation only: their damage budget is 0, so they neither hurt nor ignite a droid that enters later.
- The burning droid reuses the existing lava/burn overlay (`IsUnitLavaBurning` includes the fire status). The fire gun (card 40) still uses its legacy burn path until it is migrated.
- Skin: body `incendiary_grenade` in flight (verified in the real scene together with cryo and hopping).

## Evidence

- EditMode Battle+Progression: `Captures/weapon12/w19-edit-1.xml`, 198/198 PASS (cycle on enemy and ally with exact tick timing; presentation-only ground fire with a late-comer; repeat hit refresh/+25%/two ticks in total; limits, no feature, art resources present).
- Full EditMode: `Captures/weapon12/w19-edit-full.xml`, 727 tests, only the 38 baseline failures (no new failure).
- PlayMode `StarterWeaponsBattleSmokeTests`: `Captures/weapon12/w19-play-3.xml`, 29/29 PASS. (A first run right after an EditMode session failed one test on a foreign log line from the multiplayer bootstrap; the identical rerun is green.)

## Open items

- Overlay look and the flame art were not inspected on screen.
- Global HP/damage balance (prototype damage ~ weapon 40); Loadout base stock TBD.
