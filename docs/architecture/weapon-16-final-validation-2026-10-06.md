# Weapon 16 — rail rifle validation

## Result

**Implemented** (mechanics). Card 16, `weapon.rail-rifle`, TTH limits 4/0/4/0 (Core, Loadout), feature `terrain-pierce` (rank 5, four steps).

- A straight bullet (no gravity, shared start speed `V0`) tunnels through ground, carving a narrow channel (radius 0.12), and passes through every droid on its line.
- Shared penetration budget `P0` = one second of travel inside obstacles (same value as the drill rocket base). Time spent inside ground or inside a droid body spends it equally, so a body costs its real thickness. When it is spent the bullet ends where it is; otherwise it flies on until it leaves the arena.
- Every droid is hurt once per pass with the full damage (the proposed uniform damage; no falloff per pierced droid). The shooter is hit only after the bullet has left its hitbox once.
- `terrain_pierce`: each of the four steps adds 10% of `P0` (linear; 140% at four). Without the purchase the base budget already works.
- A hostile magnet (grounder) stops a rail bullet (physical carrier) at the device. A bullet that simply runs out concludes the shot and hands the turn over (retreat window) like an impact would.
- Skin: `UI/Weapons/blaster_shot`, rotated along the flight, scaled so the opaque bolt is about 0.035 world units thick (pellet-sized).

## Evidence

- EditMode Battle+Progression: `Captures/weapon12/w16-edit-4.xml`, 188/188 PASS (new: two droids hurt once each and the shooter untouched, narrow channel and droid behind the wall, budget exhaustion at the expected length and +40% with four steps, paid service/limits/JSON, shared speed).
- PlayMode `StarterWeaponsBattleSmokeTests`: `Captures/weapon12/w16-play.xml`, 26/26 PASS (new: real scene shot, texture, speed, bolt scale/orientation, bolt leaves the arena).
- Full EditMode: `Captures/weapon12/w16-edit-full.xml`, 717 tests, only the 38 baseline failures.
- Automated editor evidence only; no screenshot or physical-device run of the bolt size.

## Open items

- Bolt visual size was derived from the sprite's opaque bounds, not judged on screen.
- Damage per pierced droid is uniform (spec proposal) pending owner confirmation; global HP/damage balance pending.
- Loadout base stock is catalog TBD (prototype 2).
