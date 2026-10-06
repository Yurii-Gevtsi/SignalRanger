# Weapon 24 — EMP cannon validation

## Result

**Partial — needs owner decision.** Base mechanics are implemented and verified; the two purchasable features are not.

Implemented (card 24, `weapon.emp`, TTH limits 4/4/2/4):

- Straight `emp_shot` (verified in the real scene). Every droid in the primary radius, enemy or ally, gets the shared two-turn electric cycle: the direct hit now; a tick of 20% of that hit at the end of each of the victim team's next two turns; refresh without stacking; +25% to the next direct hit of the same element; the existing electro overlay is shown while the status lasts (`IsUnitElectrified`).
- The base adds no movement or aim ban: the electric status does not stop any shot (the card forbids replacing the unclear aim ban by a ban on any shot).
- First purchase swaps the button skin to `emp_5_10_B` (checked through the shared icon resolver).

Not implemented — required owner input (spec open question 2 and the card's own note):

1. `aim_jam` (rank 5, two steps, catalog "1+1, 1→2 turns"): what exactly the blocked aim does — the whole aimed shot, or only changing the direction. The old runtime used a total fire stun, which the card explicitly rejects as a replacement.
2. `movement_lock` (rank 10, two steps, "2→3 turns", needs the maximum of `aim_jam`): confirm the movement ban contract (all directions, jumps, rope and fuel as in shared rule 6) and whether electricity without purchases blocks anything.
3. The old `aim_jam`/`movement_lock` scales are marked "migration question" in the card: confirm whether purchases keep the old 1+1 / 2→3 scale.
Until then the two purchases are sellable but have no gameplay effect; they should not be offered in the shop UI.

## Evidence

- EditMode Battle+Progression: `Captures/weapon12/w24-edit-1.xml`, 215/215 PASS (new: two-turn electric cycle on enemy and ally with exact ticks and no shot ban; refresh/+25%/two ticks in total; registry limits, the two features and the skin ids).
- PlayMode: `Captures/weapon12/w24-play.xml`, 34/34 PASS (new case: `emp_shot` texture on the real projectile).
- Automated editor evidence only.
