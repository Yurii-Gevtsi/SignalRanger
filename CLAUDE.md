# Правила проєкту для Claude
Це оригінальна мобільна Unity-гра, яка створюється з нуля. Приклади в `Examples/` використовуються як глибокий технічний і продуктовий референс для пришвидшення розробки.

## Головна мета

Спроєктувати й реалізувати простішу, доступнішу мобільну тактичну artillery-гру з короткими покроковими боями, чесною фізикою пострілу, чистим мобільним UI, поступовою прогресією та можливістю майбутнього росту через зброю, арени, події, PvE/PvP, економіку, рекламу, покупки й live-ops.

Базовий продуктовий напрям:

```text
чистота бойового ядра як у компактних успішних artillery-іграх
+ відчуття довгострокового розвитку як у live-іграх
- перевантажена економіка, VIP-перевага, шум підсистем і токсична реклама
```

## Робота з прикладами

У `Examples/` лежить розпакований і декомпільований референс-пакет:
декомпільований ігровий код (у його `_decompiled_swf/scripts/ru/`) +
оригінальні ассети. Це робочий, перевірений у продакшені референс —
ним можна вільно користуватись, щоб не робити все з нуля.

Перед кожною суттєвою новою фічею або системою спершу подивись, як це
реалізовано в референсі:

1. Знайти в референсі подібну систему, екран, механіку або групу
   ассетів (ключові пакети: `physics/`, `physics/model/`, `weapons/`,
   `weapons/ammo/`, `objects/`, `game/`, `scene/`, `messages/`).
2. Розібрати *принцип* робочого механізму, а тоді реалізувати його
   заново під наш проект — власні назви, моделі, конфіги, баланс, UI.

## Механіка та фізика — орієнтир на референс

Бойове ядро (політ снаряда, гравітація, руйнівний ландшафт, колізії,
відскок, кут+сила пострілу, черговість ходів) — специфіка жанру, а не
конкретної гри. Тут референс — головне джерело:

- Бери реалізацію з референсу **за основу** як доведено робочий
  механізм: вивчи алгоритм (цикл інтегрування руху, попіксельний
  hit-test по ландшафту, маски колізій, систему сил) і **адаптуй** під
  нашу гру.
- **Без прямого копіпасту** і без порядкового порту з мови референсу в
  C#. Реалізуй з розуміння, власною структурою класів, спираючись на
  фізику/математику Unity.
- Числові константи (урон, коефіцієнти відскоку тощо) не переносити
  гуртом — підбирати свої під наш геймфіл.

## Архітектура та структура — переймати з адаптацією

Досвід референсу щодо архітектури, структури файлів і розподілу
відповідальності можна переймати, але:

- Адаптувати під **нашу** гру та її шари (див. «Архітектурні принципи»).
- **Модернізувати під сучасні практики Unity станом на 2026** — не
  копіювати легасі-підхід старого стека 1:1. Де в референсі власний
  движок чи ручні рішення — використовувати штатні можливості Unity
  (2D, URP, Input System, ScriptableObject-конфіги).
- Брати *ідею* організації (розбиття на снаряди/зброю/фізику/сцену,
  data-driven конфіги зброї), а не конкретні файли.

## Межі IP та копіювання

Дозволено ретельно аналізувати референс і спиратись на нього для
пришвидшення розробки, але:

- жодного прямого копіпасту коду й порядкового порту — лише реалізація
  з розуміння, адаптована під наш проект;
- жодних чужих назв (класів, зброї, персонажів), ассетів, текстів;
- копірайт захищає вираження (код, арт, звук, назви), а не ідеї та
  механіки жанру — спираємось на друге, не на перше.

## Архітектурні принципи

- Тримати gameplay, UI, data, SDK та platform integrations окремими шарами.
- Обгортати сторонні SDK власними інтерфейсами сервісів.
- Для ранньої розробки використовувати Unity 2D, URP, Input System, ScriptableObject-конфіги та просту composition root.
- Прогресію, економіку, магазин, рекламу, IAP, remote config і live-ops додавати шарами.
- Не робити платну або VIP-перевагу в ranked/competitive combat.
- **Жодного хардкоду гравцеського тексту в C#** — без винятків для «старого коду» чи «поступової міграції». Кожен рядок, який бачить гравець, проходить через `GameText`/локалізацію; рядок-літерал у коді допустимий лише як fallback-параметр цього виклику. Деталі й перевірка — `docs/architecture/localization.md`.

## Правила співпраці

- Перед великими змінами читати `CLAUDE.md` і релевантні файли в `docs/`.
- Перед широкими змінами core systems, data models, SDK integrations або folder structure спочатку запропонувати архітектуру.
- Важливі рішення документувати в `docs/architecture/` або `docs/design/`.
- Файли в `Examples/` можна читати й аналізувати, але не змінювати без прямого прохання.
- Після імплементації конкретних кроків\етапів\фічів з документу - позначати таск як виконаний.

## Сувора заборона
Заборона на будь які згадки щодо використання прикладів з назвами ігор у документації, змінах, коментарях, комітах і так далі

<!-- USER AIOS BOOTSTRAP BEGIN (user-owned — not managed by AIOS) -->
## AIOS bootstrap

This repo runs on AIOS. Codex is the primary provider (personal project);
Claude is a fallback used only once Codex limits are exhausted or on
explicit request. Gemini / OpenCode / Grok are not part of this workflow.

- **Memory first.** Before broad repository reading, run 1–4 narrow
  searches:
  `node "C:/Users/Yurii/.rexcil/aios/scripts/aios.mjs" search "<q>" --source memory --json`
- **Codemap before a broad scan.** Structure questions go to
  `code-review-graph`, then targeted reads, then a wide scan only if
  genuinely needed.
- **Classify non-trivial work** as direct / guarded / planned before
  executing.
- **Model tier is automatic.** `.aios/model-registry.json` (project-local)
  makes `aios team`/`aios harness`/`aios model-router route` resolve to the
  Codex ladder (sol/terra/luna/5.5) here, and to the Claude ladder in
  work projects — same AIOS install, opposite defaults per project.
- **Persist durable knowledge and unfinished state** via
  `aios memo add` using the `ARCH:` / `ADR:` / `SYSTEM:` /
  `CONVENTION:` / `WORKFLOW:` / `ACTIVE:` prefixes.

Detailed rules: `.claude/AIOS_PROJECT_WORKFLOW.md` — load it on demand
for non-trivial or delegated work, not for small tasks.
<!-- USER AIOS BOOTSTRAP END -->

<!-- AIOS NATIVE BEGIN -->
AIOS native enhancements are active in this repository. This is the shared
workflow core for every supported coding client.

## AIOS Workflow Policy

Classify the work item before selecting a plan, skill, or delegation route:

- `direct`: answer, inspect, or report status without a persistent plan or skill chain.
- `guarded`: make one clear, reversible project-local change behind
  `pre-edit-safety-gate` and focused verification.
- `planned`: for unclear, multi-step, risky, or delegated work, create or reuse
  one work item and execute only the Provider selected by the current Rex
  Capability Command.

Do not turn a new objective into a continuation merely because it follows an
earlier task. Do not inject a fixed skill chain at startup.

## Rex Ownership and Safety

- The current Rex Capability Command is the only authority that selects a
  software Provider or advances a software stage. Return the Command's required
  evidence before asking Rex to advance.
- `direct` does not invoke a Provider. `guarded` and `planned` run only the
  Provider named by the current Command.
- Before a cohesive code, workflow, or migration batch, use
  `pre-edit-safety-gate`. Before claiming changed behavior is complete, use
  `verification-before-completion` and preserve concrete evidence.
- Proceed autonomously only with read-only inspection and reversible
  project-local work. Ask before destructive or hard-to-reverse operations,
  external side effects, publication, credential or permission changes, costs,
  or material scope expansion.

## Context and Privacy

Runtime context is pull-based: do not inject session history, handoffs, memory
packets, personas, or router guides into ordinary startup prompts. Load only the
specific state required by an explicit continuation or current task.

Treat prompts, code, logs, screenshots, tool output, and browser data as
potentially outbound. Never expose secrets, credentials, cookies, private keys,
customer data, or unredacted authorization logs. Warn before using a custom
model endpoint or relay, and share only redacted sensitive data.

## On-Demand Routes

Load detailed instructions only when the current task needs them:

- Workflow command and selected Provider: `rex-workflow` and the Provider skill
  named by Rex.
- Context recall and memory lifecycle: `contextdb-autopilot` or
  `aios-offload-recall`.
- Structural code exploration and impact: `aios-codemap-ops`.
- Browser safety and interaction: `skill-constraints` plus the available browser
  MCP surface.
- Planned long-running or delegated work: `aios-long-running-harness`; use
  `model-router` only when that route selects model dispatch.
- Local token-compression setup or diagnostics: `aios-interception-runtime`.

Client overlays may describe only verified native capabilities. They must not
change this workflow, claim unsupported hooks, or imply that every task uses a
team, harness, browser, or model-routing path.

## AIOS Native Claude Layer

- Prefer repo-local `.claude/skills` and `.claude/agents`.
- SessionStart is read-only status output; it must not create a plan or inject a workflow chain.
- UserPromptSubmit calls the workflow-policy adapter. It decides `direct`, `guarded`, or `planned` before a plan or skill is selected.
- Keep work grounded in the AIOS runtime and verification flow after that decision.
<!-- AIOS NATIVE END -->

<!-- AIOS CODEMAP BEGIN -->
## MCP Tools: code-review-graph

This project exposes a structural knowledge graph via the `code-review-graph` MCP. Use it only when structural relationships materially affect the current decision; do not turn routine work into a graph-tool loop.

### Bounded checkpoints

- Initial orientation: call `get_minimal_context(task="...")` at most once when repository structure is not already clear.
- Before a risky or multi-file change: use `get_impact_radius(detail_level="minimal")`; call `query_graph(pattern="tests_for", target="...")` only for the concrete target being changed.
- After edits: call `detect_changes(detail_level="minimal")` once when the graph was used or the change has meaningful blast radius.
- Before submitting: use `get_affected_flows()` or `get_suggested_questions()` only if unresolved structural risk remains.
- Finding code: prefer `semantic_search_nodes` when semantic graph search is likely to beat a direct repository search.
- Budget: no more than three graph calls per work item. Treat `next_tool_suggestions` as optional hints and never follow them recursively.

### Planning context proposals

When an active structured-plan task has implementation targets, call AIOS MCP `aios_plan_task` with `action="propose_context"`, the task id, and workspace-relative targets when the task has none. The tool derives target, caller, callee, and test candidates from codemap, but it does not modify the active plan. Present the candidate refs to a human. An explicit human-controlled CLI confirmation with `aios plan task <id> --confirm-context-candidates` (optionally repeated `--candidate-ref <ref>`) activates selected refs for orchestration; it is a process boundary, not an identity/authentication boundary. Do not claim context will be delivered before that command succeeds.
<!-- AIOS CODEMAP END -->

### Localization Integrity Gate

Any localization edit is incomplete until all 13 JSON dictionaries parse as strict UTF-8, have an identical flattened key set, and contain no empty player-facing value. For Ukrainian, reject replacement characters and literal `?` loss markers. These checks must be automated in `LocalizationCoverageTests` and run before handoff.

Before every commit, the versioned `.githooks/pre-commit` hook must run `node tools/localization-integrity-gate.mjs --staged`; bypassing the hook is not permitted. The hook rejects invalid or divergent dictionaries and staged player text that bypasses localization.
