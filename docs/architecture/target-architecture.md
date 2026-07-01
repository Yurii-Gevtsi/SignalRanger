# Target Architecture

## Product Direction

Build an original mobile 2D tactical artillery game in Unity. The first version should feel lighter and friendlier than older genre examples:

- short matches;
- small teams;
- few early weapons;
- clean touch controls;
- guided first session;
- readable UI;
- progression that starts simple and expands over time.

The long-term structure should support more weapons, character upgrades, events, cosmetics, monetization, live tuning, and optional online services without forcing those concerns into core gameplay.

The product direction from player-review research is: preserve a clean battle-first experience, add long-term progression gradually, and treat fairness/trust as a core feature. The target is not a huge meta-game at launch; it is a satisfying tactical duel that can grow without becoming noisy.

## Proposed Unity Folder Structure

Recommended project-owned root:

```text
Assets/_Game/
  Runtime/
    Core/
    App/
    Gameplay/
    Progression/
    Economy/
    Inventory/
    Rewards/
    Tutorial/
    UI/
    Audio/
    Data/
    Infrastructure/
  Content/
    Art/
    Audio/
    Prefabs/
    ScriptableObjects/
    Localization/
  Scenes/
    Boot/
    Menu/
    Battle/
  Tests/
    EditMode/
    PlayMode/
  Editor/
```

This is our own structure and should not mirror the unpacked examples.

## Layers

`Core`
: Shared interfaces, result types, events, time abstractions, lightweight service registration, and domain models that do not depend on Unity scene objects when possible.

`App`
: Bootstrap, service composition, application flow, scene loading, platform startup, and lifecycle handling.

`Gameplay`
: Battle rules, turns, unit control, weapon execution, projectiles, terrain interaction, win/loss conditions, and battle camera behavior.

`Progression`
: Player journey, level unlocks, chapters, mission goals, difficulty ramps, and feature gates.

`Economy`
: Soft currency, reward grants, sinks, pricing models, and transaction validation for local gameplay.

`Inventory`
: Owned weapons, consumables, cosmetics, upgrades, and loadouts.

`Rewards`
: Match rewards, daily rewards, first-time bonuses, event rewards, and reward presentation.

`Tutorial`
: Step-based onboarding, contextual prompts, tutorial battle constraints, and feature unlock explanations.

`UI`
: Screens, panels, navigation, presenters/view models, reusable widgets, and HUD controllers.

`Audio`
: Music, SFX, mixer routing, user volume settings, and battle audio cues.

`Data`
: Save data, authored configs, local defaults, migrations, and repositories.

`Infrastructure`
: Concrete adapters for ads, IAP, analytics, remote config, notifications, platform APIs, and backend clients.

## Core Services

Initial service set:

- `SceneLoadingService`
- `GameSessionService`
- `SaveService`
- `ConfigService`
- `ProgressionService`
- `EconomyService`
- `InventoryService`
- `RewardService`
- `TutorialService`
- `UiNavigationService`
- `AudioService`
- `SettingsService`
- `AnalyticsService`
- `AdsService`
- `PurchaseService`
- `RemoteConfigService`
- `NotificationService`

SDK-facing services should expose project-owned interfaces. UI and gameplay must call our interfaces, not SDK APIs directly.

## MVP Scope

Recommended MVP:

- Boot scene.
- Main menu.
- Battle scene.
- One tutorial battle.
- One simple progression path with 8-12 handcrafted missions.
- 4-6 original weapons.
- 2-3 original unit archetypes.
- Local save.
- Simple soft currency.
- Basic inventory/loadout.
- Settings for audio and language-ready text keys.
- Analytics interface with a local/no-op implementation.
- Ads/IAP/remote config interfaces with no-op implementations.

Review-informed launch direction after the prototype is proven:

- 12-16 original weapons;
- 4-6 original character archetypes;
- 4-6 arenas;
- one soft currency;
- account level, character growth, and weapon mastery as the only early progression layers;
- early access to PvP or a safe PvP-like equivalent;
- normalized competitive power if ranked PvP exists.

Defer until the core loop is fun:

- real ads SDK;
- real billing SDK;
- push notifications;
- backend login;
- clans/social features;
- large event systems;
- complex crafting;
- many currencies;
- heavy upgrade trees.

Avoid permanently:

- paid/VIP combat advantage in ranked play;
- aggressive forced ads after every match;
- progression that creates large new-player versus old-player power gaps;
- account, save, or purchase flows without recovery/restore plans.

## Data Strategy

Use ScriptableObjects for authored content in early development:

- weapon definitions;
- unit archetypes;
- mission definitions;
- reward tables;
- progression chapters;
- tutorial steps;
- UI theme references.

Use JSON or a versioned serializable model for local saves. Save data must store player progress and ownership, not direct references to proprietary sample content.

## Dependency Direction

Preferred dependency direction:

```text
UI -> App/Core interfaces
Gameplay -> Core/Data interfaces
Progression/Economy/Inventory -> Core/Data interfaces
Infrastructure -> Core interfaces
App -> composes concrete implementations
```

Avoid direct dependencies from gameplay into ads, purchases, Firebase, Android plugins, or backend code.

## Architecture Decisions To Revisit

- Whether to use Addressables after content volume grows.
- Whether to add a dependency injection framework or keep a lightweight project-owned composition root.
- Whether battle terrain uses Unity 2D colliders, tilemaps, generated meshes, or a custom destructible mask.
- Whether multiplayer is local/offline first, asynchronous later, or real-time later.
