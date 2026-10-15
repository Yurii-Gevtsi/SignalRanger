# Weapon 20 — repair jammer validation

## Result

**Implemented** (mechanics). Card 20, `weapon.repair-jammer`, TTH limits 0/0/2/2 (Loadout, Radius = duration), no feature.

The runtime already matched the card; this closure adds the missing evidence.

- No aim. With no enemy within `N0` (2.75) the press is unavailable: no charge, no turn and no attack right are spent.
- With an enemy near, a short toss attaches the body to it (it follows the droid); after 1.5 s the body vanishes with no blast, no damage and no crater, and only then does the status start (not before).
- The status blocks every repair source implemented so far — repair kit and timed regeneration (virus healing and any resource heal must route through the same `IsUnitRepairJammed` check when those weapons are closed). Duration 2 turns of the victim, 3 and 4 with the two Radius purchases (read from the saved progress).
- The `no-repair-2` icon (`UI/Common/no-repair-2`, real alpha: corners 0, about 53% transparent pixels) is shown next to the jammed droid only; the body skin is `UI/Weapons/repair_jammer`.

## Evidence

- EditMode Battle+Progression: `Captures/weapon12/w20-edit-1.xml`, 201/201 PASS (new: unavailable without an enemy and nothing spent; 1.5 s delay, no damage/crater and the body leaves; limits 0/0/2/2, JSON round-trip, bonus 2 turns). Existing tests keep covering: no direct damage, healing blocked for two victim turns, regeneration blocked, four turns with two upgrades, icon resource and view state.
- PlayMode: `Captures/weapon12/w20-play.xml`, 30/30 PASS (new: real scene, jammer used next to an enemy, exactly one `no-repair-2` icon visible).

## Open items

- Healing by virus (card 33) and by any resource pick-up must call the same block when implemented.
- Loadout base stock is catalog TBD (prototype 2).
