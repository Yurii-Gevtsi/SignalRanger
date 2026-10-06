# Grounder scene visual RED

The focused Unity EditMode test creates an actual `BattleSceneController`, attempts to invoke its grounder deployment handler, and checks the resulting SpriteRenderer and cleanup. It fails at `BattleVisualResourceTests.cs:227` because the handler is null; no scene visual is created. Exact command: `node tools/run-unity-mcp-editmode-test.mjs Game.Tests.EditMode.Gameplay.BattleVisualResourceTests.Grounder_DeploymentShowsItsSkinAndRemovalClearsIt`. Fresh RED receipt: `receipt:43680575-a9f9-491e-b4a6-ddc5b72fd442`.
