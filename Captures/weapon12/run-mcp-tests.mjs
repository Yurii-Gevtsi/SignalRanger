import { spawnSync } from 'node:child_process';
import { readFile, stat, copyFile } from 'node:fs/promises';
const [run, mode, ...groups] = process.argv.slice(2);
const xmlPath = `${process.env.USERPROFILE}/AppData/LocalLow/GY Signal Studio/Signal Ranger/TestResults.xml`;
const previous = (await stat(xmlPath)).mtimeMs;
function call(tool, args, output) {
  const result = spawnSync(process.execPath, ['Captures/weapon12/mcp.mjs', tool, JSON.stringify(args), ...(output ? [output] : [])], {encoding: 'utf8'});
  if (!result.stdout.trim()) throw new Error(result.stderr);
  return JSON.parse(result.stdout);
}
let start;
for (let i = 0; i < 6; i++) {
  start = call('run_tests', {mode, group_names:groups, include_failed_tests:true, init_timeout:120000}, `Captures/weapon12/${run}-start.json`);
  if (start.success) break;
  await new Promise(resolve => setTimeout(resolve, 1000));
}
if (!start.success) throw new Error(JSON.stringify(start));
const job = start.data.job_id;
console.log(`Started ${mode} job ${job}`);
for (let i = 0; i < 20; i++) {
  const result = call('get_test_job', {job_id:job, include_details:true, wait_timeout:30}, `Captures/weapon12/${run}-results.json`);
  if (!result.success) { await new Promise(resolve => setTimeout(resolve, 1000)); continue; }
  if (result.data?.status === 'running') continue;
  for (let retry = 0; retry < 20 && (await stat(xmlPath)).mtimeMs <= previous; retry++)
    await new Promise(resolve => setTimeout(resolve, 500));
  if ((await stat(xmlPath)).mtimeMs <= previous) throw new Error('No fresh test XML');
  await copyFile(xmlPath, `Captures/weapon12/${run}.xml`);
  const xml = await readFile(xmlPath, 'utf8');
  const root = xml.match(/<test-run\b[^>]+>/)?.[0];
  console.log(root);
  for (const test of xml.matchAll(/<test-case\b[^>]*result="Failed"[\s\S]*?<\/test-case>/g)) console.log(test[0]);
  process.exit(root?.includes('result="Passed"') && !root.includes('testcasecount="0"') ? 0 : 1);
}
throw new Error('Test job did not complete in 10 minutes');
