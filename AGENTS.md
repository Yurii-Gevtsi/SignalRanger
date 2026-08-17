# Правила проєкту для Codex
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

## Правила співпраці

- Перед великими змінами читати `CLAUDE.md` і релевантні файли в `docs/`.
- Перед імпортом або інтеграцією модульної руйнівної арени читати `docs/design/arena-unity-integration-instructions.md`.
- Для всіх нових згенерованих raster-ассетів обов'язково вимагати PNG зі справжнім alpha-каналом і перевіряти, що фон має alpha = 0. Маджента, шахова підкладка або інший запечений фон не є штатним форматом. Вони допускаються лише як аварійний fallback, після чого в окремому generated output треба автоматично побудувати чистий RGBA та перевірити його на темному, світлому й кольоровому фоні за правилами integration-документа.
- Перед широкими змінами core systems, data models, SDK integrations або folder structure спочатку запропонувати архітектуру.
- Важливі рішення документувати в `docs/architecture/` або `docs/design/`.
- Файли в `Examples/` можна читати й аналізувати, але не змінювати без прямого прохання.
- Після імплементації конкретних кроків\етапів\фічів з документу - позначати таск як виконаний.
- Після будь-якого редагування файлів із кирилицею обов’язково перевіряти кодування через UTF-8/байтовий інструмент (наприклад Python `Path.read_text(encoding="utf-8")`) і пошук mojibake-патернів (`Р’`, `Рџ`, `Рґ`, `Г—`, `Ð`, `Ñ`, `â`). Не довіряти виводу PowerShell `Get-Content` як єдиній перевірці, бо консоль може некоректно відрендерити нормальний UTF-8.
- Будь-яка зміна, видалення або додавання тексту в локалізаційному словнику має одночасно виконуватися для всіх наявних мов; перед завершенням обов’язково перевіряти однакові ключі та валідність усіх JSON-словників.

## Сувора заборона
Заборона на будь які згадки щодо використання прикладів з назвами ігор у документації, змінах, коментарях, комітах і так далі

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
