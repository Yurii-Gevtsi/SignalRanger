import { readFile, stat } from "node:fs/promises";
import { join } from "node:path";

const endpoint = process.env.UNITY_MCP_ENDPOINT ?? "http://127.0.0.1:8080/mcp";
const testName = process.argv[2];
const unityTestResultsPath = process.env.UNITY_TEST_RESULTS_PATH
    ?? join(
        process.env.USERPROFILE ?? "C:/Users/Yurii",
        "AppData",
        "LocalLow",
        "GY Signal Studio",
        "Signal Ranger",
        "TestResults.xml");

if (!testName) {
    throw new Error("Usage: node tools/run-unity-mcp-editmode-test.mjs <fully-qualified-test-name>");
}

function parseMcpEvent(text) {
    const dataLine = text
        .split(/\r?\n/)
        .filter(line => line.startsWith("data: "))
        .at(-1);
    if (!dataLine) {
        throw new Error(`Unity MCP returned no JSON-RPC event: ${text}`);
    }

    return JSON.parse(dataLine.slice("data: ".length));
}

async function request(sessionId, id, method, params) {
    const response = await fetch(endpoint, {
        method: "POST",
        headers: {
            "content-type": "application/json",
            accept: "application/json, text/event-stream",
            ...(sessionId ? { "mcp-session-id": sessionId } : {})
        },
        body: JSON.stringify({ jsonrpc: "2.0", id, method, params })
    });

    if (!response.ok) {
        throw new Error(`Unity MCP HTTP ${response.status}: ${await response.text()}`);
    }

    return { message: parseMcpEvent(await response.text()), headers: response.headers };
}

async function getTestResultFileTimestamp() {
    try {
        return (await stat(unityTestResultsPath)).mtimeMs;
    } catch {
        return 0;
    }
}

function parseFocusedTestResult(xml) {
    const escapedTestName = testName.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
    const testCaseMatch = xml.match(
        new RegExp(`<test-case\\b[^>]*fullname="${escapedTestName}"[^>]*result="(Passed|Failed|Inconclusive|Skipped)"[^>]*>([\\s\\S]*?)</test-case>`)
    );
    if (!testCaseMatch) {
        return null;
    }

    return {
        status: testCaseMatch[1] === "Passed" ? "succeeded" : "failed",
        test_name: testName,
        details: testCaseMatch[2]
            .replace(/<[^>]+>/g, " ")
            .replace(/<!\[CDATA\[|\]\]>/g, "")
            .trim()
    };
}

const previousResultTimestamp = await getTestResultFileTimestamp();
let jobId;
let startFailure;
for (let startAttempt = 0; startAttempt < 6 && !jobId; startAttempt++) {
    const initialized = await request(null, 1 + startAttempt * 2, "initialize", {
        protocolVersion: "2025-03-26",
        capabilities: {},
        clientInfo: { name: "game-test-runner", version: "1.0" }
    });
    const sessionId = initialized.headers.get("mcp-session-id");
    if (!sessionId) {
        throw new Error("Unity MCP did not return an MCP session id.");
    }

    const run = await request(sessionId, 2 + startAttempt * 2, "tools/call", {
        name: "run_tests",
        arguments: {
            mode: "EditMode",
            test_names: [testName],
            include_failed_tests: true,
            include_details: true
        }
    });
    const runResult = JSON.parse(run.message.result.content[0].text);
    jobId = runResult.data?.job_id;
    startFailure = runResult.error;
    if (!jobId) {
        await new Promise(resolve => setTimeout(resolve, 500));
    }
}

if (!jobId) {
    throw new Error(`Unity MCP did not start the test job after reconnect retries: ${startFailure ?? "unknown error"}`);
}

for (let attempt = 0; attempt < 120; attempt++) {
    const resultTimestamp = await getTestResultFileTimestamp();
    if (resultTimestamp > previousResultTimestamp) {
        const xmlResult = parseFocusedTestResult(await readFile(unityTestResultsPath, "utf8"));
        if (xmlResult) {
            console.log(JSON.stringify(xmlResult, null, 2));
            process.exit(xmlResult.status === "succeeded" ? 0 : 1);
        }
    }

    await new Promise(resolve => setTimeout(resolve, 250));
}

throw new Error(`Unity MCP test job ${jobId} did not produce a focused result within 30 seconds.`);
