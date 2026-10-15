# Weapon acquisition and upgrade canonical sheet

Status: historical economy/runtime reference, superseded for target balance on 2026-09-16 by [the canonical arsenal, progression and economy catalog](weapon-catalog-canonical.md). Edit all target prices, rewards, level gates and upgrade rules there, not here. The old rank-10 cap, price modifiers and pacing tables below are not implementation requirements.

For current names, mechanics, roster, per-weapon level/price and all mode rewards, see `weapon-catalog-canonical.md`. Older values remain below only to explain existing implementation and migration needs. Monetization safety principles remain relevant where they do not conflict with the canonical catalog.

## Ownership rules

- Commander level unlocks eligibility; credits plus weapon-specific chips are the normal upgrade payment, while the listed crystal amount is an alternative payment path.
- Starting weapons are available immediately and do not require purchase.
- Blueprints are the persisted weapon-specific chip balance used by upgrades.
- Every purchase and upgrade is ledger-idempotent and must be safe to retry.
- Premium can reduce waiting or provide convenience, but may not create a weapon or combat mechanic unavailable to Free-to-Play players.

## Upgrade rules

- Ranks 1–10 per weapon.
- The rank baseline for advances 2–10 is 300/500/800/1,200/1,800/2,500/3,500/5,000/7,000 credits and 10/15/25/40/60/80/110/150/220 chips. The selected TTH applies its own multiplier: Core 1.25x, Loadout 1.20x, Aim 1.00x, Radius 0.85x.
- Total from rank 1 to rank 10: 22,600 credits and 710 blueprints.
- Every commander level after a weapon's unlock level banks one additional weapon rank. The player chooses one of Core, Aim, Loadout, or Radius for each rank.
- Rank nodes should normally improve one axis at a time: damage, radius, range, ammo, projectile count, control, terrain damage, healing, accuracy, homing or utility duration.
- Keep Core and Loadout as the strongest and most expensive choices; Aim and Radius should remain cheaper and weaker so the four paths have a meaningful budget trade-off.

## Initial time-to-rank targets

These are starting targets, not permanent balance:

| Milestone | Target active play | Tuning implication |
|---|---:|---|
| First new weapon | 30–60 minutes | Level gate and credits should align with early arena income |
| Rank 3 on a main weapon | 60–120 minutes | Enough blueprints from campaign plus arena |
| Rank 5 evolution | 2–4 hours | The first meaningful build choice |
| Rank 10 on one weapon | 8–12 hours | Long-term goal without mandatory spending |
| Full 15-weapon catalog | 40–60 hours | Requires variety, not one repetitive farm loop |

Use measured median active-play data, not theoretical reward maximums. If a target is missed by more than 20%, adjust rewards or costs through remote config before changing combat values.

## Historical PvP power score

The first PvP version should match players using a historical account-strength score, not only commander level. The score is monotonic: it never decreases after a purchase, upgrade or level increase.

```text
historicalPower = max(previousHistoricalPower, currentAccountPower)

currentAccountPower =
  100 × commanderLevel
  + 250 × ownedWeaponCount
  + 75 × sum(allPurchasedWeaponRanks - 1)
  + 150 × evolutionChoicesMade
  + droneUpgradePower
```

Recommended `droneUpgradePower` values: +100 per armor/health tier, +80 per mobility tier, +60 per jump/trajectory tier and +40 per comfort-only tier. Cosmetic items, subscriptions, ad skips and inventory capacity do not add power.

Matchmaking starts within ±10% of historical power and expands to ±20% after 20 seconds. At 45 seconds the search stops waiting for a live opponent and the match is filled by a generated one built inside the same band, so an empty early queue never becomes an empty screen; see `raid-and-pvp-battle-modes.md`. Use selected-loadout power only as a tie-breaker and abuse detector; never let a player lower matchmaking power by equipping an intentionally weak loadout. Competitive season resets may reset visible rating, but not historical power.

## Monetization guardrails

- Sell convenience, cosmetics, extra presets, respec alternatives and time savings.
- Do not sell exclusive combat weapons, exclusive damage mechanics or unmatchable stat tiers.
- Any paid acceleration must be earnable through normal play within a defined target window.
- Rewarded ads may grant a capped PvE bonus; they do not function in competitive PvP.
- Recommended initial caps: 5 rewarded ad claims per day, 2 combat-use extensions per day and 1 post-battle reward multiplier per battle.

## Change checklist

When adding a weapon, update: base definition, unlock level/price, blueprint source, upgrade nodes, branch text, historical-power contribution, shop presentation, localization keys, tests and this sheet.
