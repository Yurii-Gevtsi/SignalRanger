# Grounder turn lifetime RED

Unity EditMode test `Grounder_ExpiresAfterTwoOpposingTurns` places the device and advances turns through two completed opponent turns. `GrounderRemoved` must fire once after the second, never before it. The test observed zero removals at `BattleSimulationTests.cs:306`, matching the missing lifetime countdown. Exact command: `node tools/run-unity-mcp-editmode-test.mjs Game.Tests.EditMode.Gameplay.BattleSimulationTests.Grounder_ExpiresAfterTwoOpposingTurns`. Fresh failing receipt: `receipt:de31cf51-6e9d-4fb3-a58a-492e0d03a5a0`.
