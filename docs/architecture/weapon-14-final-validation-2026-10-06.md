# Weapon 14 — sticky grenade validation

## Result

Mechanics: **implemented** for the defined part; **needs owner decision** for the rest (see Open items). Card 14, catalog row `sticky_grenade` (`weapon.sticky-grenade`, TTH limits 4/4/2/4).

- Base: arc throw, sticks to a droid or surface, 1.5 s fuse, standard-grenade blast, no status.
- `toxic_release` (rank 5, id `toxic-release`): the blast applies the shared poison cycle to every droid (enemy or ally) inside the primary radius. No instant tick; ticks (20% of the direct hit that droid took) at the end of the victim's two next turns, then the status and overlay are removed. Implemented through the existing shared poison path (`ApplyPoison`) selected by `WeaponEvolutionProfile.StickyToxicRelease`.
- Skins: flight/held body `sticky_grenade`; after the feature `sticky_grenade_5A` (verified in the real battle scene for both states).
- Loadout limit 2 and Aim 2 reach rank 5; third Loadout purchase is refused; second, invented feature option is refused; JSON round-trip restores the feature.

## Evidence

- EditMode Battle+Catalog: `Captures/weapon12/w14-edit-2.xml`, 178/178 PASS (new: base sticks/fuse/no poison; toxic cycle on enemy and ally with exact tick timing; paid service + JSON + icon resolution).
- PlayMode `StarterWeaponsBattleSmokeTests`: `Captures/weapon12/w14-play-2.xml`, 24/24 PASS (new: base and purchased body texture on the thrown grenade, and detonation).
- Automated editor evidence only, not a physical device.

## Open items (owner input)

1. Catalog allows four purchases of `toxic-release`; the spec says the role of the old `1+3` duration scale is an open question. Purchases 2–4 currently change nothing and must not be sold as meaningful until decided.
2. Whether a toxic cloud stays on the ground and infects later arrivals (spec: do not invent a persistent field) and the cloud art (owner will supply).
3. Global HP/damage balance (shared `H_cap` dependency); prototype damage 40.
