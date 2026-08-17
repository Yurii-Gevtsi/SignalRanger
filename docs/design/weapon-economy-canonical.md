# Weapon acquisition and upgrade canonical sheet

Status: editable balance baseline. Runtime values are extracted from `WeaponProgressionCatalog`, `PrototypeWeaponLibrary` and `ShopScreenController`; this document defines the target economy and the rules for future changes.

For weapon names, branch text/mechanics, the full arsenal roster (starter/shop/unique/campaign-reward), and per-weapon level/price, see `weapon-arsenal-consolidated-proposal.md` — it is now the canonical source for those. The general rules below (rank cost curve, historical power score, monetization guardrails) still apply and are not superseded.

## Ownership rules

- Commander level unlocks eligibility; credits or the listed crystal alternative purchase the weapon.
- Starting weapons are available immediately and do not require purchase.
- Blueprints are weapon-specific progression materials and never replace the purchase price.
- Every purchase and upgrade is ledger-idempotent and must be safe to retry.
- Premium can reduce waiting or provide convenience, but may not create a weapon or combat mechanic unavailable to Free-to-Play players.

## Upgrade rules

- Ranks 1–10 per weapon.
- Costs to advance ranks 2–10: 300/500/800/1,200/1,800/2,500/3,500/5,000/7,000 credits and 10/15/25/40/60/80/110/150/220 blueprints.
- Total from rank 1 to rank 10: 22,600 credits and 710 blueprints.
- Rank 5 selects branch A or B; rank 10 is the capstone of the selected branch.
- Rank nodes should normally improve one axis at a time: damage, radius, range, ammo, projectile count, control, terrain damage, healing, accuracy, homing or utility duration.
- Do not stack multiple large damage increases on one branch. A branch should trade damage for control, reliability or terrain interaction.

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

Matchmaking starts within ±10% of historical power, expands to ±20% after 20 seconds and ±35% after 45 seconds. Use selected-loadout power only as a tie-breaker and abuse detector; never let a player lower matchmaking power by equipping an intentionally weak loadout. Competitive season resets may reset visible rating, but not historical power.

## Monetization guardrails

- Sell convenience, cosmetics, extra presets, respec alternatives and time savings.
- Do not sell exclusive combat weapons, exclusive damage mechanics or unmatchable stat tiers.
- Any paid acceleration must be earnable through normal play within a defined target window.
- Rewarded ads may grant a capped PvE bonus; they do not function in competitive PvP.
- Recommended initial caps: 5 rewarded ad claims per day, 2 combat-use extensions per day and 1 post-battle reward multiplier per battle.

## Change checklist

When adding a weapon, update: base definition, unlock level/price, blueprint source, upgrade nodes, branch text, historical-power contribution, shop presentation, localization keys, tests and this sheet.

