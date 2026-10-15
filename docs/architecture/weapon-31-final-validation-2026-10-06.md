# Weapon 31 — signal rocket (sky hook) validation

## Result

**Implemented** (mechanics and skin). Card 31, `weapon.sky-hook`, TTH limits 4/0/0/0 (Core only), one rocket per battle.

- Launch: no aim; the rocket is set up beside the droid (on the side it faces) and flies straight up at `2 × V0` (24 u/s, the library speed is now `2 × SharedProjectileSpeed`). It explodes slightly below the top of the arena: the top of the playable grid minus 0.75 (not the top of the screen), so the blast stays on the visible scene. The blast uses the shared `R0` (the prototype library radius 0.9 was replaced). The shot ends the turn like any attack.
- Ground: the rocket erases ground on its way without any penetration budget (a five-unit slab did not stop it); the channel stays open.
- Carrying: the first hostile droid whose body the rocket touches (at launch or on the vertical path; body contact, not the `N0` distance) is lifted at the rocket's own speed. Allies are never picked up; only one droid is carried.
- A carried droid that meets ground or a ceiling comes off and falls; the rocket goes on through the obstacle. At the final point the droid is thrown sideways with the electric harpoon's final-throw impulse (60°, about two rope lengths of range — one shared helper now serves both weapons); the side is the droid's side of the rocket, on symmetry the owner's facing at launch. The droid is also inside the blast, so it takes the blast damage.
- Hostile grounder: a rocket that passes inside the magnet's range is taken over by the magnet (a gradual pull cannot reach something moving at 24 u/s), explodes at the magnet and the carried droid is just let go and falls (the card's proposal).
- Skin: `sky_hook` body (art points up, so the flight rotation has a −90° art offset, also in the per-frame sync) with the shared `missle_tail_clean` exhaust attached under it, scale 0.049. The mechanic is no longer gated off.

## Evidence

- EditMode `BattleSimulationTests`: `Captures/weapon12/w31-edit-3.xml`, 261/261 PASS. New: set up beside the droid, 24 u/s straight up, blast at 7.25 with no drift, turn passes; five-unit slab erased; enemy touching at launch carried at 24 u/s and thrown at 60° with the harpoon force, hurt and lands; ally never carried and the enemy on the path is; ceiling lets the carried droid go (never above it) while the rocket goes through; hostile magnet; registry 4/0/0/0 + one per battle + speed + R0 + art incl. tail + paid Core steps and limits + JSON.
- PlayMode: `Captures/weapon12/w31-play.xml`, 41/41 PASS (real scene: body skin, attached tail, nose points up also after the per-frame sync, scale, blast). A screenshot of the real scene with the rocket in flight was inspected: `Captures/weapon12/w31-skyhook-shot.png` (upright rocket with the exhaust below it).
- Full EditMode: `Captures/weapon12/w31-edit-full.xml`, 803 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence plus one editor screenshot; not a phone test.

## Open items

- Blast damage is the prototype 46 (global balance pending); the blast radius is `R0` by the shared rule, no card-specific value.
- Blast on a rocket that carries nobody is an ordinary explosion at the top of the arena (no one is expected there).
- Core purchases scale damage as for other weapons; the tail art scale was judged on one screenshot only.
- Fast carriers are caught by the grounder only by contact or, for this rocket, by the heading take-over; the older shots keep the gradual pull.
