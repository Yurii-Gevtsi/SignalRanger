# Rope traversal tool

## Implemented behaviour

- The engineering weapon is unlocked by default, consumes no ammunition, and uses a 45-degree total aiming sector centred on vertical.
- Its hook advances on a straight path, anchors at the first solid terrain cell, and disappears when it reaches its maximum range.
- While attached, the droid remains constrained by the rope length. Left and right add tangential momentum; up reels in and down releases line.
- A second fire input detaches the rope and hook without resetting the droid velocity, preserving the current swing trajectory.
- The tool has one development path. Higher ranks increase maximum line range, the upward aiming sector, and horizontal swing acceleration.

## Ownership

- `RopeTraversal` owns the hook flight and fixed anchor.
- `UnitMotor` owns rope-constrained movement and detachment momentum.
- `BattleSimulation` owns turn-safe activation, range scaling, and input routing.
- `BattleSceneController` and `BattleHudView` own the line/hook presentation and the temporary Up/Down controls.

## Delivery checklist

- [x] Straight hook flight with terrain anchoring and range expiry.
- [x] Tether-constrained movement, reel controls, and momentum-preserving release.
- [x] Runtime tether/hook visual and dedicated `rope` / `rope_shot` assets.
- [x] Default unlock, unlimited combat uses, single development path, and localization.
- [ ] Device playtest: tune range, reel rate, and swing acceleration on target hardware.
