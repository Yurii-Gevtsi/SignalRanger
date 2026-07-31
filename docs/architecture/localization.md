# Локалізація

## Джерело перекладів

Для кожної мови існує один UTF-8 JSON-словник у
`Assets/_Game/Content/Resources/Localization/`:

```text
uk.json
en.json
pl.json
de.json
es.json
fr.json
it.json
pt-BR.json
ja.json
ko.json
tr.json
ar.json
zh-Hans.json
```

JSON групує рядки за контекстом (`ui`, `intro`, `battle`, `content`), а
код звертається до стабільних ключів з крапкою: `ui.play`,
`intro.sigma.body`, `battle.ammo`.

Додати мову означає скопіювати `uk.json`, назвати файл кодом локалі
(`pl.json`, `de.json`) і перекласти тільки значення. Ключі не можна
видаляти або перейменовувати без зміни коду.

## Runtime

`JsonLocalizationService` завантажує словники через Resources, зберігає
вибраний код мови у `SaveSettingsData.languageCode` і використовує `uk`
як fallback, якщо переклад або мова ще відсутні.

Для параметризованих рядків використовуються іменовані маркери:

```json
"ammo": "Боєприпаси: {count}"
```

```csharp
AppServices.Localization.Translate("battle.ammo", new LocalizationArgument("count", 3));
```

Нові екрани не повинні містити користувацький текст прямо в C# — тільки
ключі локалізації. Існуючий `GameText` переноситься поступово, щоб не
змішувати локалізацію з великими gameplay-змінами.
