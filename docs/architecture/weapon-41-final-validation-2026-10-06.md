# Weapon 41 — nanotoxic charge validation

## Result

**Partial — base implemented and verified; the two purchasable features need the owner.** Card 41, `weapon.toxic`, TTH limits 4/4/2/4.

- The base was already the shared poison cycle (two victim turn-ends, a tick of 20% of the direct hit, refresh without stacking, every droid in the primary radius, enemy or ally, the poison overlay). This closure verified it in the real scene and added what was missing: the **shared +25% repeat bonus** now also works for poison (a droid already poisoned by this charge, the toxic sticky grenade or the poison spider takes +25% direct damage from the next poison hit; the ticks do not grow).
- The base adds no movement damage: walking while poisoned costs nothing extra until the movement feature is bought (checked).
- The old purchasable effects (armor corrosion, per-second movement damage, longer duration) used to leak onto the new purchases through a legacy mapping; the canonical resolver now returns the base profile for the charge until the owner defines the features. The legacy-branch resolver tests for old saves are unchanged.
- Skins: `toxic_shot` on the flying charge (confirmed in the real scene), button skin `toxic_5_10_A` after the first purchase (registry).

Not implemented — needs the owner (spec open question 2 and the card's note):

1. `movement_poison` (rank 5, two steps): the damage per accumulated second of movement `N` is not given in the catalog (the spec says it is "proposed in the catalog", but only the 20% status is there), and the role of the two purchases ("1+1") is open. The spec's accounting proposal — only the poisoned droid's own controlled movement, not knockback or falling, with fractions of a second accumulated inside the status — is not yet implemented either (the legacy code counts any unstable motion).
2. `poison_bonus` (rank 10, three steps): the catalog proposes `+25%` base then `+33⅓% → +41⅔% → +50%`; confirm it and whether it applies to the charge's own hits only.
3. The Radius purchases ("longer negative effect") have no number.
Until then both feature purchases are sellable but have no gameplay effect and should not be offered in the shop UI.

## Evidence

- EditMode Gameplay: `Captures/weapon12/w41-edit-1.xml` — the 3 new nanotoxic tests PASS (poison on enemy and ally with exact two-tick timing and no movement damage; refresh/+25%/two ticks in total; registry, art, no leaked legacy effects) together with the pre-existing toxic and sticky-toxic tests. The other failures in that run are the known baseline ones.
- PlayMode: `Captures/weapon12/w41-play.xml`, 52/52 PASS (the parametrised thrown-body test now also covers `toxic_shot`). Full EditMode: `Captures/weapon12/w41-edit-full.xml`, 854 tests, only the 38 baseline failures of `7e4388f`. Localization: the audit and the integrity gate PASS after the four hint updates.
- Automated editor evidence only; not a phone test.

## Open items

- Damage 30 (prototype) pending the global `H_cap` balance.
- The weapon hint text was updated for the four elemental/status weapons (virus, cryo cannon, lava gun, nanotoxic) in all 13 languages.
