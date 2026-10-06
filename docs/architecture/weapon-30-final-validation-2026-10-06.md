# Weapon 30 — rocket fuel (boost) validation

## Result

**Implemented** (mechanics). Card 30, `weapon.boost`, TTH limits 0/0/4/4 (Loadout, Radius), prototype stock 2 charges.

- Launch: no aim; it works only from a stable standing droid and spends one charge. The droid rises vertically (no arc, no run-up) by the height of the apex of an ordinary jump (mean of the archetype's front and back jump heights) and then hangs: gravity does not act while the fuel burns.
- Window: 5 seconds, +1 second per Radius purchase (9 at four) — `WeaponUpgradeChoiceResolver.ResolveBoostSeconds`, also shown as the Radius value of the workshop card.
- Steering: up / down / left / right with the same on-screen buttons as the rope (the HUD shows the rope controls while the fuel burns), 3.2 u/s. The rise is automatic and cannot be steered; steering opens after the apex.
- Collisions: terrain, ceiling, walls and the map border block only the direction that would penetrate; other droids block the body; the timer keeps running while blocked.
- End: a second press of the fire button, the end of the window, or the end of the turn cuts the fuel; the droid then falls under normal physics and lands (or falls out of the arena like any other fall). The fuel does not spend the turn action — other weapons may be used afterwards, as after the rope. A second press does not spend another charge.
- A frozen or disoriented droid keeps the automatic rise and hang but cannot steer (the card's "movement ban keeps the vertical launch and hover" rule; the movement-lock statuses of the cannons will use the same gate when they exist).
- Visual: the existing droid jet trail is shown for the whole hover (the card asks for an "effect of the fuel" on the droid, no new flying object).
- The mechanic is no longer gated off; the old "unimplemented utility" test now guards `turn_transfer` and `sky_hook` instead.

## Evidence

- EditMode `BattleSimulationTests`: `Captures/weapon12/w30-edit-3.xml`, 254/254 PASS. New: rise to the apex of an ordinary jump (measured with a real jump in the same harness) and hang motionless, four-direction steering, another droid blocks, ceiling blocks the rise and steering while the timer runs, 5 s / 9 s window, second press and landing without spending the action or a charge, charges run out and the turn ending cuts the fuel, registry 0/0/4/4 + seconds + charges + art + paid Loadout/Radius steps and limits + JSON.
- PlayMode: `Captures/weapon12/w30-play.xml`, 40/40 PASS (new: in the real scene the fuel lifts the droid, the shared up button climbs, the unit view refreshes with the jet trail, a second press drops it and it lands).
- Full EditMode: `Captures/weapon12/w30-edit-full.xml`, 796 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test.

## Open items

- Rise and steering speeds (4.5 and 3.2 u/s) are prototype values.
- Knockback and damage while hovering do not interrupt the fuel (the card says nothing); the droid is not pushed by blasts while it burns.
- There is no keyboard binding for vertical steering (the on-screen buttons only); the movement-lock gate is checked through frozen/disoriented only.
- The visual of the fuel is the shared jet trail; a dedicated flame art was not requested.
