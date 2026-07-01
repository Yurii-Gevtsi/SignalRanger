# Player Review Direction

This document summarizes product direction from the provided review analysis for Wormix and Warlings 2. It is not a source for copying content, names, balance, UI, economy, or proprietary systems. It is a high-level interpretation of player needs, frustrations, and opportunities for an original game.

## Strategic Formula

The best direction for this project:

```text
Warlings 2-like clarity of battle core
+ Wormix-like feeling of long-term progression
- Wormix-like economy/system overload
- unfair PvP, aggressive ads, and account trust problems
```

In practical terms: build a clean, readable, satisfying battle loop first, then add progression in layers. Do not begin with a heavy meta-game.

## What Players Seem To Love

Across the genre signals, players respond strongly to:

- short turn-based tactical duels;
- funny or expressive characters;
- varied weapons with clear tactical roles;
- readable projectile physics;
- destructible or interactive terrain;
- PvE and PvP options;
- unlocks that create anticipation;
- new maps, weapons, seasonal arenas, and event reasons to return.

The fantasy works: clever shots, surprising weapons, compact battles, and the feeling that victory came from skillful decisions.

## What Breaks Trust

The most dangerous issues are not lack of content. They are trust failures:

- uncomfortable aiming or accidental shots;
- unfair matchmaking;
- large power gaps between new and old players;
- VIP or paid advantages in competitive combat;
- exploits, cheating, or unclear server authority;
- lost purchases or broken account migration;
- unstable reconnects;
- ads that feel like punishment after every match;
- long animations or weapons that create cheap losses;
- bad spawn positions that decide the match before the player acts.

For this genre, trust is a primary feature. Players need to believe that controls, physics, match results, saves, purchases, and ranking are fair.

## Lessons From Wormix

Useful ideas:

- long-term content progression can retain players;
- multiple character archetypes can deepen team identity;
- PvE and PvP can coexist;
- events, bosses, and new content can keep the game alive.

Avoid:

- too many currencies;
- too many crafting resources;
- too many parameters;
- too many early gates;
- VIP-driven combat advantage;
- overwhelming new players before they reach the real fun;
- piling permanent systems on top of permanent systems.

Wormix is valuable mainly as a warning about system weight. Keep the joy of tactical battles and progression, not the complexity mass.

## Lessons From Warlings 2

Useful ideas:

- a compact, readable battle-first product is easier to understand;
- a moderate weapon roster can feel rich without becoming exhausting;
- offline AI, local hotseat, and online PvP serve different player moods;
- destructible terrain and short sessions are strong genre anchors;
- player requests for more maps/weapons usually mean the core loop is working.

Avoid:

- unfair online matchmaking;
- aggressive ad cadence;
- obvious weapon imbalance;
- spawn positions that create unfair early deaths;
- weapons or animations that waste too much match time;
- leaving anti-cheat and competitive integrity too late.

Warlings 2 is closer to the product shape we want: battle first, meta second.

## Product Pillars

### 1. The Shot Must Feel Fair

The first priority is aiming, power control, projectile readability, collision clarity, and feedback. Every miss should feel like the player's mistake or a readable tactical risk, not a UI failure.

Design implications:

- deliberate fire confirmation;
- visible trajectory preview or partial assist early on;
- clear wind/force indicators if wind exists;
- generous touch targets;
- no accidental release-to-fire during early tests;
- instant feedback on damage, knockback, and terrain effects.

### 2. PvP Must Feel Safe Early

Players should taste human tactical tension early, but competitive power should be normalized.

Design implications:

- early casual PvP or bot-assisted PvP equivalent;
- ranked PvP only after basics are learned;
- normalized stats in ranked modes;
- matchmaking by skill/rating and available arsenal band;
- no paid combat advantage in ranked;
- server-authoritative results when online features arrive.

### 3. Progression Must Be Legible

Progression should motivate, not bury the player.

Recommended initial layers:

- account level;
- character growth;
- weapon mastery.

Avoid early:

- many currencies;
- many crafting resources;
- nested upgrade trees;
- VIP battle power;
- dozens of unlock conditions.

### 4. Content Should Expand Through Events, Not Permanent Noise

Long-term freshness should come from time-limited content and new battle contexts rather than constant permanent economy expansion.

Good candidates:

- seasonal arenas;
- weekly boss challenges;
- temporary weapon pools;
- mutator modes;
- short PvE event chains;
- cosmetic event rewards.

### 5. Monetization Must Not Punish Play

Ads and purchases should not make each match feel taxed.

Guidelines:

- no forced ad after every battle;
- rewarded ads should be optional and clearly framed;
- remove-ads purchase can exist later;
- paid content should focus on cosmetics, convenience, passes, and non-ranked expression;
- ranked combat must remain fair.

## Recommended Launch Scope

Target a small but complete first public version:

- 12-16 original weapons;
- 4-6 original character archetypes;
- 4-6 arenas;
- short campaign/tutorial path;
- offline AI battles;
- local or casual PvP path if feasible;
- simple account level;
- simple character and weapon progression;
- one soft currency;
- optional premium currency only for cosmetics/pass/convenience later.

For the first prototype, this is still too much. The prototype should prove only:

- aiming;
- projectile feel;
- turn flow;
- damage;
- terrain interaction;
- win/loss;
- one readable HUD.

## Priority Risks To Design Around

1. Bad aiming feel.
2. Cheap losses from spawn, edge falls, or overpowered weapons.
3. Matchmaking unfairness.
4. Power gap from progression or monetization.
5. Account/save/purchase trust failures.
6. Too much meta complexity before the player loves the core.
7. Ads damaging the rhythm.
8. Content updates adding permanent complexity instead of fresh play.

## Design North Star

The player should feel:

```text
I understood the situation.
I made a clever shot.
The result was fair.
I earned progress.
I want one more match.
```

