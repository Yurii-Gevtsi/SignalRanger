# Weapon 29 — force field validation

## Result

**Implemented** (mechanics). Card 29, `weapon.force-field`, TTH limits 4/0/0/4 (Core, Radius), one device per battle. The earlier registry limit for Core was 0; the card makes Core available (+1 turn per purchase, four purchases), so it is now 4/0/0/4 like the grounder.

- Placement: no aim, a sphere centred on the placing droid; it is the only radius of the device, equal to the shared nearby distance `N0` (1.4), +10% of the base per Radius purchase (1.96 at four). Spending the charge hands the turn over. The prototype library radius 1.8 was replaced by the shared `N0`.
- Defence group: only **hostile energy carriers** are stopped. A charge of `impulse`, `emp` (and the future cannons `freeze`, `fire-gun`, `toxic`) is absorbed whole on the first sub-step inside the sphere: no blast, no damage, no status. Physical carriers (grenades, missiles, shards…) pass through — they belong to the grounder. The owner's own charges pass through its own field. A status already applied to a droid is not removed.
- Beams: a drill-laser beam is cut where it first enters a hostile field (range is shortened to the boundary, and the visual event carries the shortened range); a beam that starts inside the field is not cut by it. Plasma sword has no carrier and is unaffected (the card defines no defence for the blade).
- Duration: 2 enemy turn-ends, each Core purchase adds one (6 at four) — the same counting as the grounder, and the same wording as its player hint ("ходи противника").
- Presentation: `force_field_sphere` is drawn scaled to the field diameter, flashes on every absorption, and disappears when the field expires. The Core description reuses the approved text of the grounder core ("Покращує енергетичне ядро та збільшує час дії на 1 хід."). The weapon hint text was updated for all 13 languages (see the localization commit part).
- The mechanic is no longer gated off (`IsBattleMechanicAvailable`); the old "unimplemented utility" test no longer lists it.

## Evidence

- EditMode `BattleSimulationTests`: `Captures/weapon12/w29-edit-2.xml`, 247/247 PASS. New: energy carriers stopped on the boundary without a blast while physical ones are not (5 cases), friendly energy ignored, radius = N0 and 1.96 at four purchases checked by the absorption point, 2→6 enemy turns, one per battle + turn handed over, beam cut at 1.6 (field boundary at −1.4), registry 4/0/0/4 + art + core description key + paid Core/Radius steps and limits + JSON.
- PlayMode: `Captures/weapon12/w29-play.xml`, 39/39 PASS (new: the real scene shows the sphere at diameter 2×N0, an enemy impulse charge is absorbed, the sphere flashes, one per battle, the sphere is gone after two enemy turns).
- Full EditMode: `Captures/weapon12/w29-edit-full.xml`, 786 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test.

## Open items

- The field is static at its placement point (it does not follow the droid) — owner to confirm; no rule in the card.
- Explosions outside the sphere whose blast radius reaches inside are not blocked (the carrier is stopped, an already-detonated blast is not a carrier).
- Beam cutting is implemented for the drill laser only; laser fence, cryo and lava streams, and the cannon charges must use `IsForceFieldEnergyCarrier` / `ClipBeamAtHostileForceFields` when weapons 38–41 are closed.
- The sphere art was not judged on screen (alpha, size relative to the droid).
