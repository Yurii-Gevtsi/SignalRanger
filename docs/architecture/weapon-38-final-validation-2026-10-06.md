# Weapon 38 — laser fence validation

## Result

**Implemented** (mechanics and presentation fixes). Card 38, `weapon.laser-fence`, TTH limits 4/4/4/4, prototype stock 4 hulls (Loadout base TBD in the card). The deploy/link/crossing core already existed; the closure checked it against the card and fixed what did not match.

- Hulls land on the ground from a thrown body; two hulls of the same owner form a beam when they are within the maximum link length, which is now 5.4 in **both** the simulation and the scene (the scene still used the old 1.8, so beams longer than 1.8 were not drawn although they worked).
- The visible size of a hull is half of the former one (0.055 instead of 0.11).
- A hostile droid crossing a beam takes the weapon's base damage once; the beam is spent and disappears for good (a new event now tells the scene — before, the line stayed on screen after it had fired); if the struck droid is the active one its turn ends. The owner and the owner's side are never hurt.
- A hull can be destroyed by a blast: any explosion covering it with at least its durability (prototype 30, a playtest number as the card says) wrecks it; its beams disappear with it, a carried hull is dropped first, and the scene removes the hull and its beam. Direct non-explosive hits (rail bullet, pellets) do not damage hulls yet.
- The carrier (weapon 35) can move a hull; a link longer than 5.4 stops existing, and a pair that was carried but not spent can link again when brought close; a spent pair is never re-armed.
- Existing skins suffice (`laser_fence` for the hull, the plain beam line).

## Evidence

- EditMode Gameplay: `Captures/weapon12/w38-edit-1.xml` — all laser-fence tests PASS: 5.4 vs 5.5 resolver cut and the public range, one crossing = base damage, the beam spent event, owner/own side safe and the struck active droid's turn ends, blast on a hull removes it and the broken pair no longer hurts, registry 4/4/4/4 and stock 4; plus the pre-existing deploy and crossing tests. (The other failures in that run are the known baseline ones.)
- PlayMode: `Captures/weapon12/w38-play.xml`, 49/49 PASS (real scene: two small hulls at 0.055, the beam drawn within the 5.4 reach, a blast removes the hull and its beam).
- Full EditMode: `Captures/weapon12/w38-edit-full.xml`, 843 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test; the new hull size was not judged on screen.

## Open items

- Hull durability (30) and the rule "a blast within its radius hurts a hull" are prototypes; the card leaves durability to playtest.
- Beams form between **every** pair of the owner's hulls within 5.4 (four hulls can make a mesh); the card speaks of "two hulls" — the owner to confirm whether a fixed pairing is wanted.
- The Loadout base stock (4) and the meaning of the Aim purchases for this weapon are not in the card.
- Direct bullet hits on hulls are not implemented.
