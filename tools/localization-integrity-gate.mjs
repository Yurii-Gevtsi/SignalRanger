#!/usr/bin/env node

import { execFileSync } from "node:child_process";
import { readFileSync, readdirSync, statSync } from "node:fs";
import { join, relative } from "node:path";

const root = process.cwd();
const localizationDirectory = join(root, "Assets", "_Game", "Content", "Resources", "Localization");
const runtimeDirectory = join(root, "Assets", "_Game", "Runtime");
const languages = ["ar", "de", "en", "es", "fr", "it", "ja", "ko", "pl", "pt-BR", "tr", "uk", "zh-Hans"];
const baseLanguage = "en";
const mojibakeMarkers = [
    "\u00d0", "\u00d1", "\ufffd", "\u00c3\u00a2",
    "\u0420\u2019", "\u0420\u040f", "\u0420\u0491", "\u0413\u2014"
];
const args = new Set(process.argv.slice(2));
const failures = [];
const dictionaries = new Map();

function flatten(node, prefix = "", output = {}, duplicates = []) {
    for (const [key, value] of Object.entries(node)) {
        const path = prefix.length === 0 ? key : `${prefix}.${key}`;
        if (value !== null && typeof value === "object" && !Array.isArray(value)) {
            flatten(value, path, output, duplicates);
        } else {
            if (Object.hasOwn(output, path)) {
                duplicates.push(path);
                continue;
            }

            output[path] = typeof value === "string" ? value : "";
        }
    }

    return { entries: output, duplicates };
}

function readDictionary(language) {
    const path = join(localizationDirectory, `${language}.json`);
    const buffer = readFileSync(path);
    const text = new TextDecoder("utf-8", { fatal: true }).decode(buffer);
    return flatten(JSON.parse(text));
}

function sortedKeys(entries) {
    return Object.keys(entries).sort();
}

function sameKeys(left, right) {
    return left.length === right.length && left.every((key, index) => key === right[index]);
}

function placeholders(value) {
    return [...String(value).matchAll(/\{\s*([^}]+?)\s*\}/g)]
        .map(match => match[1])
        .sort()
        .join("|");
}

function walk(directory, predicate, output = []) {
    for (const entry of readdirSync(directory)) {
        const path = join(directory, entry);
        if (statSync(path).isDirectory()) {
            walk(path, predicate, output);
        } else if (predicate(path)) {
            output.push(path);
        }
    }

    return output;
}

function stagedAddedLines() {
    const diff = execFileSync("git", ["diff", "--cached", "--unified=0", "--", "Assets/_Game/Runtime"], {
        cwd: root,
        encoding: "utf-8"
    });
    return diff.split(/\r?\n/)
        .filter(line => line.startsWith("+") && !line.startsWith("+++"))
        .map(line => line.slice(1));
}

function verifyDictionaries() {
    for (const language of languages) {
        try {
            const dictionary = readDictionary(language);
            dictionaries.set(language, dictionary.entries);
            for (const duplicateKey of dictionary.duplicates) {
                failures.push(`${language}.json defines duplicate flattened key: ${duplicateKey}.`);
            }
        } catch (error) {
            failures.push(`${language}.json is not strict UTF-8 JSON: ${error.message}`);
        }
    }

    const baseEntries = dictionaries.get(baseLanguage);
    if (!baseEntries) {
        return;
    }

    const baseKeys = sortedKeys(baseEntries);
    for (const [language, entries] of dictionaries) {
        const keys = sortedKeys(entries);
        if (!sameKeys(baseKeys, keys)) {
            failures.push(`${language}.json does not have the exact ${baseLanguage}.json key set.`);
        }

        for (const [key, value] of Object.entries(entries)) {
            if (value.trim().length === 0) {
                failures.push(`${language}.json has an empty player-facing value for ${key}.`);
            }
            if (mojibakeMarkers.some(marker => value.includes(marker))) {
                failures.push(`${language}.json contains an encoding-loss marker in ${key}.`);
            }
            if (Object.hasOwn(baseEntries, key) && placeholders(value) !== placeholders(baseEntries[key])) {
                failures.push(`${language}.json changes placeholder names in ${key}.`);
            }
            if (language === "uk" && value.includes("?")) {
                failures.push(`uk.json contains a question-mark loss marker in ${key}.`);
            }
        }
    }
}

function verifyStagedPlayerText() {
    if (!args.has("--staged")) {
        return;
    }

    const added = stagedAddedLines();
    const fallbackCall = /\bT\s*\(\s*"[^"]+"\s*,(?![ \t]*string\.Empty\b)[ \t]*/;
    const directAssignment = /\.text\s*=\s*(?:\$?"|new\s+string\s*\()/;
    const directModeText = /\bCreateModeText\s*\([^,]+,\s*[^,]+,\s*"[^"]+"/;
    for (const line of added) {
        if (fallbackCall.test(line)) {
            failures.push(`Non-empty localization fallback in staged C#: ${line.trim()}`);
        }
        if (directAssignment.test(line) && !line.includes("string.Empty")) {
            failures.push(`Direct player-text assignment in staged C#: ${line.trim()}`);
        }
        if (directModeText.test(line)) {
            failures.push(`Direct player text passed to CreateModeText in staged C#: ${line.trim()}`);
        }
    }
}

function verifyGameTextFallbacks() {
    const path = join(runtimeDirectory, "Core", "Localization", "GameText.cs");
    const source = readFileSync(path, "utf8");
    let cursor = 0;
    let nonEmptyFallbacks = 0;
    while (cursor < source.length) {
        const callStart = source.indexOf('T("', cursor);
        if (callStart < 0) {
            break;
        }

        let keyEnd = callStart + 3;
        while (keyEnd < source.length && source[keyEnd] !== '"') {
            keyEnd += source[keyEnd] === "\\" ? 2 : 1;
        }
        keyEnd++;
        let fallbackStart = keyEnd;
        while (/\s/.test(source[fallbackStart])) fallbackStart++;
        if (source[fallbackStart] !== ",") {
            cursor = keyEnd;
            continue;
        }
        fallbackStart++;
        while (/\s/.test(source[fallbackStart])) fallbackStart++;

        let index = fallbackStart;
        let depth = 0;
        let inString = false;
        while (index < source.length) {
            const character = source[index];
            if (inString) {
                if (character === "\\") index += 2;
                else if (character === '"') { inString = false; index++; }
                else index++;
                continue;
            }
            if (character === '"') { inString = true; index++; continue; }
            if (character === "(" || character === "[" || character === "{") { depth++; index++; continue; }
            if (character === ")" || character === "]" || character === "}") {
                if (depth === 0 && character === ")") break;
                depth--; index++; continue;
            }
            if (character === "," && depth === 0) break;
            index++;
        }

        if (source.slice(fallbackStart, index).trim() !== "string.Empty") {
            nonEmptyFallbacks++;
        }
        cursor = index;
    }

    if (nonEmptyFallbacks > 0) {
        failures.push(`GameText.cs has ${nonEmptyFallbacks} non-empty player-text fallback(s).`);
    }

    const baseEntries = dictionaries.get(baseLanguage);
    if (!baseEntries) {
        return;
    }
    const literalKeys = [...source.matchAll(/\bT\s*\(\s*"([^"]+)"/g)].map(match => match[1]);
    for (const key of new Set(literalKeys)) {
        if (key.endsWith(".")) {
            continue;
        }
        if (!(key in baseEntries)) {
            failures.push(`GameText.cs references a missing localization key: ${key}.`);
        }
    }
}

function verifyCampaignCatalogLocalizationKeys() {
    const catalogPath = join(runtimeDirectory, "App", "CampaignPlanetCatalog.cs");
    const source = readFileSync(catalogPath, "utf8");
    const baseEntries = dictionaries.get(baseLanguage);
    if (!baseEntries) {
        return;
    }

    const catalogKeys = new Set(
        [...source.matchAll(/"(campaign\.(?:sector|planet)\.[^"]+)"/g)].map(match => match[1])
    );
    for (const key of catalogKeys) {
        if (!(key in baseEntries)) {
            failures.push(`CampaignPlanetCatalog.cs references a missing localization key: ${key}.`);
        }
    }
}

verifyDictionaries();
verifyGameTextFallbacks();
verifyCampaignCatalogLocalizationKeys();
verifyStagedPlayerText();

if (failures.length > 0) {
    console.error("Localization Integrity Gate: FAILED");
    for (const failure of failures) {
        console.error(`- ${failure}`);
    }
    process.exit(1);
}

console.log(`Localization Integrity Gate: PASS (${languages.length} dictionaries, strict UTF-8, exact key parity).`);
