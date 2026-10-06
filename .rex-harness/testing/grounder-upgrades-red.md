# Grounder upgrade RED

Unity EditMode `Grounder_AllowsFourCoreAndFourRadiusUpgrades` failed at `WeaponProgressionCatalogTests.cs:50`: expected four Core purchases, observed zero. The radius limit was already four. Exact command: `node tools/run-unity-mcp-editmode-test.mjs Game.Tests.EditMode.Progression.WeaponProgressionCatalogTests.Grounder_AllowsFourCoreAndFourRadiusUpgrades`. Fresh failing receipt: `receipt:0c96da62-d760-42db-9714-ede62d785d26`. This is a behavior gap in the progression catalog, not infrastructure failure.
