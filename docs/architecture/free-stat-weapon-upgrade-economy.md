# Free-stat weapon upgrade economy

## Contract

Each owned weapon starts at rank 1. A weapon receives one available rank for
each commander level reached after its unlock level. The player chooses exactly
one of four parameters for that rank:

- Core — damage;
- Aim — accuracy/range;
- Loadout — ammunition or uses;
- Radius — impact radius or effect reach.

The choice is stored per weapon and target rank. It is not a global choice and
it can be different for every weapon and every rank.

## Economy policy

The existing rank curve remains the pacing baseline. The parameter multiplier
is applied to credits, weapon chips, and the premium crystal alternative:

| Parameter | Multiplier |
| --- | ---: |
| Core | 1.25x |
| Loadout | 1.20x |
| Aim | 1.00x |
| Radius | 0.85x |

The normal payment is credits plus weapon-specific chips. Crystals are an
alternative payment path, not a third mandatory cost. The UI shows all three
future amounts even when the weapon is not yet eligible for its next rank; the
action remains disabled until the commander level and selected payment path
are valid.

## Compatibility

Existing saves keep their rank and branch fields. If a save has no explicit
free-choice record for a purchased rank, the shared resolver derives a
temporary compatible choice from the old catalog node. New purchases always
write an explicit choice. Existing evolution data remains readable for old
saves, while the new four-parameter path no longer requires selecting an
evolution branch.

## Ownership

`WeaponUpgradeService` owns validation and spending. `WeaponUpgradeEconomy`
owns costs and commander-level gates. `WeaponUpgradeChoiceResolver` is the
single read path used by the workshop, shop, hangar, and battle modifiers.
