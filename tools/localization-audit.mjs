#!/usr/bin/env node

import { readFileSync, readdirSync, statSync } from "node:fs";
import { join, relative } from "node:path";

const root = process.cwd();
const strict = process.argv.includes("--strict");
const localizationDirectory = join(root, "Assets", "_Game", "Content", "Resources", "Localization");
const runtimeDirectory = join(root, "Assets", "_Game", "Runtime");
const languages = readdirSync(localizationDirectory)
    .filter(name => name.endsWith(".json"))
    .map(name => name.slice(0, -5))
    .sort();

function flatten(value, prefix = "", output = {}) {
    for (const [key, child] of Object.entries(value)) {
        const path = prefix ? `${prefix}.${key}` : key;
        if (child && typeof child === "object" && !Array.isArray(child)) flatten(child, path, output);
        else output[path] = typeof child === "string" ? child : "";
    }
    return output;
}

const dictionaries = Object.fromEntries(languages.map(language => [
    language,
    flatten(JSON.parse(readFileSync(join(localizationDirectory, `${language}.json`), "utf8")))
]));
const base = dictionaries.en ?? dictionaries.uk;
const report = { languages, keyParity: [], empty: [], encoding: [], sameAsBase: {}, invariant: {}, hardcodedPlayerText: [], nonEmptyFallbacks: [] };

function isMeaningfulSourceText(value) {
    const withoutArguments = String(value).replace(/\{[^}]*\}/g, "");
    const withoutFormatting = withoutArguments.replace(/[\d\s\p{P}\p{S}]/gu, "");
    return withoutFormatting.length > 2;
}

function isIntentionalInvariant(key) {
    return key.startsWith("campaign.planet.")
        || key.startsWith("content.arena.")
        || key.startsWith("content.weaponEvolutionName.")
        || key === "content.opponent.sigma"
        || key === "content.content.opponent.sigma"
        || key === "campaign.experienceAbbreviation"
        || key === "intro.ranger.title"
        || key === "ui.arsenal"
        || key === "ui.playerLevel"
        || key === "ui.menu"
        || key === "raid.title"
        || key === "duel.title"
        || key === "mode.missions"
        || key === "mode.cluster"
        || key === "mode.arena"
        || key === "content.arenaFallback"
        || key === "campaign.invasionDefense"
        || key === "content.category.bonus"
        || key === "content.category.arc"
        || key === "content.role.direct"
        || key === "content.role.arc"
        || key === "content.upgradeStat.Homing"
        || key === "content.upgradeTitle.generic"
        || key === "raid.difficultyNormal"
        || key === "content.weapon.weapon.fpv-drone"
        || key === "content.weaponEvolutionName.long-range-drill-rocket";
}

for (const language of languages) {
    const entries = dictionaries[language];
    const missing = Object.keys(base).filter(key => !(key in entries));
    const unexpected = Object.keys(entries).filter(key => !(key in base));
    if (missing.length || unexpected.length) report.keyParity.push({ language, missing, unexpected });
    report.empty.push(...Object.entries(entries).filter(([, value]) => !value.trim()).map(([key]) => `${language}:${key}`));
    report.encoding.push(...Object.entries(entries)
        .filter(([, value]) => /[\uFFFD\u0000]/u.test(value))
        .map(([key]) => `${language}:${key}`));
    if (language !== "en") {
        const invariant = Object.entries(entries)
            .filter(([key, value]) => value === base[key] && isIntentionalInvariant(key))
            .map(([key]) => key);
        report.invariant[language] = invariant;
        report.sameAsBase[language] = Object.entries(entries)
            .filter(([key, value]) => value === base[key] && /[A-Za-z]/.test(value)
                && isMeaningfulSourceText(value) && !key.startsWith("meta.")
                && !isIntentionalInvariant(key))
            .map(([key]) => key);
    }
}

function walk(directory) {
    const files = [];
    for (const entry of readdirSync(directory)) {
        const path = join(directory, entry);
        if (statSync(path).isDirectory()) files.push(...walk(path));
        else if (path.endsWith(".cs")) files.push(path);
    }
    return files;
}

for (const path of walk(runtimeDirectory)) {
    const source = readFileSync(path, "utf8");
    const displayPath = relative(root, path);
    for (const match of source.matchAll(/\.text\s*=\s*(\$?"[^"\r\n]*")/g)) {
        const literal = match[1].replace(/\$?"|\{[^}]*\}|\\u[0-9A-Fa-f]{4}/g, "");
        if (/[A-Za-zА-Яа-яІіЇїЄє]/.test(literal)) report.hardcodedPlayerText.push(`${displayPath}:${source.slice(0, match.index).split(/\r?\n/).length}`);
    }
    for (const match of source.matchAll(/\b(?:T|Localized|WeaponName|WeaponDescription|WeaponCategory|MissionTitle|MissionDescription)\s*\(\s*"[^"]+"\s*,\s*(?!string\.Empty\b)([^,)\r\n]+)/g)) {
        if (/^\s*"/.test(match[1])) report.nonEmptyFallbacks.push(`${displayPath}:${source.slice(0, match.index).split(/\r?\n/).length}`);
    }
}

console.log(JSON.stringify(report, null, 2));
const untranslated = Object.values(report.sameAsBase).reduce((count, keys) => count + keys.length, 0);
const failures = report.keyParity.length + report.empty.length + report.encoding.length + report.nonEmptyFallbacks.length
    + report.hardcodedPlayerText.length + (strict ? untranslated : 0);
process.exitCode = failures ? 1 : 0;
