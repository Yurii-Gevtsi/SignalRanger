# Weapon 26 — electric harpoon validation

## Result

**Implemented** (mechanics). Card 26, `weapon.electric-harpoon`, TTH limits 4/0/2/4, features `impact_pull` (rank 5) and `double_grab` (rank 10).

- Rope length and its growth come from the rope weapon (`WeaponUpgradeChoiceResolver.ResolveRopeLength`, Radius purchases), not from the old per-rank formula. Skins `electric_harpoon_shot` / `electric_harpoon_5A_shot`.
- Base cycle: the hook flies the rope length; on contact the droid takes X at once and is electrified. It is dragged along the ground (not teleported) at 4.8 u/s (twice the previous speed) for up to five seconds. Every second it takes Y and makes a small hop that can carry it over an obstacle. Without `impact_pull`, reaching the shooter just ends the cycle: no final blow, no throw. If the droid is stuck, the rope holds for five seconds and then lets go without a final action.
- `impact_pull`: reaching the shooter inside the window adds Z and throws the droid away from the shooter at 60° with a range of about two rope lengths.
- `double_grab`: the hook holds two droids, each with its own X/Y/Z cycle and its own pull window; a second rope line is drawn to the second droid.
- A hostile grounder on the hook path takes 1 HP and ends the shot (the shared grounder interception rule).

## Evidence

- EditMode Gameplay+Progression: `Captures/weapon12/w26-edit-2.xml`, 223/223 PASS (new: X then ground pull at double speed, no final blow without impact_pull, impact_pull Z + 60° throw, blocked target keeps the rope 5 s then releases, double grab with two independent cycles and a single grab without the purchase, registry 4/0/2/4 + rope length + art + resolver flags).
- PlayMode: `Captures/weapon12/w26-play.xml`, 36/36 PASS (new: in the real scene the hook shows the base or the evolved shot skin).
- Full EditMode: `Captures/weapon12/w26-edit-full.xml`, 750 tests, 38 failures. The 38 failures are the pre-existing baseline at `7e4388f` (menu/mission/battle visual resource tests); no new failure appears, and two baseline failures (mortar progress tests) now pass.
- Automated editor evidence only; not a phone test.

## Open items

- X/Y/Z shares (0.35 / 0.15 / 0.35 of the weapon damage) are prototype values pending the owner's numbers and the global HP balance.
- `double_grab` visual is a second rope line only; there is no dedicated art for the second target.
- Visuals were not checked on screen.
