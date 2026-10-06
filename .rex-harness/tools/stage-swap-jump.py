from pathlib import Path
import os
import subprocess
import tempfile


ROOT = Path(__file__).resolve().parents[2]


def git(*args, input_text=None, env=None):
    if input_text is not None:
        return subprocess.run(
            ["git", *args], cwd=ROOT, input=input_text.encode("utf-8"),
            capture_output=True, check=True, env=env).stdout.decode("utf-8")
    return subprocess.run(
        ["git", *args], cwd=ROOT, text=True, encoding="utf-8",
        capture_output=True, check=True, env=env).stdout


def base(path):
    return git("show", f"HEAD:{path}")


def current(path):
    return (ROOT / path).read_text(encoding="utf-8")


def section(text, start, end):
    first = text.index(start)
    return text[first:text.index(end, first)]


def one_test(text, name):
    marker = f"        [Test]\n        public void {name}()"
    first = text.index(marker)
    return text[first:text.index("        [Test]", first + len(marker))]


def replace_one(text, old, new):
    assert text.count(old) == 1, (old[:100], text.count(old))
    return text.replace(old, new)


files = {}
p = "Assets/_Game/Runtime/Gameplay/Battle/Simulation/BattleSimulation.cs"
b, w = base(p), current(p)
b = replace_one(b, 'private const string TeleportWeaponId = "weapon.teleport";',
    'private const string TeleportWeaponId = "weapon.teleport";\n        private const string SwapJumpWeaponId = "weapon.swap-jump";')
b = replace_one(b,
    section(b, "        private void RepositionOwner(", "        private bool TryFindTeleportExchangeTarget("),
    section(w, "        private void RepositionOwner(", "        private bool TryFindTeleportExchangeTarget("))
b = replace_one(b, "        private void ExchangeTeleportPositions(",
    section(w, "        private bool TryFindSwapJumpExchangeTarget(", "        private void ExchangeTeleportPositions(")
    + "        private void ExchangeTeleportPositions(")
b = replace_one(b,
    section(b, "        private void ExchangeTeleportPositions(", "        private void MoveTeleportExchangeTarget("),
    section(w, "        private void ExchangeTeleportPositions(", "        private void MoveTeleportExchangeTarget("))
b = replace_one(b,
    "RepositionOwner(impact.Point, impact.OwnerUnitId, impactWeapon);",
    "RepositionOwner(impact.Point, impact.OwnerUnitId, impactWeapon, impact.SourceOrigin);")
b = replace_one(b,
    "            var projectile = new SimProjectile(\n                ++_nextProjectileId,",
    "            var spawnedWeapon = ResolveWeaponFromProjectileId(request.Definition.ProjectileId);\n"
    "            var sourceOrigin = spawnedWeapon?.WeaponId == SwapJumpWeaponId\n"
    "                ? _state.GetUnit(request.OwnerUnitId)?.Position\n"
    "                : (Vector2?)null;\n"
    "            var projectile = new SimProjectile(\n                ++_nextProjectileId,")
b = replace_one(b,
    "                request.Definition.TimeoutSeconds);\n            var spawnedWeapon = ResolveWeaponFromProjectileId(request.Definition.ProjectileId);",
    "                request.Definition.TimeoutSeconds,\n                sourceOrigin: sourceOrigin);")
b = replace_one(b,
    "                        || projectileWeapon?.WeaponId == TeleportWeaponId\n",
    "                        || projectileWeapon?.WeaponId == TeleportWeaponId\n"
    "                        || projectileWeapon?.WeaponId == SwapJumpWeaponId\n")
b = replace_one(b,
    "                projectile.Definition.KnockbackMultiplier);",
    "                projectile.Definition.KnockbackMultiplier,\n                projectile.SourceOrigin);")
files[p] = b

p = "Assets/_Game/Runtime/Gameplay/Battle/Projectiles/ProjectileImpact.cs"
b = base(p)
b = replace_one(b, "float knockbackMultiplier = 1f)",
    "float knockbackMultiplier = 1f,\n            Vector2? sourceOrigin = null)")
b = replace_one(b, "KnockbackMultiplier = Mathf.Max(0f, knockbackMultiplier);",
    "KnockbackMultiplier = Mathf.Max(0f, knockbackMultiplier);\n            SourceOrigin = sourceOrigin;")
b = replace_one(b, "public float KnockbackMultiplier { get; }",
    "public float KnockbackMultiplier { get; }\n        public Vector2? SourceOrigin { get; }")
files[p] = b

p = "Assets/_Game/Runtime/Gameplay/Battle/Simulation/SimProjectile.cs"
b = base(p)
b = replace_one(b, "float remainingLifetime)",
    "float remainingLifetime,\n            Vector2? sourceOrigin = null)")
b = replace_one(b, "RemainingLifetime = remainingLifetime;",
    "RemainingLifetime = remainingLifetime;\n            SourceOrigin = sourceOrigin;")
b = replace_one(b, "public float RemainingLifetime { get; private set; }",
    "public float RemainingLifetime { get; private set; }\n"
    "        /// <summary>Throw-time origin for swap-jump's enemy destination.</summary>\n"
    "        public Vector2? SourceOrigin { get; }")
files[p] = b

p = "Assets/_Game/Runtime/Gameplay/Battle/Assembly/BattleSceneController.cs"
b = base(p)
b = replace_one(b,
    '                "weapon.teleport" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),',
    '                "weapon.teleport" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),\n'
    '                "weapon.swap-jump" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),')
files[p] = b

p = "Assets/_Game/Tests/EditMode/Gameplay/BattleSimulationTests.cs"
b, w = base(p), current(p)
b = replace_one(b,
    section(b, "        private static Harness CreateHarness(", "        [Test]"),
    section(w, "        private static Harness CreateHarness(", "        [Test]"))
b = replace_one(b,
    "        [Test]\n        public void Grenade_FuseCountsFromThrow_AndExplodesWhileResting()",
    section(w,
        "        [Test]\n        public void SwapJump_WithoutNearbyEnemy_RepositionsOwnerOnly()",
        "        [Test]\n        public void Grenade_FuseCountsFromThrow_AndExplodesWhileResting()")
    + "        [Test]\n        public void Grenade_FuseCountsFromThrow_AndExplodesWhileResting()")
files[p] = b

p = "Assets/_Game/Tests/EditMode/Gameplay/BattleVisualResourceTests.cs"
b, w = base(p), current(p)
b = replace_one(b, "using Game.Gameplay.Battle.Runtime;",
    "using Game.Gameplay.Battle.Runtime;\nusing Game.Gameplay.Battle.Simulation;")
b = replace_one(b,
    "        [Test]\n        public void LoadingScreenPreview_BuildsMissionTextSpinnerAndPercent()",
    one_test(w, "SwapJump_UsesTeleportProjectileSprite")
    + "        [Test]\n        public void LoadingScreenPreview_BuildsMissionTextSpinnerAndPercent()")
files[p] = b

with tempfile.TemporaryDirectory() as temp:
    index = Path(temp) / "index"
    env = dict(os.environ, GIT_INDEX_FILE=str(index))
    git("read-tree", "HEAD", env=env)
    for path, content in files.items():
        object_id = git("hash-object", "-w", "--stdin", input_text=content).strip()
        git("update-index", "--add", "--cacheinfo", f"100644,{object_id},{path}", env=env)
    print(git("diff", "--cached", "--stat", env=env))
    print(git("diff", "--cached", "--check", env=env))
    # Copy only after every transformation and index check succeeded.
    Path(ROOT / ".git" / "index").write_bytes(index.read_bytes())
