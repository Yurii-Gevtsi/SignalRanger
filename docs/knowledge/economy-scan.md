# Economy and content audit

Audit date: 2026-08-14. Scope: campaign planets and missions, rewards, weapons, progression, activity quests, currencies, ads and purchase interfaces. Evidence is repository code or design documentation; no external content is treated as a source.

## Executive read

The local vertical slice has two authored campaign chapters on a four-planet map, eight training missions, a two-currency economy, a 15-weapon progression catalog, battle and mission reward grants, and daily/weekly activity UI. The main economy path is implemented locally and transaction-ledger protected. Ads, purchases, premium status and server economy are interfaces/no-op adapters or design-stage work. The largest balance risk is that weapon upgrades add combat power while the documented product direction calls for normalized competitive power; this needs a mode policy before PvP.

## Campaign and planets

`CampaignPlanetCatalog` defines four planets. `chapter.basics` and `chapter.tactical-tools` are assigned to the first two planets; the remaining two have empty chapter IDs and are reserved. `MvpMissionLibrary.CreateAll()` defines eight missions, all on `arena.desert-sci-fi`, with normal difficulty for missions 1-6 and hard for 7-8. Missions 1-5 are in the basics chapter; 6-8 are tactical-tools. Mission unlock is linear: the highest unlocked index advances after first completion and completed mission IDs prevent duplicate first rewards.

Evidence: `Assets/_Game/Runtime/App/CampaignPlanetCatalog.cs:8-56`; `Assets/_Game/Runtime/Progression/Missions/MvpMissionLibrary.cs:3-160`; `Assets/_Game/Runtime/Progression/Missions/CampaignProgressionService.cs:24-118`.

## Currencies, sinks, and rewards

The save model exposes soft currency (`softCurrency`/credits), premium currency (`premiumCurrency`/crystals), commander XP, and per-weapon blueprints. `EconomyService` grants/spends credits and premium currency, grants XP/blueprints, rejects negative amounts, and records idempotent ledger entries. Weapon upgrades atomically spend credits plus weapon-specific blueprints. Admin mode bypasses spend checks for QA.

First-completion mission rewards are: credits 250/300/350/400/450/500/600/700; XP 80/100/120/140/160/180/220/260; blueprints 25/30/35/40/50/60/70/80. Unlocks are authored on missions 3-8. Replay scaling is 40%. Arena victory base rewards are easy 100 credits/40 XP/8 blueprints, medium 150/60/12, hard 220/90/18; non-victory is 50% and performance caps are +30% credits and +10% blueprints. XP is not performance-scaled.

Evidence: `Assets/_Game/Runtime/Progression/Missions/MvpMissionLibrary.cs:8-151`; `Assets/_Game/Runtime/Data/Progression/BattleRewardCatalog.cs:18-88`; `Assets/_Game/Runtime/Economy/EconomyService.cs:19-183`; `docs/design/weapon-progression-economy-audit.md:64-123`.

## Weapon progression and shop

The catalog contains 15 weapon IDs. Impulse and blaster are granted at start; the remaining 13 require commander levels 2-15. Credit unlock prices range from 800 to 7000 and premium alternatives from 16 to 68 crystals. Each owned weapon has ranks 2-10, fixed costs (300/500/800/1200/1800/2500/3500/5000/7000 credits and 10/15/25/40/60/80/110/150/220 blueprints), commander-level offsets, and an A/B branch at rank 5 with capstones at rank 10. `ShopScreenController` presents a seven-item page, locks by level, and supports credit or crystal purchase; purchase calls are ledger-idempotent through `EconomyService`.

Evidence: `Assets/_Game/Runtime/Data/Progression/WeaponProgressionCatalog.cs:68-208`; `Assets/_Game/Runtime/App/ShopScreenController.cs:121-151,519-659`; `docs/design/weapon-progression-economy-audit.md:17-63,124-178`.

## Activity, daily, and weekly content

`ActivityQuestCatalog` exposes seven visible rows per period: a fixed daily login row plus six deterministic daily selections, or seven weekly selections. Daily pool targets/rewards include 3 wins for 10 crystals, 1500 terrain cells for 15 chips, 6000 damage for 20 crystals, 2 arena battles for 450 credits, 5 story missions for 25 crystals, and other movement/shot/ad/resource tasks. Weekly examples include 15 wins for 1500 credits, 10000 terrain cells for 60 crystals, 40000 damage for 40 chips, and 20 rewarded ads for two chests. Claims and counters persist per UTC daily/weekly period and reset at boundaries.

The seven-day login track is implemented in the rewards screen: 500 credits, 10 crystals, one chest, 500 credits, 20 crystals, 20 chips, and one super chest. The UI calls `TryClaimDailyLoginReward` before granting the selected reward. Chest contents and any server-authoritative chest resolution are not defined in the local economy layer.

Evidence: `Assets/_Game/Runtime/Progression/ActivityQuestCatalog.cs:5-116`; `Assets/_Game/Runtime/Progression/ActivityProgressService.cs:155-285`; `Assets/_Game/Runtime/App/RewardsScreenController.cs:323-355,694-726`; `docs/architecture/activity-progress-tracking.md:3-39`.

## Ads, purchases, premium, and backend status

The app has `IAdsService`, `IPurchaseService`, `IPremiumStatusService`, remote-config and cloud-sync abstractions. Current no-op adapters report ads unavailable, purchases unavailable, and premium inactive. The setup and backend documents describe Unity Economy, IAP receipt validation, premium pass, server time, daily caps, remote tuning, and purchase restore/recovery as future stages; these are not evidence of a live production economy. The rewards screen can request rewarded placement `reward.default`, but completion only counts after an adapter event and the default adapter never completes.

Evidence: `Assets/_Game/Runtime/Infrastructure/NoOpAdsService.cs:5-20`; `Assets/_Game/Runtime/Infrastructure/NoOpPurchaseService.cs:3-12`; `Assets/_Game/Runtime/Infrastructure/NoOpPremiumStatusService.cs:5-11`; `Assets/_Game/Runtime/App/RewardsScreenController.cs:588-595`; `docs/setup/ugs-setup.md:170-200`; `docs/architecture/backend-economy-implementation.md` (checklists for future server/IAP work).

## Implemented vs planned

Implemented with high confidence: local save currencies and ledger; campaign unlock/first rewards; eight missions and four-planet map data; battle reward formulas; weapon unlock/purchase and rank-cost catalog; daily/weekly quest rotation and claims; seven-day login UI and local claim state; no-op adapter boundaries.

Implemented in this pass: historical account-power persistence and monotonic calculation, deterministic rewarded-ad guardrails, product ID catalog, receipt-validation/entitlement contracts, idempotent local entitlement store, and canonical campaign pacing/reserved-planet metadata. Still planned or incomplete: live platform receipt validator, server-backed entitlement store, premium pass expiry/grants, remote-config tuning, purchase restore/account recovery, chest contents, event currencies, respec, and actual PvP matchmaking integration.

## Balance gaps and follow-up questions

1. Validate credit and blueprint income against the 22,600-credit/710-blueprint cost of one full weapon track; current campaign and repeat rates do not define a target time-to-rank.
2. Decide whether upgrades are normalized in competitive modes; otherwise progression and crystal unlocks can create a power gap.
3. Define chest/super-chest contents and the Chip/blueprint naming contract.
4. Add a single authoritative reward-claim path for daily quests; the screen currently owns the login grant while quest claims are separate callbacks.
5. Specify daily ad caps, rewarded-ad value, purchase products, restore behavior, and server replay/idempotency rules before enabling monetization.
6. Fill the two reserved planets with chapters only after mission count, arena variety, and reward pacing are designed.
