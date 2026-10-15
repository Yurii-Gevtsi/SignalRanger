# Weapon 35 — carrier (ufo_carrier) validation

## Result

**Implemented** (mechanics, skins; presentation judged only by tests). Card 35, `weapon.ufo-carrier`, TTH limits 0/0/0/4 (Radius only), one flight per battle. Before this closure the carrier was an ordinary exploding controlled projectile.

- It is a controlled copter built on the same session as the bomber: no aim, spends the charge and the action, steered up/down/left/right at 4 u/s, stopped by ground, ceiling, walls and the map border, 10 s of flight plus 1 s per Radius purchase (14 at four).
- Cargo: the fire button grabs the nearest movable item the copter physically touches (contact distance 0.62): mines (own or hostile) and guardian drones, and laser-fence nodes. Droids, titanium barriers, crawling spiders and mines that have already been triggered are not cargo; with nothing in reach the press is refused. A second press lets the item go: it falls straight to the surface (no bounce, no roll). The item keeps its owner and its armed state; while it is held it cannot go off (a hostile mine carried over our droid does not trigger) and it goes off normally after the release. A node carried away moves its link with it, and a link longer than the maximum length simply stops existing (the link resolver is recomputed every tick).
- End of the window: the copter lets go of the cargo, falls and blasts like a mine (the mine's damage and `R0`, hurting any droid at the point); the turn passes after that. A hostile grounder destroys the copter at the magnet (the magnet loses 1 HP, no blast; the cargo is released); the falling copter, an explosive carrier, blasts at the magnet.
- Presentation: the animated rotor frames `fpv_0…fpv_4` (the card's art), mirrored with the direction of travel; the held item is drawn over the copter's body (higher sorting order, hung 0.2 above the centre) and goes back to the ground layer on release. The fire button stays usable although the one charge is spent.

## Evidence

- EditMode `BattleSimulationTests`: `Captures/weapon12/w35-edit-3.xml`, 285/285 PASS. New: grab an enemy mine, it follows the copter, stays inert over our droid, falls on the second press and then goes off on our droid; a droid is never cargo and contact is needed; window end with cargo (item released, copter falls and blasts with the mine damage, turn passes); a laser-fence node is carried and dropped; registry 0/0/0/4 + mine damage and radius + 14 s + art (ufo_carrier, fpv_0-4) + paid Radius steps only.
- PlayMode: `Captures/weapon12/w35-play.xml`, 46/46 PASS (real scene: the copter is animated, grabs a mine, the mine is drawn over the copter and travels with it, and returns to the ground layer on release).
- Full EditMode: `Captures/weapon12/w35-edit-full.xml`, 828 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; the copter size and the cargo lift were not judged on screen (the bomber's art uses the same canvas and was).

## Open items

- Medkits and resources are listed as cargo by the card, but the battle has no such objects yet, so they cannot be carried; they join the cargo rules when they exist.
- Whether a crawling spider and a dynamite charge may be carried is not in the card (both are excluded).
- Speed 4 u/s, contact distance 0.62, lift 0.2 and the +1 s Radius step are prototype values; the capture needs a press with the copter touching the item (no auto-grab).
- A node's dropped position snaps to the surface directly below it.
