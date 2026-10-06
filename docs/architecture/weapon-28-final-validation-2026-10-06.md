# Weapon 28 — grounder validation

## Result

**Implemented** (mechanics). Card 28, `weapon.grounder`, TTH limits 4/0/0/4 (Core, Radius), one device per battle.

- Placement: no aim, the device drops next to the owner and is shown with the `grounder` skin. Using it spends the single charge, does not heal the owner and hands the turn over.
- Defence group: only hostile physical carriers are attracted. A carrier that flies inside the device's `N0` (base 1.4; each of four Radius purchases adds 10% of the base, 1.96 at four) is pulled to the magnet and detonates there, hurting the device by its direct damage (prototype device health 45, so two ordinary hits destroy it). Explosions at the magnet hurt the owner only if the owner stands inside the physical blast radius. Friendly projectiles and energy carriers (`impulse`, `emp`) are ignored.
- Non-explosive carriers are destroyed at the magnet without running their action (teleport and swap-jump spheres, repair jammer, guardian and laser-fence deploy bodies; deployed mines detonate at the magnet instead of deploying). Metal hooks (rope, electric harpoon) are caught during flight and cost the device 1 HP; no blast.
- Duration: 2 enemy turn-ends; each Core purchase adds one (to 6 at four). The counter ticks at the end of the opposing team's turns, so the owner's own turn in between is not counted.
- Removal clears the skin.

## Evidence

- EditMode `BattleSimulationTests`: `Captures/weapon12/w28-edit-1.xml`, 236/236 PASS. New: an 18-case matrix (16 physical carriers incl. mortar shards, sticky, hopping, cryo, incendiary, missile, orbital rocket, ion pellet, disc, bur rocket, teleport, swap jump, jammer are caught and ended; the two energy carriers fly on), blast hurts the owner only inside the radius, registry 4/0/0/4 + one charge + art + paid Core/Radius steps and limits + JSON. Existing tests keep covering friendly/energy ignore, two-hit destruction, 2→6 turns, +40% N0, hooks 1 HP, intercepted mine, deployables.
- PlayMode: `Captures/weapon12/w28-play.xml`, 38/38 PASS (new: the real scene places the `grounder` skin, spends the single charge and removes the skin after two enemy turns).
- Full EditMode: `Captures/weapon12/w28-edit-full.xml`, 775 tests, only the 38 baseline failures of `7e4388f`.
- Automated editor evidence only; not a phone test.

## Open items

- Device health 45 is the prototype for `45% × H_unlock(grounder)`; global HP balance pending (owner).
- "2 turns" is counted as two enemy turn-ends (the owner's turn in between is not counted); owner to confirm that this, and not the owner's own turns, is the intended unit.
- Carriers of weapons 29–41 (sky hook, spider body, virus, bomber, UFO, nuclear charge, laser fence, cannons) must be added to the carrier matrix as those weapons are closed.
- Visual size/position of the magnet was not judged on screen.
