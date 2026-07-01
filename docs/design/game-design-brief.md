# Game Design Brief

## Working Concept

An original mobile 2D turn-based tactical artillery game about small teams of expressive fighters battling on compact destructible arenas. The game should be easy to understand in the first minute, satisfying to replay in short sessions, and expandable through new weapons, arenas, events, and progression layers.

Working genre description:

```text
Short tactical artillery duels with fair physics, readable terrain, light progression, and gradual content growth.
```

This is an original project. Do not copy names, assets, balance, layouts, configs, weapons, maps, characters, or proprietary systems from existing games.

## Design North Star

The player should feel:

```text
I understood the situation.
I made a clever shot.
The result was fair.
I earned progress.
I want one more match.
```

## Product Position

The game should sit between two extremes:

- cleaner and easier to enter than heavy live-service tactical games;
- deeper over time than a disposable one-session arcade game.

The first public version should feel like a polished battle game with a light campaign and progression, not like a crowded meta-system.

## Target Player Experience

First session:

- the player starts quickly;
- the first battle teaches aiming and power;
- the player fires within the first minute;
- the result is readable even if they miss;
- the battle ends quickly;
- the player receives a small reward;
- the next mission or match is obvious.

Longer-term:

- unlock new weapons gradually;
- discover different character archetypes;
- learn arena hazards and terrain tricks;
- improve weapon mastery;
- return for events, new arenas, and short challenges.

## Core Loop

```text
Choose mission or match
-> enter battle
-> move/aim/fire during turn
-> react to terrain and enemy turn
-> win or lose
-> receive rewards and mastery progress
-> unlock or improve options
-> play one more short match
```

## Battle Rules For MVP

Initial battle rules:

- 1 team versus 1 team;
- 1-2 units per side during early prototypes;
- turn-based actions;
- one active unit per turn;
- limited move range;
- one shot per turn;
- simple health;
- victory when all enemy units are defeated;
- battle length target: 2-5 minutes.

Add later:

- wind;
- water or fall deaths;
- multi-unit teams;
- arena hazards;
- alternate objectives;
- ranked rules;
- event mutators.

## Controls

Control quality is a top priority.

Prototype control model:

- drag or hold to aim;
- visible aim direction;
- visible power indicator;
- explicit fire confirmation;
- no accidental release-to-fire until playtests prove it feels good;
- cancel aim button or gesture;
- clear trajectory preview for tutorial and early weapons.

The shot should feel fair before any progression or economy is added.

## First Prototype Scope

The first Unity prototype should prove only the battle feel:

- one battle scene;
- two placeholder units;
- turn switching;
- aim and power control;
- one projectile weapon;
- projectile arc;
- collision with unit or terrain;
- simple radial damage;
- health display;
- win/loss state;
- restart button.

No shop, no account level, no ads, no IAP, no login, no live services.

## MVP Scope

After the first prototype feels good, MVP should include:

- boot flow;
- main menu;
- battle scene;
- short tutorial;
- 8-12 handcrafted campaign missions;
- 4-6 original weapons;
- 2-3 original character archetypes;
- 2-3 arenas;
- local save;
- basic loadout;
- one soft currency;
- simple reward screen;
- settings for audio;
- no-op interfaces for analytics, ads, IAP, remote config, and notifications.

## First Public Version Scope

The first public version can grow to:

- 12-16 original weapons;
- 4-6 original character archetypes;
- 4-6 arenas;
- offline AI battles;
- campaign progression;
- early casual PvP or safe PvP-like equivalent if feasible;
- account level;
- character growth;
- weapon mastery;
- one soft currency;
- optional premium currency only for cosmetics, pass, or convenience later.

## Original Character Archetypes

Start with archetypes defined by tactical role, not copied fantasy/species/classes.

Suggested early archetypes:

- `Striker`: balanced movement, reliable damage, easy for beginners.
- `Tinker`: utility tools, terrain placement, indirect tactics.
- `Bulwark`: higher health, defensive tools, slower movement.
- `Scout`: mobility and repositioning, lower health.
- `Medic`: light support and recovery, limited damage.
- `Saboteur`: status effects, traps, disruption.

MVP should start with 2-3 of these. More archetypes only after the core rules are stable.

## Original Weapon Roles

Start with role categories before inventing final names, visuals, and stats.

MVP weapon set:

- direct projectile;
- arcing explosive;
- short-range burst;
- terrain placement tool;
- defensive shield or cover tool;
- movement/repositioning tool.

First public version can expand with:

- delayed explosive;
- bouncing projectile;
- area denial;
- low-damage multi-hit;
- healing/support shot;
- knockback tool;
- precision high-risk shot;
- terrain cutter;
- status effect tool;
- event-only experimental weapon.

Rules:

- every weapon needs a clear tactical purpose;
- avoid weapons that decide the match before counterplay exists;
- keep animations short;
- allow problem weapons to be disabled in special modes or private matches later;
- tune from playtests, not from reference games.

## Arena Direction

Arena goals:

- compact enough for short matches;
- readable on mobile;
- varied terrain shapes;
- safe spawn positions;
- no first-turn cheap deaths;
- clear hazards only after the player understands basic movement and shooting.

Early arena themes should be original and simple. For prototyping, use placeholder geometry only.

## Progression Direction

Use three early progression layers:

- account level;
- character growth;
- weapon mastery.

Keep these legible:

- account level unlocks broad access;
- character growth gives small PvE-oriented improvements and cosmetic identity;
- weapon mastery teaches commitment and unlocks variants/cosmetics before power creep.

Ranked PvP, if added, should normalize combat power so older accounts and paying players do not dominate new players through stats.

## Economy Direction

Early economy:

- one soft currency;
- rewards from missions and battles;
- currency used for unlocks, basic upgrades, or cosmetics depending on balance;
- no crafting resource web;
- no VIP combat power;
- no many-currency system.

Premium currency, if added later:

- cosmetics;
- season pass;
- convenience;
- non-ranked expression;
- no ranked combat dominance.

## PvP Direction

PvP should appear early enough that players feel real tactical tension, but it must be protected.

Principles:

- casual PvP before ranked PvP;
- bot-assisted PvP-like mode is acceptable while backend is absent;
- normalized stats in ranked;
- matchmaking by skill and arsenal band;
- reconnect plan before serious ranked launch;
- server validation and anti-cheat before competitive stakes become meaningful.

## Monetization Direction

Monetization should not punish playing.

Allowed later:

- optional rewarded ads;
- cosmetics;
- battle pass or event pass;
- remove-ads purchase;
- convenience purchases that do not dominate ranked combat.

Avoid:

- forced ad after every match;
- two forced ads in a row;
- paid/VIP combat advantage;
- purchase flows without restore and account recovery planning.

## Live-Ops Direction

Prefer temporary freshness over permanent system bloat:

- seasonal arenas;
- weekly boss challenges;
- temporary weapon pools;
- mutator events;
- short PvE challenge chains;
- cosmetic rewards;
- limited-time casual PvP rules.

Avoid adding a new permanent economy layer with every update.

## What We Are Not Building First

Not in first prototype:

- campaign;
- shop;
- inventory;
- ads;
- IAP;
- login;
- cloud save;
- multiplayer backend;
- complex destructible terrain;
- many weapons.

Not in MVP:

- clans;
- real-money shop;
- push notifications;
- complex crafting;
- many currencies;
- ranked PvP;
- seasonal battle pass;
- large live-ops system;
- advanced anti-cheat.

Avoid permanently:

- copied content from existing games;
- paid combat dominance;
- overloaded first-session UI;
- unfair spawn logic;
- hidden formulas that make losses feel fake;
- progression that buries the fun.

## Immediate Implementation Target

Build `Milestone 1: Fair Shot Prototype`.

Acceptance criteria:

- the project opens in Unity;
- a battle scene can be played in editor;
- player can aim deliberately;
- player can fire one projectile;
- projectile arc is readable;
- hit detection works;
- damage updates health;
- turns alternate;
- match ends with win/loss;
- restart works;
- no external SDK is required.

