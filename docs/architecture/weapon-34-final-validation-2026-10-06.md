# Weapon 34 — bomber (copter) validation

## Result

**Implemented** (mechanics, skins). Card 34, `weapon.bomber`, TTH limits 4/0/0/4 (Core, Radius), one flight per battle.

Before this closure the bomber was routed through the generic controlled-drone path (an ordinary exploding projectile). It is now a real copter:

- Launch: no aim; the copter lifts off above the droid, spends the charge and the action (the flight is the attack). The shared direction buttons (the HUD shows them for a controlled projectile) steer it up/down/left/right at 4 u/s.
- Window: 10 s, +1 s per Radius purchase (14 at four; the "+1 s" step is a prototype choice — the card only says Radius lengthens the flight). Radius no longer scales the blast. When the window ends the copter vanishes and the turn passes once the bombs have landed.
- Collisions: ground, ceiling, walls and the map border stop the copter on the blocked axis; it never explodes.
- Bombs: every press of the fire button drops one bomb straight down from the copter, with no delay between presses (four presses in a row produce four bombs); there are four per flight, a fifth press is refused and the copter stays up until the window ends. A bomb blasts at once on the ground or on a hostile droid (it falls past its own side), with the ordinary mine blast: the mine's damage (54, Core steps scale it) and `R0`, digging a crater like a mine.
- Hostile grounder: the copter has no explosive payload, so it is destroyed at the magnet (the magnet loses 1 HP); every bomb is a physical explosive carrier and blasts at the magnet, destroying it (54 ≥ 45).
- Presentation: the copter is the `bomber` art (about 0.9 units wide, mirrored with its travel direction, never rotated); the bomb is an enlarged `blaster_shot` standing vertically along the fall (about 0.35 × 0.11). The fire button stays usable during the flight although the single charge is spent (the same "keep control" rule as the teleport sphere's second press). A screenshot of the real scene (`Captures/weapon12/w34-bomber-shot.png`) shows the copter and a falling bomb.

## Evidence

- EditMode `BattleSimulationTests`: `Captures/weapon12/w34-edit-2.xml`, 280/280 PASS. New: launch/charge/action and four-direction steering at 4 u/s, ground/ceiling/border stops without exploding, 10 s and 14 s windows and turn passage, four drops in a row and the refused fifth, mine-like blast and crater on a hostile droid, an ally passed, magnet destroys the copter and catches a bomb, registry 4/0/0/4 + mine damage and radius + Radius is flight time (no blast scaling) + art + paid Core/Radius steps.
- PlayMode: `Captures/weapon12/w34-play.xml`, 45/45 PASS (real scene: copter skin, scale and mirroring, vertical bomb skin that stays vertical, blast, the copter keeps flying).
- Full EditMode: `Captures/weapon12/w34-edit-full.xml`, 823 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence plus one editor screenshot; not a phone test.

## Open items

- Copter speed 4 u/s, the +1 s Radius step and the bomb's fall (ordinary gravity, no initial speed) are prototype values.
- There is no on-screen counter of the bombs left or of the flight time.
- The ordinary mine damage is the prototype 54 (global balance pending).
- Core steps scale the bomb damage; the card does not say what Core means for this weapon.
