# Weapon 39 — cryo cannon validation

## Result

**Partial — base implemented and verified; the two purchasable features need the owner.** Card 39, `weapon.freeze`, TTH limits 4/4/2/4.

Changed in the base:

- The cannon previously applied the old movement-blocking freeze (one turn as a base). It now uses the **same shared cold** as the cryo grenade and the cold spider: the direct hit now, a tick of 20% of that hit at the end of each of the victim's next two turns, refresh without stacking, `+25%` to the next direct hit of the same element. Every droid in the primary radius, enemy or ally, is chilled (the ice overlay is shown). The base adds no movement block or aim ban.
- The cold is shared across weapons (checked): a droid chilled by the cannon takes the grenade's next direct hit +25% (and the other way round, by the same status).
- Flight skin `ice_shot` is confirmed in the real scene; the button skin after the first purchase is `freeze_5_10_B` (registry).
- The old purchasable effects (freeze 2/3 turns, the chain bonus 35/60%) used to leak through a legacy mapping onto the new purchases. The canonical resolver now returns the base profile for `weapon.freeze` and `weapon.emp` until the owner defines the features (the same legacy leak was possible for the EMP cannon's aim-jam/movement-lock purchases; the EMP report said they had no effect, and this change makes that true and tests it).

Not implemented — needs the owner (spec open question 2 and the card's note):

1. `full_freeze` (rank 5, two steps): is the complete movement block part of the base or bought? What exactly does it block (all directions, jumps, rope, fuel — shared rule 6) and for how many turns per step?
2. `cold_bonus` (rank 10, three steps): the catalog proposes `+25%` base then `+33⅓% → +41⅔% → +50%` per purchase ("if we keep three purchases"); confirm it, and whether it applies to the cannon's own hits only or to every cold weapon.
Until then both purchases are sellable but have no gameplay effect and should not be offered in the shop UI.

## Evidence

- EditMode Gameplay: `Captures/weapon12/w39-edit-2.xml` — the 4 new cryo-cannon tests PASS (shared cold cycle on enemy and ally with exact ticks and no movement block; refresh/+25%/two ticks; the cold shared with the grenade; registry, art, no leaked legacy effects for the cannon and the EMP cannon). The other failures in that run are the known baseline ones.
- PlayMode: `Captures/weapon12/w39-play.xml`, 50/50 PASS (the parametrised thrown-body test now also covers `ice_shot` on the real projectile and its detonation).
- Full EditMode: `Captures/weapon12/w39-edit-full.xml`, 847 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test.

## Open items

- Damage 36 (prototype) pending the global `H_cap` balance.
- The weapon hint text ("freezes the target, slowing the droid") describes the old behaviour; it is queued with the other elemental texts.
