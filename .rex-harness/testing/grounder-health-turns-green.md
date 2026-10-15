# Grounder turn lifetime GREEN

Added `ExpireGroundersForEndingTurn` to the three existing turn-advance paths in `BattleSimulation`. It decrements a grounder's remaining duration only when the opposing team's turn ends and emits `GrounderRemoved` at zero. The focused Unity EditMode test passed: `receipt:b37e2e47-2ee9-49dd-baa2-224acfaac090`. This slice does not cover device damage, upgrades, or rendering.
