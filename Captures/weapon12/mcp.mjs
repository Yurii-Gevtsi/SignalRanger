import { writeFile } from 'node:fs/promises';
const [tool, argumentsJson, output] = process.argv.slice(2);
async function request(session, id, method, params) {
  const response = await fetch('http://127.0.0.1:8080/mcp', {
    method: 'POST', headers: { 'Content-Type': 'application/json', Accept: 'application/json, text/event-stream', ...(session ? {'mcp-session-id': session} : {}) },
    body: JSON.stringify({jsonrpc: '2.0', id, method, params})
  });
  const body = await response.text();
  if (!response.ok) throw new Error(`${response.status}: ${body}`);
  const data = body.split(/\r?\n/).filter(line => line.startsWith('data: ')).at(-1);
  return {session: response.headers.get('mcp-session-id'), message: JSON.parse(data ? data.slice(6) : body)};
}
const initialized = await request(null, 1, 'initialize', {protocolVersion: '2025-03-26', capabilities: {}, clientInfo: {name: 'weapon12-validation', version: '1'}});
const response = await request(initialized.session, 2, tool === 'schemas' ? 'tools/list' : 'tools/call', tool === 'schemas' ? {} : {name: tool, arguments: JSON.parse(argumentsJson)});
const result = tool === 'schemas' ? response.message.result.tools.filter(t => JSON.parse(argumentsJson).includes(t.name)).map(t => ({name:t.name,inputSchema:t.inputSchema})) : JSON.parse(response.message.result.content[0].text);
if (output) await writeFile(output, JSON.stringify(result, null, 2));
console.log(JSON.stringify(result, null, 2));
if (result.success === false) process.exitCode = 1;
