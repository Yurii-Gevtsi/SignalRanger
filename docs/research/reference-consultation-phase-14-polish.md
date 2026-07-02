# Reference Consultation: Phase 14 Polish Closure

Feature:
: Закриття після-MVP polish-боргів: TMP/localized runtime UI, multi-projectile impact resolution і utility weapon roles.

Example signals:
: Reference packages показують три корисні патерни: 1) бойовий HUD і меню тримають короткі, повторно використовувані UI-рядки в одному місці; 2) multi-hit / cluster-подібні постріли завершують хід лише після завершення всіх impact-ів; 3) utility-інструменти на кшталт укриття, захисту чи швидкого repositioning додають тактичну глибину без роздування базової damage-моделі.

Useful principles:
: Переносити весь runtime UI на один текстовий каталог і один текстовий стек, а не змішувати legacy `Text` і TMP. Для weapon impact важливо пов'язувати damage/radius з конкретним projectile, а не з "активною зброєю сцени", інакше burst-ролі працюють некоректно. Utility-роли мають мутувати поле бою або стан юніта окремими правилами, а не маскуватися під звичайний explosive hit.

Risks to avoid:
: Не копіювати чужі weapon names, layouts, utility-ідеї чи геймплейні цифри буквально. Не ламати turn flow, завершуючи хід після першого impact у multi-projectile shot. Не тримати локалізовані рядки розкиданими по MonoBehaviour-ах, якщо UI вже будується в runtime.

Our original implementation:
: Додано `GameText` як єдину точку для runtime UI chrome. Menu/loadout/settings/battle HUD переведені на TMP. `ProjectileImpact` тепер несе `damage` і `impactRadius`; `BattleSceneController` трекає кількість projectile-ів у польоті, тож `BurstProjectileEffect` працює без дострокового завершення ходу. До `PrototypeWeaponLibrary` додано `weapon.rampart-kit`, `weapon.guard-pulse` і `weapon.relay-step`; після ручного тесту utility-ролі переведено з "projectile з іншим impact" у direct utility actions: cover ставиться в обрану точку, guard лікує активного союзника, relay переміщує власника до цільової позиції в межах радіуса. Це ближче до reference-принципу: інструменти повинні мати окремий control grammar, а не маскуватися під звичайну гармату.
