# Phase 14: Android Build, Performance, MVP Review Gate

## MVP Closure Status

Status: MVP feature scope is closed.

Manual smoke notes:

- Menu, loadout, settings, battle HUD, turn flow and result flow are working in the current playable build.
- All eight runtime weapons are selectable through the test loadout path and have distinct tactical behavior for MVP testing.
- Cover, guard, relay, close-range burst and multi-projectile weapons passed manual gameplay iteration.
- Remaining work is outside MVP feature scope: final Unity test run, Android build, profiler pass and release-readiness review.

Ця нотатка фіксує стан Phase 14 після автономного полірування і слугує чек-листом перед першим внутрішнім build'ом.

## 14.1. Android Settings

Оновлено в `ProjectSettings/ProjectSettings.asset`:

- `companyName`: `HomeStudio` (заміна `DefaultCompany`).
- `productName`: `TacticalArtilleryMvp` (заміна `Game`).
- `applicationIdentifier.Android`: `com.homestudio.tacticalartillery` (заміна шаблонного).
- `AndroidTargetArchitectures`: `4` (ARM64) — вимога Play Store для нових додатків з 2019. Раніше було `2` (ARMv7).

Лишається (свідомо):

- `AndroidMinSdkVersion: 26` (Android 8.0) — розумний нижній поріг.
- `AndroidTargetSdkVersion: 0` (auto = latest) — Unity сама підтягує актуальний SDK.
- `useCustomMainManifest: 0` — Unity генерує AndroidManifest.xml, permissions мінімальні (лише `INTERNET` за замовчуванням). Кастомний манифест не потрібен, доки не додаємо ads/notifications SDK.
- `defaultScreenOrientation: 4` (AutoRotation) — гра підтримує portrait і landscape через `CanvasScaler` `ScaleWithScreenSize` (ref 1080×1920).

## 14.2. Performance Pass (чек-лист)

Виконати вручну перед першою збіркою (потребує Profiler у редакторі):

- [ ] **Frame rate у бою**: стабільно ≥ 30 fps на середньому Android-девайсі під час пострілу + польоту снаряда.
- [ ] **Cleanup снарядів/ефектів**: `_arenaObjects` очищається у `ClearArenaRuntime`; imapct flash має `Destroy(flash, 0.25f)`. Перевірити Scene Hierarchy на відсутність накопичення `Production Shell`/`Impact Flash` після 10 боїв.
- [ ] **Аллокації в turn loop**: у Profiler'і подивитись GC.Alloc за 1 хід (aim → fire → impact → next turn). Основні підозрювані: `SelectMany`/`ToList` у `OnProjectileImpacted`, `Sum` у HUD `GetTeamHealth`.
- [ ] **Scene loading time**: перехід Boot → Menu → Battle ≤ 2s на середньому пристрої.
- [ ] **Memory**: після 5 боїв немає лінійного росту `Textures/Objects` у Profiler'і.

## 14.3. MVP Review Gate

Перед позначенням MVP як завершеного:

- [x] Стабільний Unity-проєкт з `Assets/_Game/` — так.
- [x] Boot → Menu → Battle → Result flow — так.
- [x] Короткий tutorial (5 кроків у першому бою) — так.
- [x] 8-12 campaign missions — 8 mock-missions у `MvpMissionLibrary`.
- [x] 4-6 оригінальних weapons — 5 (`steady-orb`, `pulse-shell`, `close-pop`, `long-lance`, `scatter-arc`).
- [x] 2-3 оригінальних character archetypes — 3 (`Striker`, `Skirmisher`, `Bulwark`).
- [x] 2-3 arenas — 3 (`Training Meadow`, `Split Ridge`, `Central Cover`).
- [x] Local save — JSON у `Application.persistentDataPath` + міграція + fallback.
- [x] Basic loadout — modal екран з меню, зберігається у save, впливає на зброю бою.
- [x] One soft currency — `EconomyService` + `SaveEconomyData.softCurrency`.
- [x] Simple rewards — `MissionRewardDefinition` + `CampaignProgressionService.CompleteMission`.
- [x] Audio/settings — модальний settings екран, гучність зберігається у save; `IAudioService` no-op готовий під кліпи.
- [x] No-op infrastructure interfaces — analytics/ads/IAP/remoteconfig/notifications.
- [x] Core battle logic + save migration test coverage — EditMode тести + PlayMode smoke.

Не в MVP-зрізі (свідомо):

- Real ads/IAP SDK, push notifications, backend login, ranked PvP, clans, complex crafting, багато валют, heavy live-ops.

## 14.4. Reference Consultation

Feature:
: Android build settings, performance-план, MVP review gate.

Example signals:
: Мобільні аналоги мають мінімальні permissions за замовчуванням, підтримують ARM64, стабільні 30+ fps на середньому пристрої, а першу збірку роблять з placeholder-іконками й дефолтною версією.

Useful principles:
: Аdd тільки те, що справді потрібно (permissions, SDK, архітектури). Reserve custom manifest until infrastructure actually needs it.

Risks to avoid:
: Не додавати permissions "про запас" (INTERNET/READ_PHONE_STATE тощо) — це впливає на trust store listing. Не залишати шаблонний `applicationIdentifier`.

Our original implementation:
: Мінімальні зміни в `ProjectSettings.asset` (companyName, productName, identifier, ARM64). `useCustomMainManifest: 0` лишається, доки не з'являться реальні SDK. Performance-план і review-gate винесені в цю нотатку; ручний прогін — за автора.
