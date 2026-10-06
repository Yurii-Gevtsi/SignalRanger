# Огляд змін зброї №7–10

Фіксована точка: `origin/Development` (`1c008b6a71d0f9468efbea8c95efb61af3ad7e83`). Перевірено `git diff origin/Development...HEAD` (непорожній), `git log origin/Development..HEAD --oneline` і незакомічений робочий diff цільових файлів. Через спільний незакомічений стан репозиторію висновки нижче обмежені №7–10, а не всіма файлами між гілками. Джерела вимог: `docs/design/weapon-combat-specification.md`, `docs/design/weapon-catalog-canonical.md`, `docs/architecture/weapons-7-10-test-contract.md`. Стандарти: `AGENTS.md`, `CLAUDE.md`.

## Standards

Жорстких порушень у цільових змінах не виявлено: файли `Examples/` не змінені цією роботою, текст гравця додано в усі 13 мов, strict audit і integrity gate проходять, документація читається UTF-8 без mojibake. `git diff --check` не вивів помилок. Переглянуто всі 12 ознак запаху коду: Mysterious Name, Duplicated Code, Feature Envy, Data Clumps, Primitive Obsession, Repeated Switches, Shotgun Surgery, Divergent Change, Speculative Generality, Message Chains, Middle Man, Refused Bequest. Для вузьких змін №7–10 нових блокувальних випадків не виявлено. Окреме винесення HP-балансу в чистий resolver доцільне під час введення `H_cap`, щоб не розширювати вже великий `BattleSimulation`; це рекомендація щодо майбутньої зміни, не дефект чинних чотирьох механік.

## Spec

- **Незавершена числова вимога, hard:** каталог §3.4 після уточнення 2026-09-28 вимагає `BaseDamage=roundHalfUp(H_cap(U)×p)` і окремої `CoreDelta=roundHalfUp(H_cap(L_purchase)×q)` на кожну покупку. `PrototypeWeaponLibrary.cs` досі задає прототипні абсолютні 65/54/35, а `WeaponUpgradeChoiceResolver.cs` множить базу на 1.5/3/5/10. `SaveWeaponUpgradeChoiceData` містить лише ранг зброї й шлях, без рівня покупки/дельти. Виправлення: затвердити та внести єдину криву `H_cap(1…60)`, відсотки `p/q`, зафіксувати абсолютні дельти в сейві, перевести runtime/UI й мігрувати старі записи. Доки це не зроблено, числову шкоду №7/8/10 не позначати остаточно збалансованою.

Інші вимоги карток №7–10 мають перевірку через публічну симуляцію та реальну сцену: 30/30 цільових EditMode і 10/10 PlayMode пройшли. Невдалі 2 з 139 у ширшій симуляції стосуються балки й мортири №12–13, тому не прикриваються цим висновком.

Висновок: Standards — 0 жорстких знахідок; Spec — 1 незавершена числова вимога. Бойові механіки №7–10 перевірені, глобальний HP-баланс лишається відкритим.
