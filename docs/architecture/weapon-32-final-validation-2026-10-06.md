# Weapon 32 — kamikaze spider validation

## Result

**Implemented** (base and element choice); the role of the old "1+3" duration purchases needs the owner. Card 32, `weapon.spider-mine`, TTH limits 4/0/2/4, prototype stock 2, animated body `spider_mine_frame_00…03`, purchased body `spider_mine_5A_frame_00…03`.

Two defects of the earlier base were found by real checks and fixed:

- **The spider blew up on its first tick.** The crawl used the "falling object landing" helper, which only succeeds for objects at least 6 cm above the ground; a crawler resting on the ground was therefore treated as having left the map. The crawl now follows the ground: it climbs ledges up to 0.45, steps down up to 1.2, and blows up where it cannot go on (a taller wall, a deeper drop or the map edge).
- **It could never attach to a standing droid.** The attach test measured the distance between centres (0.34), but a standing droid's centre is half a body above the crawler on the ground. It now attaches when it is level horizontally (0.34) and the droid's body spans its height.

Behaviour now: set up beside the droid without aim and without spending the action; crawls ahead at 1.4 u/s; attaches to the first enemy it reaches and blasts after 1.5 s; with nobody on the way it crawls for five seconds and blasts anyway; the blast does not hurt allies.

Element choice (`element_choice`): the first feature now offers three options at rank 5 — `spider-freeze`, `spider-burn`, `spider-poison` — through the existing choice modal (it was one option before, which could not express a choice). Each runs the shared two-turn status cycle of the matching grenade: the blast deals only its direct damage, the status ticks 20% of that direct damage at the end of each of the victim's two turns, a repeat hit refreshes it and boosts only its direct damage. All three options share the evolved skin `spider_mine_5A`. The element is reachable through the paid service, saved, and restored from JSON; the obsolete 1-turn description text was replaced by three new descriptions in all 13 dictionaries.

## Evidence

- EditMode `BattleSimulationTests`: `Captures/weapon12/w32-edit-4.xml` (before the localization change), 267/267 PASS. New: the base crawl/attach/1.5 s fuse/damage/ally immunity, the five-second search, each element's status and its two-tick total, registry 4/0/2/4 with three options, art, paid purchase of each option with JSON and icon.
- PlayMode: `Captures/weapon12/w32-play.xml`, 43/43 PASS (new: the real scene animates the base or the purchased body and the crawler moves). Full EditMode: `Captures/weapon12/w32-edit-full.xml`, 809 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test.

## Needs the owner

- The old 1+3 purchases: the first feature still costs four special steps (the registry keeps that count), but only the first one has an effect (the element). What do the remaining three steps do — or should the feature cost a single step?
- The card says the spider's status duration is the shared two turns; confirm there is no separate duration purchase.
- Prototype numbers: crawl speed 1.4, search time 5 s, fuse 1.5 s, climb 0.45, drop 1.2, damage 45.
- The element is chosen at the purchase (one element per weapon); say if it must instead be switchable before each use.
