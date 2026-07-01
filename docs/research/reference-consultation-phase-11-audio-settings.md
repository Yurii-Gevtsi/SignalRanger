# Reference Consultation: Phase 11 Audio And Settings

Feature:
: Екран налаштувань із гучністю музики/звуків, збереженою у профіль, і сервіс аудіо за інтерфейсом.

Example signals:
: Приклади показують, що налаштування зазвичай тримають прості перемикачі гучності/мови окремо від бойового екрана, а звук іде через власний менеджер. Для MVP корисний принцип — сервіс аудіо за інтерфейсом і збереження гучності у save, а не конкретні шини, мікшери чи набори звуків.

Useful principles:
: Гучність — це стан профілю (save), а не UI. Геймплей просить звук через інтерфейс сервісу, тож можна почати з no-op реалізації й додати реальні кліпи пізніше без змін у виклику. Контроли мають бути великими touch-таргетами.

Risks to avoid:
: Не копіювати чужі звуки, назви шин, мікшери чи layout налаштувань. Не тягнути повну локалізацію чи багато мов у MVP-зріз. Не робити дрібні контроли.

Our original implementation:
: `SettingsScreenController` — модальний екран із меню зі степер-контролами (`-`/`+`, крок 0.1) для музики та звуків і візуальними fill-барами. Зберігає через `IAudioService.SetVolumes` + save; аудіо-сервіс читає гучність із профілю. Мова показана як «Українська» (повна локалізація — окремий шар). Реалізація аудіо поки no-op за інтерфейсом, готова під реальні кліпи.

## Update: Battle feedback → audio hooks (Phase 11.1/11.2)

Feature:
: Бойові події (приціл, постріл, вибух, поразка юніта, кінець бою) просять SFX через сервіс.

Useful principles:
: Feedback — це keyed-події, а gameplay не має знати ні конкретних кліпів, ні аудіо-збірки. Один map «ключ→sfx id» тримати в app-шарі; hooks існують навіть коли аудіо ще no-op.

Risks to avoid:
: Не прив'язувати gameplay-логіку до наявності звуку/VFX. Не копіювати чужі звукові набори.

Our original implementation:
: Core-міст `BattleFeedbackBridge` + `IBattleFeedbackSink`; app-реалізація `AppBattleFeedbackSink` мапить ключ у `sfx.battle.<key>` і кличе `IAudioService.PlaySfx`. `BattleSceneController` емітить `aim/shot/impact/defeat/finished` через `BattleFeedbackEventType`. Gameplay бачить лише Core-контракт. Покрито edit-mode тестом мапінгу ключ→sfx.
