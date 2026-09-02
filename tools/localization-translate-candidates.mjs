#!/usr/bin/env node

// One-shot translation helper for the localization-translator agent.
// It uses MyMemory's public neural translation endpoint; review its output
// before shipping and never use it as a substitute for human proofreading.
import { readFile, writeFile } from "node:fs/promises";
import { join } from "node:path";

const root = process.cwd();
const directory = join(root, "Assets", "_Game", "Content", "Resources", "Localization");
const localeMap = {
    ar: "ar", de: "de", es: "es", fr: "fr", it: "it", ja: "ja", ko: "ko",
    pl: "pl", "pt-BR": "pt", tr: "tr", uk: "uk", "zh-Hans": "zh-CN"
};
const files = Object.keys(localeMap);
const delayMs = Number(process.env.LOCALIZATION_TRANSLATION_DELAY_MS ?? 120);
const dryRun = process.argv.includes("--dry-run");

function flatten(value, prefix = "", output = {}) {
    for (const [key, child] of Object.entries(value)) {
        const path = prefix ? `${prefix}.${key}` : key;
        if (child && typeof child === "object" && !Array.isArray(child)) flatten(child, path, output);
        else output[path] = typeof child === "string" ? child : "";
    }
    return output;
}

function setPath(value, path, replacement, prefix = "") {
    for (const [key, child] of Object.entries(value)) {
        const current = prefix ? `${prefix}.${key}` : key;
        if (current === path) {
            value[key] = replacement;
            return true;
        }
        if (child && typeof child === "object" && !Array.isArray(child)
            && setPath(child, path, replacement, current)) return true;
    }
    return false;
}

function isMeaningfulSourceText(value) {
    const withoutArguments = String(value).replace(/\{[^}]*\}/g, "");
    const withoutFormatting = withoutArguments.replace(/[\d\s\p{P}\p{S}]/gu, "");
    return withoutFormatting.length > 2;
}

function isIntentionalInvariant(key) {
    return key.startsWith("campaign.planet.") || key.startsWith("content.arena.")
        || key.startsWith("content.weaponEvolutionName.")
        || key === "content.opponent.sigma" || key === "content.content.opponent.sigma"
        || key === "campaign.experienceAbbreviation" || key === "intro.ranger.title"
        || key === "ui.arsenal" || key === "ui.playerLevel" || key === "ui.menu"
        || key === "raid.title" || key === "duel.title" || key === "mode.missions"
        || key === "mode.cluster" || key === "mode.arena" || key === "content.arenaFallback"
        || key === "content.category.bonus" || key === "content.category.arc"
        || key === "content.role.direct" || key === "content.role.arc"
        || key === "content.upgradeStat.Homing" || key === "content.upgradeTitle.generic"
        || key === "raid.difficultyNormal" || key === "content.weapon.weapon.fpv-drone"
        || key === "content.weaponEvolutionName.long-range-drill-rocket";
}

async function translate(text, target) {
    const url = new URL("https://clients5.google.com/translate_a/t");
    url.searchParams.set("client", "dict-chrome-ex");
    url.searchParams.set("q", text);
    url.searchParams.set("sl", "en");
    url.searchParams.set("tl", target);
    const response = await fetch(url);
    if (!response.ok) throw new Error(`HTTP ${response.status}`);
    const payload = await response.json();
    const result = Array.isArray(payload) && typeof payload[0] === "string" ? payload[0] : null;
    if (typeof result !== "string" || !result.trim()) throw new Error("empty translation");
    return result.replaceAll("&#39;", "'").replaceAll("&quot;", '"').replaceAll("&#10;", "\n");
}

async function translateBatch(items, target) {
    const delimiter = "\n<<<LOC_SPLIT>>>\n";
    const translated = await translate(items.map(item => item.value).join(delimiter), target);
    const parts = translated.split(delimiter);
    if (parts.length !== items.length) throw new Error(`batch split mismatch (${parts.length}/${items.length})`);
    return parts;
}

const english = flatten(JSON.parse(await readFile(join(directory, "en.json"), "utf8")));
const summary = {};
for (const language of files) {
    const path = join(directory, `${language}.json`);
    const source = JSON.parse(await readFile(path, "utf8"));
    const entries = flatten(source);
    const candidates = Object.entries(entries).filter(([key, value]) =>
        value === english[key] && /[A-Za-z]/.test(value) && isMeaningfulSourceText(value)
        && !isIntentionalInvariant(key) && !key.startsWith("meta."));
    let changed = 0;
    let unchanged = 0;
    for (let start = 0; start < candidates.length; start += 4) {
        const batch = candidates.slice(start, start + 4).map(([key, value]) => ({ key, value }));
        let replacements;
        try {
            replacements = await translateBatch(batch, localeMap[language]);
        } catch (error) {
            throw new Error(`${language}:${batch[0].key}: ${error.message}`);
        }
        for (let index = 0; index < batch.length; index++) {
            const replacement = replacements[index].trim();
            if (replacement === batch[index].value) unchanged++;
            else {
                if (!setPath(source, batch[index].key, replacement)) throw new Error(`${language}:${batch[index].key}: key not found`);
                changed++;
            }
        }
        await new Promise(resolve => setTimeout(resolve, delayMs));
    }
    summary[language] = { candidates: candidates.length, changed, unchanged };
    if (!dryRun) await writeFile(path, `${JSON.stringify(source, null, 2)}\n`, "utf8");
}
console.log(JSON.stringify({ dryRun, summary }, null, 2));
