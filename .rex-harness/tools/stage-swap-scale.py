from pathlib import Path
import os
import subprocess
import tempfile

root = Path(__file__).resolve().parents[2]

def git(*args, input_bytes=None, env=None):
    result = subprocess.run(["git", *args], cwd=root, input=input_bytes,
                            capture_output=True, check=True, env=env)
    return result.stdout.decode("utf-8")

def base(path):
    return git("show", f"HEAD:{path}")

files = {}
p = "Assets/_Game/Runtime/Gameplay/Battle/Assembly/BattleSceneController.cs"
s = base(p)
old = '                "weapon.teleport" => Vector3.one * (diameter * 0.72f),'
assert s.count(old) == 1
files[p] = s.replace(old, old + '\n                "weapon.swap-jump" => Vector3.one * (diameter * 0.72f),')

p = "Assets/_Game/Tests/EditMode/Gameplay/BattleVisualResourceTests.cs"
s = base(p)
w = (root / p).read_text(encoding="utf-8")
start = "                var ordinaryTeleport = WeaponDefinition.CreateRuntime("
end = "            }\n            finally"
added = w[w.index(start):w.index(end, w.index(start))]
old = '                Assert.That(sprite.texture.name, Is.EqualTo("teleport_5_10_A"));\n'
assert s.count(old) == 1
files[p] = s.replace(old, old + added)

with tempfile.TemporaryDirectory() as temp:
    index = Path(temp) / "index"
    env = dict(os.environ, GIT_INDEX_FILE=str(index))
    git("read-tree", "HEAD", env=env)
    for path, content in files.items():
        oid = git("hash-object", "-w", "--stdin", input_bytes=content.encode("utf-8")).strip()
        git("update-index", "--add", "--cacheinfo", f"100644,{oid},{path}", env=env)
    git("diff", "--cached", "--check", env=env)
    print(git("diff", "--cached", "--stat", env=env))
    (root / ".git" / "index").write_bytes(index.read_bytes())
