# UGS: покрокове налаштування

Практична інструкція до `docs/architecture/backend-online-services.md`.
Виконується на етапі 1 (фундамент онлайну); секції IAP/Economy — на етапі 1.5,
PvP — на етапі 2.

## 1. Unity Cloud проєкт

1. [cloud.unity.com](https://cloud.unity.com) → створити організацію (якщо нема) і проєкт.
2. В Unity Editor: **Project Settings → Services** → залінкувати проєкт
   (з'явиться Project ID — він публічний, у репозиторії йому бути можна).
3. **Dashboard → Projects → Environments**: створити environment `development`
   (дефолтний `production` вже є). Уся розробка і тести — тільки в `development`.
4. **Організація → Billing**: додати платіжну картку (без неї перевищення
   free tier може загейтити API) і ввімкнути алерти free tier (75% / 100%).

## 2. Пакети в Unity

Через Package Manager (етап 1):

- `com.unity.services.core` — базова ініціалізація
- `com.unity.services.authentication`
- `com.unity.services.cloudsave`
- `com.unity.remote-config`
- `com.unity.services.deployment` — деплой конфігів із репозиторію

Етап 1.5: `com.unity.purchasing` (Unity IAP), `com.unity.services.economy`,
`com.unity.services.cloudcode`.

Етап 2: `com.unity.services.lobby`, `com.unity.services.relay`,
`com.unity.netcode.gameobjects`, (опц.) `com.unity.services.matchmaker`.

## 3. Ініціалізація в коді

У `GameBootstrapper`, до будь-якого використання сервісів:

```csharp
var options = new InitializationOptions();
options.SetEnvironmentName(BuildConfig.IsRelease ? "production" : "development");
await UnityServices.InitializeAsync(options);
```

Потім автентифікація (див. нижче), потім усі інші UGS-виклики.
Помилка ініціалізації/відсутність мережі → composition root збирає
no-op реалізації, гра працює офлайн.

## 4. Authentication

### 4.1 Анонімний вхід (база, працює одразу)

Dashboard → **Authentication** → увімкнути. У коді:

```csharp
await AuthenticationService.Instance.SignInAnonymouslyAsync();
```

Session token кешується на пристрої — повторні запуски повертають
того самого гравця без жодних дій. Нічого більше вмикати не треба.

### 4.2 Автоматичний вхід Google Play Games (GPGS v2)

Це той самий «гра сама прив'язала мій Google Play акаунт при першому
запуску». Auto sign-in робить сам GPGS SDK при старті гри; наш код лише
підхоплює результат і обмінює його на UGS-сесію.

**Google Play Console:**

1. Створити застосунок (якщо ще нема) → **Grow → Play Games Services →
   Setup and management → Configuration** → створити проєкт PGS
   (він прив'яжеться до Google Cloud проєкту).
2. **Credentials**: додати два OAuth-клієнти в Google Cloud:
   - тип **Android** — package name + SHA-1 відбитки. Потрібні всі три:
     debug keystore, upload key і **Play App Signing key**
     (Play Console → Setup → App signing) — без останнього автовхід
     не працюватиме в білдах зі стору;
   - тип **Web application** — його **Client ID і Client Secret**
     потрібні для UGS (redirect URI не потрібен).
3. Додати себе в тестери PGS, поки застосунок не опубліковано.

**UGS Dashboard:**

**Authentication → Identity providers → Google Play Games** → вставити
Web Client ID + Client Secret.

**Unity:**

1. Встановити [Google Play Games plugin for Unity](https://github.com/playgameservices/play-games-plugin-for-unity) (v2, GPGS v11+).
2. **Window → Google Play Games → Setup → Android setup**: вставити
   resources XML із Play Console і Web Client ID.
3. Потік при старті:

```csharp
PlayGamesPlatform.Activate();
PlayGamesPlatform.Instance.Authenticate(async status =>
{
    if (status == SignInStatus.Success)
    {
        PlayGamesPlatform.Instance.RequestServerSideAccess(false, async authCode =>
        {
            await AuthenticationService.Instance
                .SignInWithGooglePlayGamesAsync(authCode);
        });
    }
    else
    {
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
});
```

`Authenticate` при запуску відпрацьовує тихо (без діалогів), якщо гравець
не вимкнув auto sign-in у застосунку Play Games. Для анонімних гравців
у налаштуваннях — кнопка прив'язки через
`LinkWithGooglePlayGamesAsync(authCode)`.

**Типові граблі:** автовхід «працює в editor/devbuild, не працює з Play
Market» — майже завжди відсутній SHA-1 ключа Play App Signing
у Android OAuth-клієнті.

## 5. Cloud Save

Dashboard → **Cloud Save** → увімкнути. Конфігурувати нічого не треба —
ключі створюються з коду. Домовленості:

- `save` — серіалізований `SaveData` (пише клієнт);
- protected-ключі (`premiumExpiresAt`, пізніше цінності) — пише **тільки
  Cloud Code** через Access Class `protected`; клієнт лише читає.

## 6. Remote Config

Dashboard → **Remote Config** → увімкнути. Конфіг тримати в репозиторії
(Deployment package створює `.rc`-файли) і деплоїти через
**Window → Deployment** або UGS CLI — не редагувати руками в дашборді.
Розділяти значення по environments (`development` для експериментів).

## 7. Economy (етап 1.5)

Dashboard → **Economy** → увімкнути. Ресурси описуються файлами
(`*.ecc` — currencies, `*.ecp` — purchases) в репозиторії і деплояться
через Deployment. Нарахування/списання з боку клієнта заборонити
дизайном: усі зміни балансу — через Cloud Code.

## 8. Cloud Code (етап 1.5)

Dashboard → **Cloud Code** → увімкнути. Використовувати **C# modules**
(не JS-скрипти) — спільна мова з проєктом, нормальні тести.
Модулі лежать у репозиторії, деплой через Deployment/UGS CLI.

Мінімальний набір функцій:

- `ValidatePurchase` — перевірка IAP-чека, видача товару/валюти;
- `GrantPremiumPass` — запис `premiumExpiresAt` (+30 днів або продовження);
- `GetServerTime` — серверний час для клієнта;
- `SubmitBattleResult` — нарахування нагород із sanity-перевірками.

## 9. Unity IAP (етап 1.5)

1. Play Console → **Monetize → Products**: створити consumable
   «Преміум-пропуск 30 днів» + інші товари.
2. Пакет `com.unity.purchasing`, каталог товарів — своїм конфігом
   за `IPurchaseService`.
3. Чек кожної покупки відправляти в `ValidatePurchase` (Cloud Code);
   клієнт нічого не видає сам. Для серверної перевірки в Google
   Cloud Code-модулю знадобиться Google service account із доступом
   Android Publisher API (ключ — тільки в секретах Cloud Code, не в репо).

## 10. Service Account (деплой/CI)

Dashboard → організація → **Service Accounts** → створити акаунт для CI
з ролями Deployment/Economy/Cloud Code/Remote Config editor.

**Key ID + Secret Key нікуди в репозиторій не комітити** — тільки
змінні середовища CI або локальний конфіг поза git
(додати відповідні шляхи в `.gitignore`). У самому клієнті гри
жодних секретів немає взагалі: клієнт користується лише Project ID
і токеном гравця з Authentication.

UGS CLI для деплою з CI:

```bash
ugs config set project-id <PROJECT_ID>
ugs config set environment development
# ключі — через змінні середовища UGS_CLI__SERVICE_KEY_ID / SECRET_KEY
ugs deploy ./UgsConfig
```

## 11. PvP-сервіси (етап 2)

Dashboard: увімкнути **Lobby** і **Relay** (працюють одразу, конфігури
мінімальні). **Matchmaker** — коли простий підбір через Lobby перестане
влаштовувати. Netcode бере Relay-транспорт
(`UnityTransport` + Relay allocation з коду).

## 12. Чекліст безпеки

- [ ] Секрети (service account keys, OAuth client secret, Android Publisher
      key) — ніколи в репозиторії
- [ ] Цінності пишуться тільки Cloud Code (protected keys / Economy)
- [ ] Таймери цінностей — тільки за серверним часом
- [ ] Уся розробка в environment `development`; `production` — тільки релізи
- [ ] Платіжна картка додана, алерти free tier увімкнені
- [ ] SHA-1 всіх трьох ключів (debug/upload/Play App Signing) в OAuth-клієнті
