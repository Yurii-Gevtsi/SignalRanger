# Weapon 40 — lava gun validation

## Result

**Partial — base implemented and verified; the two purchasable features need the owner.** Card 40, `weapon.fire-gun`, TTH limits 4/4/2/4.

Changed in the base:

- The whole stream is **one attack**: it has a single damage budget (the weapon's damage, scaled by Core steps) spread over the flame drops and their burn time. A droid can never take more than that budget from one stream, and no drop deals the full weapon damage (a test checks it against a droid and an ally standing in the fire).
- The stream now applies the **shared fire status**, the same cycle as the incendiary grenade and the fire spider: every droid that takes damage from the stream — enemy or ally — gets the status; its tick is 20% of everything this attack dealt to that droid (it grows with the drops and is final when the fire dies out), it lasts the next two turn-ends of the victim, a new attack refreshes it without stacking, and an earlier fire status boosts the whole next attack's direct damage by 25% (one boost per attack, not per drop). The lava overlay is kept as the visual.
- The legacy purchasable effects (longer stream, ignite, chain damage) used to leak onto the new purchases through a legacy mapping; the canonical resolver now returns the base profile for the lava gun until the owner defines the features.
- Skins: the drops use `fire_shot_1…4` (animated) in the real scene (checked); the button skin after the first purchase is `fire_gun_5_10_A` (registry).

Not implemented — needs the owner (spec open question 2):

1. `movement_lock_burn` (rank 5, two steps): is the movement block part of the base or bought, what it blocks and for how many turns per step?
2. `burn_bonus` (rank 10, three steps): the catalog proposes `+25%` base then `+33⅓% → +41⅔% → +50%`; confirm it and whether it applies to the lava gun's own hits only.
3. The Radius purchases of the three element cannons are described as "longer negative effect duration" but no number is given.
Until then both feature purchases are sellable but have no gameplay effect and should not be offered in the shop UI.

## Evidence

- EditMode Gameplay: `Captures/weapon12/w40-edit-1.xml` — the 4 new lava tests PASS (one attack, bounded damage, ally hurt and burning; 20% ticks for two victim turns without stacking; +25% from a previous fire; registry/art/no leaked legacy effects). The other failures in that run are the known baseline ones.
- PlayMode: `Captures/weapon12/w40-play.xml`, 51/51 PASS (new: the real scene throws the flame drops, the droid in the fire gets the status and loses health, nothing logs an error).
- Full EditMode: `Captures/weapon12/w40-edit-full.xml`, 851 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test.

## Open items

- Damage 40 is a prototype pending the global `H_cap` balance; the stream's range, speed and the ground-burning mechanic are unchanged.
- The stream also hurts its owner or allies standing in the fire (unchanged behaviour).
- The weapon hint text still describes the old behaviour; it is queued with the other elemental texts.
