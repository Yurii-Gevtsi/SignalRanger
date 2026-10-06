# Weapon 21 — homing rocket validation

## Result

**Implemented** (mechanics). Card 21, `weapon.missle`, TTH limits 4/4/0/4, one use per battle, no feature.

- The old two-purchase kit limit is removed from the registry (Loadout limit 0) and the library stock is now 1: one launch empties the inventory.
- After launch the rocket keeps steering to its target with a bounded turn rate (150 deg/s, a smooth arc; the earlier 210 deg/s snapped round and 110 deg/s made it circle the target) and, without any purchase, may climb over up to two obstacles on its way (waypoint above the obstacle). It does not search a path around the whole map.
- Skin: in flight the baked `missle_fly` sprite only; no second exhaust layer is added (verified in the real scene); the held body keeps `missle` and the separate cleaned tail `UI/Generated/missle_tail_clean` exists for the hull variant.
- A direct blast (prototype 52) hurts but does not kill a healthy 100 HP droid; it stays below the orbital and nuclear rockets by design (those values are set when cards 22 and 37 are closed).

## Evidence

- EditMode Battle+Progression: `Captures/weapon12/w21-edit-2.xml`, 204/204 PASS (new: single use and 4/4/0/4 without feature; homing hit with bounded turn rate and no kill; climbing over a wall without destroying it; the legacy detour test adapted to the two base bypasses).
- PlayMode: `Captures/weapon12/w21-play.xml`, 31/31 PASS (new case: `missle_fly` texture on the real projectile, no `Rocket Trail` child, detonation).
- Automated editor evidence only; the arc shape was judged by measured turn rate, not on screen.

## Open items

- Steering rate and the two bypasses are prototype values; the owner has not approved a feel yet.
- Global HP/damage balance (shared `H_cap` dependency).
- The legacy split/bypass evolution code remains reachable only from old saves; the catalog no longer offers it.
