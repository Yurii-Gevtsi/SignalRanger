# Інструкція з налаштування середовища розробника

Цей документ призначений для нового розробника або coding-agent, який отримав доступ до репозиторію і має зібрати середовище з нуля.

Перевірено за станом репозиторію на **2026-09-02**. Джерело істини для версій: `ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`, `Packages/packages-lock.json`, `ProjectSettings/` та `Assets/`. Якщо документ розійшовся з ними, спочатку звіряйся з файлами проєкту.

> Цільове середовище — Windows 10/11. Для macOS/Linux логіка та сама, але шляхи Unity, Android SDK і shell-команди можуть відрізнятися.

## 1. Результат налаштування

Після виконання мають працювати:

- відкриття проєкту саме в Unity `6000.5.1f1`;
- автоматичне відновлення UPM-залежностей із manifest/lock-файлів;
- компіляція runtime, editor і test assemblies;
- EditMode tests та PlayMode smoke tests, якщо доступне graphics-середовище;
- Android build environment через Unity Hub modules;
- вбудовані Google Play Games, Google Mobile Ads та External Dependency Manager for Unity;
- офлайн-режим без UGS-акаунта;
- UGS environment `development`, якщо видано cloud-доступ;
- Unity MCP та AIOS/CRG workflow, якщо це потрібно агенту.

Не створюй вручну `.csproj`, `.sln`, `Library/`, `Temp/` або `Logs/`. Unity генерує solution-файли, а Package Manager відновлює пакети.

## 2. Що встановити

### Обов’язково

1. Git із доступом до репозиторію.
2. Git LFS: бінарні ассети позначені LFS у `.gitattributes`.
3. Unity Hub.
4. Unity Editor `6000.5.1f1`, revision `0d9463e84828`.
5. Node.js LTS для `.mjs`-перевірок.
6. Rider або Visual Studio; відповідні Unity integration packages уже є в manifest.

### Android

У Unity Hub для `6000.5.1f1` встанови Android Build Support, Android SDK & NDK Tools і OpenJDK. Не підміняй bundled SDK/NDK/JDK системними версіями без окремої причини.

### iOS та опційні інструменти

Повний iOS build потребує macOS, Xcode і tooling для CocoaPods/Swift Packages. Python 3 потрібен лише для art/arena scripts у `tools/arena-assets/`. Android Studio корисний для logcat/emulator, але не замінює Unity Hub modules. Unity Cloud і AIOS потрібні лише відповідним workflow.

Офіційні посилання: [Unity Hub та Editor](https://docs.unity3d.com/Manual/GettingStartedInstallingUnity.html), [Android dependencies](https://docs.unity3d.com/Manual/android-install-dependencies.html), [Authentication](https://docs.unity.com/en-us/authentication/get-started), [Cloud Save](https://docs.unity.com/en-us/cloud-save/get-started).

## 3. Clone і Git LFS

Для нового checkout виконай:

    git lfs install
    git clone <REPOSITORY_URL> Game
    Set-Location .\Game
    git lfs pull
    git status --short

Для існуючого checkout:

    Set-Location C:\Projects\Game
    git fetch --all --prune
    git lfs install
    git lfs pull
    git status --short

Не використовуй `git reset --hard`, `git clean -fd` або інше відкидання локальної роботи без дозволу власника. Перевір LFS:

    git lfs env
    git lfs ls-files | Select-Object -First 20

Якщо PNG/аудіо є маленькими текстовими pointer-файлами, повтори `git lfs pull` і перевір доступ до LFS remote.

## 4. Unity Hub і перший запуск

1. Увійди в Unity Hub під Unity ID.
2. **Installs → Add → Install Editor version**.
3. Встанови точну версію **Unity 6 `6000.5.1f1`**, не latest.
4. Додай Android modules, якщо потрібен Android.
5. **Open → Add project from disk** і вибери корінь із `Assets/`, `Packages/`, `ProjectSettings/`.
6. Не погоджуйся на upgrade/downgrade project version.
7. Звір `ProjectSettings/ProjectVersion.txt`: `m_EditorVersion: 6000.5.1f1` та `m_EditorVersionWithRevision: 6000.5.1f1 (0d9463e84828)`.

Перший запуск відновлює `Library/`, імпортує ассети, shader cache та генерує `.sln`/`.csproj`; це локальні похідні файли.

## 5. Пакети

Package Manager має завершити resolve без errors. Прямі залежності: `Packages/manifest.json`; точні resolutions і hashes: `Packages/packages-lock.json`.

| Пакет | Версія | Роль |
|---|---:|---|
| `com.unity.render-pipelines.universal` | `17.5.0` | URP/2D |
| `com.unity.inputsystem` | `1.19.0` | Input System |
| `com.unity.ai.navigation` | `2.0.13` | navigation |
| `com.unity.services.core` | `1.18.0` | UGS core |
| `com.unity.services.authentication` | `3.7.3` | auth |
| `com.unity.services.cloudsave` | `3.4.1` | cloud save |
| `com.unity.remote-config` | `4.2.5` | remote config |
| `com.unity.services.multiplayer` | `2.3.0` | Sessions/multiplayer |
| `com.unity.netcode.gameobjects` | `2.13.1` | Netcode |
| `com.unity.multiplayer.playmode` | `2.0.2` | virtual players |
| `com.unity.multiplayer.center` | `1.0.1` | multiplayer guidance |
| `com.unity.timeline` | `1.8.12` | Timeline |
| `com.unity.visualscripting` | `1.9.11` | Visual Scripting |
| `com.unity.test-framework` | `1.7.0` | tests |
| `com.unity.textmeshpro` | `5.0.0` | text |
| `com.unity.ugui` | `2.5.0` | uGUI |
| `com.unity.ide.rider` | `3.0.38` | Rider |
| `com.unity.ide.visualstudio` | `2.0.26` | Visual Studio |
| `com.unity.ai.assistant` | `2.13.0-pre.2` | Unity AI |
| `com.unity.ai.inference` | `2.6.1` | inference |
| `com.unity.collab-proxy` | `2.12.4` | collaboration |
| `com.unity.device-simulator.devices` | `1.0.1` | simulator data |
| `com.unity.pipeline` | `0.5.0-exp.1` | pipeline tooling |
| `com.coplaydev.unity-mcp` | git tag `v10.0.0` | Unity MCP |

Усі `com.unity.modules.*` — bundled Unity modules; окремо їх не встановлюй. Не редагуй lock-файл вручну без навмисної зміни package graph.

## 6. Embedded plugins

Ці компоненти вже входять у checkout. Не імпортуй другу копію через Asset Store, `.unitypackage`, OpenUPM або інший URL:

| Шлях | Призначення |
|---|---|
| `Assets/GooglePlayGames/` | Google Play Games plugin, package version `2.1.0` |
| `Assets/GoogleMobileAds/` | Google Mobile Ads runtime/editor assemblies |
| `Assets/ExternalDependencyManager/` | Android/iOS/Package Manager Resolver |
| `Assets/Plugins/Android/` | Android manifests, Gradle templates, libraries |
| `Assets/Plugins/iOS/` | iOS native plugin material |
| `Assets/_Game/` | власний runtime, UI, data, infrastructure, tests |

Після зміни плагінів або Android bundle identifier вибери Android у **File → Build Profiles**, потім **Assets → External Dependency Manager → Android Resolver → Resolve**. `Force Resolve` застосовуй лише за потреби; випадкові `.aar`/`.jar` не видаляй.

## 7. Project Settings

Перевір у **Edit → Project Settings**: product `Signal Ranger`; Android identifier `com.gysignalstudio.signalranger`; Android minimum SDK `26`; ARM64; Linear color space; Universal Render Pipeline; новий Input System.

Keystore та passwords не копіюй із чужого ПК. Це локальні секрети.

## 8. UGS development setup

UGS не потрібен для першого offline-запуску. Якщо є доступ:

1. Відкрий [Unity Dashboard](https://cloud.unity.com/).
2. Вибери потрібну Organization і UGS Project.
3. У Unity: **Edit → Project Settings → Services → Link**.
4. Створи environment `development`.
5. Увімкни Authentication, Cloud Save та Remote Config.
6. Експерименти виконуй у `development`, не в `production`.

Поточний код має UGS adapters, anonymous auth path, Cloud Save key `save`, Remote Config через `IRemoteConfigService` і offline/no-op implementations. Без Project ID або мережі гра має працювати офлайн з warning.

Не коміть service-account keys, OAuth secrets, Android Publisher keys, keystores, passwords, access tokens або unredacted CI logs. Зберігай їх у secret store CI або поза repo. Повні online кроки: [UGS setup](ugs-setup.md).

## 9. Node tools та локалізація

    node --version
    npm --version
    node tools/localization-audit.mjs --strict
    node tools/localization-integrity-gate.mjs

Обов’язковий `npm install` для Unity відсутній: repo-local `.mjs` tools використовують Node built-ins, якщо інше не вказано tool.

Зміна player-facing text вимагає агента `localization-translator`, одночасної зміни всіх мовних словників, strict audit та integrity gate. Placeholder-переклади і hardcoded fallback у C# заборонені.

## 10. Редактор і тести

У Unity вибери **Edit → Preferences → External Tools** і Rider або Visual Studio. За потреби натисни **Regenerate project files**; `.sln`/`.csproj` не коміть.

Через Editor: **Window → General → Test Runner**, спочатку EditMode, потім PlayMode.

Batchmode (закрий Editor цього проєкту), приклад для PowerShell:

    $unity = 'C:\Program Files\Unity\Hub\Editor\6000.5.1f1\Editor\Unity.exe'
    $project = (Get-Location).Path
    & $unity -batchmode -nographics -quit -projectPath $project -runTests -testPlatform EditMode -testResults Temp\editmode-results.xml -logFile Temp\editmode.log

Для PlayMode використовуй той самий запуск із `-testPlatform PlayMode` і `Temp\playmode-results.xml`; graphics/display restrictions можуть вимагати запуск без `-nographics`.

Успіх — exit code `0` і відсутні failed tests у XML. Сам факт відкриття Editor не є доказом.

## 11. Unity MCP і агент

`.mcp.json` описує `code-review-graph` через `uvx code-review-graph serve` та `UnityMCP` на `http://localhost:8080/mcp`. Пакет Unity MCP — git tag `v10.0.0`.

Перед MCP automation відкрий Unity `6000.5.1f1`, дочекайся compile/import, запусти bridge/server і перевір endpoint. Якщо MCP недоступний, агент працює через shell і Test Runner та не вигадує MCP evidence.

AIOS-related files: `.aios/`, `.claude/`, `.codex/agents/`, `.mcp.json`, `AGENTS.md`, `CLAUDE.md`. Глобальні AIOS paths відрізняються між машинами; не перенось hardcoded `C:\Users\...` у код.

Обов’язкові правила агента: читати `CLAUDE.md` і релевантні docs; перед широкими core/data/SDK/folder changes описати архітектуру; не змінювати `Examples/`; виконувати safety gate, focused verification і review diff; не зберігати secrets/session caches.

## 12. Навігація

| Питання | Де дивитися |
|---|---|
| Загальні правила | `AGENTS.md`, `CLAUDE.md` |
| Unity/package versions | `ProjectSettings/ProjectVersion.txt`, `Packages/` |
| UGS | `docs/setup/ugs-setup.md`, `docs/architecture/backend-online-services.md` |
| Архітектура | `docs/architecture/target-architecture.md` |
| Локалізація | `docs/architecture/localization.md` |
| Arena integration | `docs/design/arena-unity-integration-instructions.md` |
| Art pipeline | `docs/design/unity-2d-art-pipeline.md` |
| Runtime | `Assets/_Game/Runtime/` |
| Tests | `Assets/_Game/Tests/EditMode/`, `Assets/_Game/Tests/PlayMode/` |
| Scripts | `tools/` |

## 13. Smoke checklist

- [ ] чистий `git status` після clone;
- [ ] `git lfs pull` і реальні binary assets;
- [ ] Editor `6000.5.1f1` та Android modules за потреби;
- [ ] package resolve без errors;
- [ ] Console без compile errors;
- [ ] гра запускається offline;
- [ ] localization audit і integrity gate проходять;
- [ ] EditMode tests проходять;
- [ ] PlayMode status перевірено або blocker записано;
- [ ] UGS link зроблено лише з правильним Project access;
- [ ] secrets/signing keys не в git;
- [ ] MCP endpoint доступний, якщо потрібна automation.

## 14. Типові проблеми

- **Версія Unity:** звір `ProjectVersion.txt`; не роби upgrade без узгодження.
- **Package Manager:** перевір мережу, Git і доступ до Unity MCP git dependency; не видаляй lock-файл навмання.
- **Рожеві матеріали:** перевір Unity `6000.5.1f1`, URP `17.5.0` і graphics settings.
- **Android duplicate classes/AAR:** спочатку EDM4U Resolve/Force Resolve, не додавай ручні копії libraries.
- **UGS:** перевір Services link, Project ID, environment `development`; offline path має залишитися робочим.
- **MCP:** перевір відкритий Unity, завершений compile/import і порт `8080`.
- **LFS:** повтори `git lfs pull`; не замінюй pointer-файли generated copies.

## 15. Перед commit

    git status --short
    git diff --check
    node tools/localization-integrity-gate.mjs --staged

Не додавай `Library/`, `Temp/`, `Logs/`, `UserSettings/`, `.idea/`, generated solutions, білди, secrets або AIOS session caches. Не обходь pre-commit hook через `--no-verify`.

Після налаштування handoff має містити: версію Unity, package resolve status, localization gate results, EditMode/PlayMode status, platform modules, MCP status і blockers.
