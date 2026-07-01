# Systems Overview

## Core Game Loop

1. Player opens the game.
2. Boot scene initializes services and loads save data.
3. Main menu shows the next recommended mission.
4. Player enters a short tactical battle.
5. Battle resolves with clear win/loss feedback.
6. Player receives rewards and progression.
7. New weapons, missions, or features unlock gradually.

The first session should avoid overloaded menus. The player should reach a playable battle quickly.

## Battle System

The battle system should be small and readable first:

- turn-based teams;
- limited turn timer;
- simple movement and aiming;
- wind or environmental modifiers only after tutorial basics;
- weapon selection with a small starting set;
- clear hit feedback;
- deterministic rules where practical for testing.

Suggested submodules:

- turn controller;
- battle state model;
- unit controller;
- weapon executor;
- projectile simulation;
- damage resolver;
- terrain interaction;
- battle result evaluator;
- battle camera;
- battle HUD presenter.

## Weapons

Start with original weapons that teach different concepts without overwhelming the player:

- direct projectile;
- arcing explosive;
- short-range burst;
- utility placement;
- defensive tool;
- movement or repositioning tool.

Each weapon should have:

- stable ID;
- display text key;
- icon reference;
- category;
- aim mode;
- ammo rules;
- cooldown or availability rules;
- damage/effect model;
- tutorial hint key.

Do not reuse weapon names, icons, stats, or upgrade paths from the APK/XAPK examples.

## Progression

First version:

- linear mission path;
- gradual mechanic unlocks;
- small reward after each battle;
- optional replay goals later.

Future expansion:

- chapters;
- challenge missions;
- events;
- difficulty tiers;
- cosmetic unlocks;
- seasonal reward tracks.

## Economy

Keep the first economy intentionally simple:

- one soft currency;
- clear reward sources;
- few early sinks;
- no premium currency until monetization design is explicit.

Economy data should be authored in our own configs and tuned from playtests, not inferred from examples.

## Inventory And Loadout

Initial version:

- owned weapons;
- simple loadout slots;
- unlock-only progression;
- no complex crafting in MVP.

Later:

- upgrades;
- skins;
- consumables;
- event items;
- rarity only if it adds clarity and retention without clutter.

## Tutorial

Tutorial should be step-based and short:

- first aim;
- first shot;
- moving;
- weapon swap;
- objective completion;
- reward screen;
- returning to the next mission.

The tutorial system should reference abstract triggers and UI targets, not hard-coded screen logic.

## UI Navigation

MVP screens:

- boot/loading;
- main menu;
- mission select or next mission panel;
- loadout;
- battle HUD;
- pause/settings;
- result screen;
- simple shop placeholder only when economy needs it.

Avoid copying older crowded mobile HUDs. Prefer fewer visible choices, larger touch targets, and progressive disclosure.

## Save System

Save data should include:

- schema version;
- player profile;
- completed missions;
- unlocked content IDs;
- owned inventory IDs;
- currency balances;
- settings;
- tutorial progress;
- last reward timestamps if daily rewards are enabled.

Use migration steps when schema changes.

## SDK And Platform Services

Use interfaces from day one, with no-op/local implementations first:

- analytics;
- ads;
- IAP;
- remote config;
- notifications;
- backend API.

Gameplay and UI call the interfaces. Real SDK adapters can be added later under `Infrastructure`.

## Recommended First Milestone

Milestone 1 should prove the feel:

- Boot to battle.
- One controllable unit per team.
- Turn switching.
- One projectile weapon.
- Health and win/loss.
- Basic mobile-friendly HUD.
- Local restart.

Only after this feels good should we add progression, economy, shop, ads, IAP, or notifications.

