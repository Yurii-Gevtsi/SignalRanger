# Weapon 36 — self-destruct protocol validation

## Result

**Implemented** (mechanics; marker icon). Card 36, `weapon.self-destruct`, TTH limits 4/0/0/4 (Core, Radius), one use per battle.

- Marking: no aim, no projectile. It spends the charge but neither the action nor the turn: the droid keeps moving and may use another weapon; nothing happens yet.
- Resolution: when the marked droid's turn ends (the natural timer zero, or any other end of that turn — the card only describes the timer case) the blast and the owner's death are resolved together, and only then is the result decided: the last two droids dying at the same moment is a draw (checked by a test), not a win for either side.
- The blast is centred on the owner with the weapon's damage (Core steps scale it; prototype 72) and a radius of 1.5 × the shared `R0` (+10% of the base per Radius purchase). It hurts every droid in the radius, allies included, and digs a crater. The owner is sacrificed even if the blast leaves them standing (the death is forced with no protection applying, so never 1 HP left).
- A marked droid that dies before its turn ends explodes nothing.
- Presentation: the existing `self_destruct` art is shown as a marker (0.6 world units) above the marked droid until the blast. No separate blast art was requested; the ordinary impact flash is used.
- The mechanic is no longer gated off.

## Evidence

- EditMode Gameplay: `Captures/weapon12/w36-edit-2.xml` — the 6 new simulation tests and the marker test PASS (marking keeps the action/turn/movement; blast, owner death, ally and craters; 1.5 R0 and a far enemy safe with the opponent winning; the draw decided after both deaths with one `BattleFinished`; a dead owner leaves no blast; registry 4/0/0/4 + one per battle + Radius percent + art + paid steps). The other failures of that run are the known baseline ones.
- PlayMode: `Captures/weapon12/w36-play.xml`, 47/47 PASS (real scene: the marking keeps the turn and puts exactly one marker on the droid).
- Full EditMode: `Captures/weapon12/w36-edit-full.xml`, 835 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test.

## Open items

- Damage 72 is the prototype for 50% of `H_cap` (global balance pending).
- If the owner ends their turn early by attacking, the protocol also fires at that turn end (the card describes only the timer zero) — the owner to confirm.
- The marker position and size were derived, not judged on screen; there is no dedicated blast art or countdown.
