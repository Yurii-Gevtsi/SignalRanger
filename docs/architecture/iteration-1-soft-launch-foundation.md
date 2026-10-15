# Iteration 1: Soft Launch Foundation

Status: planned.

Goal: turn the closed playable MVP into a release-ready closed-test / early soft-launch slice without expanding scope into PvP, live-ops or complex monetization.

Expected duration: 4-6 weeks.

## Main Principle

The MVP already proves that the battle core works. The next iteration is not "add more features". It is about making the game understandable, stable, data-driven, testable and shippable enough for real testers.

The gameplay core must not know about Google Play, ads, IAP, analytics SDKs, crash SDKs, Firebase, backend or platform plugins.

Reference packages remain useful as product and technical signals, but they are not a blueprint. For each substantial new system, inspect relevant local examples, extract high-level principles and risks, then implement our own models, content, naming, balance and UI.

## Iteration Tracks

### Track A: Architecture Hardening

Purpose: make the current MVP easier to extend without content and services leaking into battle code.

Work:

- Stabilize service boundaries for analytics, ads, IAP, remote config, crash reporting and notifications.
- Keep all SDK-facing code behind interfaces in `Infrastructure`.
- Keep battle runtime dependent only on gameplay/core/data abstractions.
- Introduce authored/catalog-style content definitions before adding large amounts of new content.
- Add content validation tests.
- Add battle event logging for debug, analytics and future replay/PvP readiness.
- Replace hardcoded difficulty behavior with tunable AI profiles.

Preferred dependency direction:

```text
UI -> App/Core view models and interfaces
Gameplay -> Core/Data interfaces
Progression/Economy/Inventory -> Core/Data interfaces
Infrastructure -> Core interfaces
App -> composes concrete implementations
```

Existing folder structure is acceptable. New work should move toward this shape without large one-shot reorganizations:

```text
Assets/_Game/Runtime/
  App/
  Core/
  Gameplay/
  Data/
  UI/
  Progression/
  Economy/
  Inventory/
  Audio/
  Infrastructure/
```

### Track B: Content Pipeline And Campaign Slice

Purpose: grow from 8 MVP missions to a small but coherent first campaign slice.

Target content:

| Content | Current | Iteration target |
|---|---:|---:|
| Campaign missions | 8 | 18-24 |
| Arenas | 3 | 5 |
| Weapons/abilities | 8 | 8-12 maximum |
| Character archetypes | 3 | keep 3, explain roles better |
| AI profiles | basic | 3 difficulty profiles |
| Elite/boss-style missions | none/minimal | 2 special missions |
| Tutorial/onboarding | basic | guided first-time flow |

Content should become data-driven before it becomes much larger.

Definitions to introduce or stabilize:

- `WeaponDefinition`
- `UnitArchetypeDefinition`
- `ArenaDefinition`
- `MissionDefinition`
- `RewardDefinition`
- `CampaignChapterDefinition`
- `EnemyLoadoutDefinition`
- `DifficultyProfileDefinition`
- `AiProfileDefinition`

Suggested `MissionDefinition` fields:

```text
id
chapterId
displayNameKey
descriptionKey
arenaId
playerTeamPreset
enemyTeamPreset
enemyDifficulty
winCondition
rewardTableId
unlockRequirements
tutorialStepIds
```

Suggested `WeaponDefinition` expansion:

```text
id
displayNameKey
descriptionKey
role
tags
damage
radius
gravity
minPower
maxPower
ammo
cooldown
unlockMissionId
upgradeGroupId
aiUsageWeight
tutorialCategory
iconId
vfxId
sfxId
```

Key rule: behavior can remain custom C# where needed, but balance, unlocks, AI usage and presentation metadata should live in data.

### Track C: UX And Onboarding

Purpose: make the first 10 minutes understandable without developer explanation.

Recommended first-time flow:

```text
Boot
-> Start screen
-> Mission 1 auto-start
-> Guided aim
-> Guided fire
-> First win
-> Reward screen
-> Unlock/loadout explanation
-> Campaign map or next mission CTA
```

First 3 minutes should have almost no optional choices. Menus, loadout and settings should exist, but the first session should lead the player toward the first battle and first reward.

Battle HUD should stay minimal:

- active player HP
- enemy HP
- turn indicator
- selected weapon
- ammo
- power
- fire button
- cancel/utility affordance

Add weapon cards:

```text
Name
Icon
Type
Damage or effect
Ammo
Short description
Best used when...
```

### Track D: Game Feel, Visuals And Audio

Purpose: make the game feel like a coherent game, not only a mechanics simulator.

Do not start with expensive full production art. Use a coherent placeholder-plus style:

- simple characters
- clean silhouettes
- readable arenas
- distinct projectiles
- simple VFX
- consistent UI palette

Minimum visual pass:

| Element | Needed |
|---|---|
| Characters | 3 readable silhouettes |
| Arenas | 5 simple but distinct themes |
| Projectiles | distinct shapes/colors/effects |
| Impact VFX | small/medium/large impacts |
| UI icons | 10-12 weapon icons |
| Rewards | currency icon, unlock icon |
| Store assets | app icon, 5 screenshots |

Minimum audio pass:

- button click
- aim start
- aim release
- projectile launch
- optional projectile flight loop
- impact
- damage
- heal
- cover placed
- unit moved
- unit defeated
- victory
- defeat
- reward

Audio hooks already exist; this iteration should connect real or placeholder assets.

### Track E: Balance, Economy And Analytics

Purpose: understand what players do and keep the early progression simple.

Progression loop:

```text
Play mission
-> earn soft currency
-> unlock weapon or option
-> change loadout
-> pass harder mission
```

Add:

- fixed mission reward plus first-time bonus
- optional 1-3 stars for performance
- campaign milestone unlocks
- one simple currency sink
- very simple local daily reward only if it does not distract from battle
- local weekly challenge only if content is already stable

Do not add yet:

- premium currency
- loot boxes
- crafting
- battle pass
- energy system
- paywall

Minimum analytics events:

- `app_started`
- `tutorial_started`
- `tutorial_completed`
- `mission_started`
- `mission_completed`
- `mission_failed`
- `weapon_selected`
- `battle_quit`
- `reward_claimed`
- `loadout_changed`
- `settings_changed`
- `crash_detected`

Primary metrics:

| Metric | Why |
|---|---|
| Tutorial completion | Shows whether players understand the game. |
| Mission 1-5 completion | Finds early drop-off points. |
| Average battle duration | Checks whether battles are too long. |
| Weapon usage | Finds ignored or overpowered weapons. |
| Fail rate per mission | Guides balance. |
| Return next day | Early retention signal. |
| Crash-free sessions | Technical quality baseline. |

## Data Validation

Content validation should be automated before campaign expansion.

Add tests for:

- all mission ids are unique
- all weapon ids are unique
- every mission references an existing arena
- every mission references an existing enemy loadout
- every reward references an existing currency/item
- every unlock target exists
- every localized key exists
- every weapon has icon/vfx/sfx placeholder ids
- campaign graph has no broken progression
- AI profiles referenced by missions exist
- difficulty profiles stay within allowed numeric ranges

## Battle Event Log

Add a local battle event log. This is not multiplayer and not replay yet; it is a foundation for debugging, analytics and future replay/PvP readiness.

Events:

- `BattleStarted`
- `TurnStarted`
- `WeaponSelected`
- `AimChanged`
- `ShotFired`
- `ProjectileImpacted`
- `DamageApplied`
- `HealApplied`
- `CoverPlaced`
- `UnitMoved`
- `UnitDefeated`
- `BattleFinished`

Rules:

- Events should be structured data, not only strings.
- Events should be emitted from gameplay-facing services or orchestration points.
- Analytics adapters may consume events, but gameplay must not depend on analytics SDKs.
- Keep enough data for debugging balance: mission id, turn, team, unit id, weapon id, positions, damage/heal amounts and result.

## AI Profiles

Do not make AI complex yet. Make it tunable.

Suggested `AiProfileDefinition` fields:

```text
id
aimErrorMin
aimErrorMax
preferredWeaponTags
aggression
defensiveChance
healThreshold
coverChance
movementChance
maxThinkingDelay
```

Difficulty behavior:

| Difficulty | Behavior |
|---|---|
| Easy | Often misses, rarely uses utility. |
| Normal | Aims reasonably, sometimes heals or places cover. |
| Hard | Better weapon choice, finishes low-HP units, uses cover/heal more often. |

## Design Pillars

### Pillar 1: Short Battles

One battle should usually take 2-4 minutes.

### Pillar 2: Understandable Tactics

The player should understand:

- where a shot will go
- why it hit or missed
- how much damage happened
- what each ability does
- why the opponent won

### Pillar 3: Progression Without Pay-To-Win

Progression should add options and tactical variety, not raw paid damage advantage.

Good direction:

```text
more options -> more interesting decisions
```

Bad direction:

```text
more payment -> more combat power
```

### Pillar 4: Content In Small Chapters

Do not build one huge campaign at once. Build small teaching chapters.

Recommended chapters:

- Chapter 1: Learn the basics
- Chapter 2: Cover and healing
- Chapter 3: Arcing weapons
- Chapter 4: Mobility and positioning
- Chapter 5: Mixed tactical fights

## Campaign Structure

### Chapter 1: Basics

| Mission | Teaches |
|---:|---|
| 1 | Aim and fire |
| 2 | Direct projectile |
| 3 | Arcing projectile |
| 4 | Enemy turns |
| 5 | Win/loss and rewards |

### Chapter 2: Tactical Tools

| Mission | Teaches |
|---:|---|
| 6 | Cover placement |
| 7 | Healing |
| 8 | Close-range burst |
| 9 | Movement/relay |
| 10 | Mixed fight |

### Chapter 3: Loadout Choice

| Mission | Teaches |
|---:|---|
| 11 | Limited ammo |
| 12 | Direct vs arcing choice |
| 13 | Enemy behind cover |
| 14 | Multiple enemies or tougher setup |
| 15 | Elite opponent |

### Chapter 4: First Challenge

| Mission | Teaches |
|---:|---|
| 16 | Complex arena |
| 17 | Enemy utility usage |
| 18 | Limited turns or pressure objective |
| 19 | Boss-style enemy |
| 20 | First campaign finale |

## Weekly Plan

### Week 1: Architecture Hardening And Android Build

Goal: stable base.

Tasks:

- Make first Android build/AAB.
- Check package id, orientation, target API and signing.
- Smoke test on a real phone.
- Add first content validation tests.
- Verify save migration/load path.
- Verify all 8 weapons in build, not only Editor.
- Add structured local error logs or basic crash reporting boundary.

Deliverable: internal Android build `v0.1-dev`.

### Week 2: Content Pipeline And Campaign Expansion

Goal: move from 8 missions to first campaign slice.

Tasks:

- Formalize `MissionDefinition`.
- Formalize `ArenaDefinition`.
- Formalize `EnemyLoadoutDefinition`.
- Add 10-12 new missions.
- Reach 18-20 playable campaign missions.
- Add 2 new arenas.
- Add difficulty profiles.
- Expand content validation.

Deliverable: Campaign Chapter 1 playable from start to finish.

### Week 3: UX And Onboarding

Goal: a new player can understand the first 10 minutes.

Tasks:

- First launch flow.
- Tutorial mission.
- Guided aim/fire.
- Weapon explanation cards.
- Better result/reward screen.
- Unlock presentation.
- Clearer loadout screen.
- Mobile safe area and touch-size review.

Deliverable: first 10 minutes playable without developer explanation.

### Week 4: Game Feel

Goal: the game feels responsive and readable.

Tasks:

- Projectile launch VFX.
- Impact VFX.
- Readable damage feedback.
- Heal/cover/move feedback.
- Camera follow/shake tuning.
- Basic SFX.
- Victory/defeat/reward sounds.
- Weapon icon placeholders.

Deliverable: polished battle feel pass v1.

### Week 5: Balance And Analytics

Goal: understand player behavior and tune the early curve.

Tasks:

- Tune weapon damage/radius/ammo.
- Tune mission difficulty curve.
- Tune reward curve.
- Add first-time rewards.
- Add analytics events through service boundary.
- Add debug analytics console/log.
- Manually test fail rates.
- Create balance spreadsheet or ScriptableObject tuning table.

Deliverable: closed-test candidate build.

### Week 6: Closed Test Prep

Goal: prepare distribution to testers.

Tasks:

- Play Console setup.
- Privacy policy.
- Store listing draft.
- App icon.
- Screenshots.
- Closed testing track.
- Test checklist.
- Feedback form.
- Known issues list.
- Crash/ANR monitoring path.

Deliverable: closed test build uploaded.

## Architectural Decisions

### ADR-001: No Live PvP Before Soft Launch

Decision: do not build live PvP before v0.2/v0.3.

Reason: current goal is to validate core fun, onboarding, campaign retention and technical stability.

### ADR-002: Data-Driven Content Before More Features

Decision: definitions, catalogs and validation come before major content growth.

Reason: without this, missions, weapons and rewards will become hard to scale and balance.

### ADR-003: One Soft Currency Only

Decision: keep one soft currency during this iteration.

Reason: the economy has not been validated with real users.

### ADR-004: Utility Weapons Are Tactical Tools

Decision: cover, heal and movement should create tactical choices, not raw power creep.

Reason: this protects fairness and prevents utility tools from becoming paid power upgrades later.

### ADR-005: Battle Events Feed Debug And Analytics

Decision: key battle actions should emit structured battle events.

Reason: this supports debugging, balancing, analytics, replay tooling and future PvP architecture.

## Definition Of Done

Iteration 1 is done when:

- Android build launches on a real phone.
- 18-24 campaign missions are playable.
- 5 arenas exist.
- 8-12 weapons/abilities are available, with no uncontrolled scope creep.
- Guided tutorial exists.
- First 10 minutes are understandable without developer explanation.
- Save/load is stable.
- No critical crashes are known.
- Basic analytics events work through our service boundary.
- Basic VFX/SFX are present.
- Store listing draft is ready.
- Closed test can be started.

## Recommended Execution Order

1. Android build.
2. Content definitions and validation.
3. Campaign expansion to 18-20 missions.
4. Tutorial/onboarding.
5. Visual/audio feedback.
6. Balance.
7. Analytics.
8. Closed test.

## Explicit Non-Goals

Do not do in this iteration:

- live PvP
- clans, leagues or ranked systems
- complex monetization
- premium currency
- loot boxes
- battle pass
- energy system
- 30+ weapons
- large story campaign
- full live-ops
- destructible terrain unless it becomes a small isolated prototype outside the release path
