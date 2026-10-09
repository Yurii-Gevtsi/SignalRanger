import { spawnSync } from 'node:child_process';

const code = `foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies()) {
    var type = assembly.GetType("Game.Tests.PlayMode.AppFlowSmokeTests");
    if (type == null) continue;
    try {
        type.GetMethod("Initialize_RebuildsDestroyedRuntimeContent").Invoke(System.Activator.CreateInstance(type), null);
        return "PASS: live mission selection, admin selection, unchanged progression, idempotent initialization";
    } catch (System.Reflection.TargetInvocationException error) {
        throw error.InnerException ?? error;
    }
}
throw new System.Exception("Test assembly not loaded");`;
const run = spawnSync(process.execPath, ['Captures/weapon12/mcp.mjs', 'execute_code',
    JSON.stringify({ action: 'execute', code })], { encoding: 'utf8' });
process.stdout.write(run.stdout);
process.stderr.write(run.stderr);
process.exit(run.status ?? 1);
