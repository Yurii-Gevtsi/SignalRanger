# Wormix — гайд для аналізу коду

**Це найцінніший референс з двох проєктів.** Гра — Worms-подібний
покроковий артилерійський бій із руйнівним ландшафтом.

## Технологія
- Adobe AIR / **ActionScript 3** (клієнт у `assets/wormix-mobile.swf`).
- Рендер: **Starling** (GPU 2D), UI: **Feathers**, скелетна анімація:
  **DragonBones**, партикли: **Stardust**, твіни: **GreenSock**.
- `classes*.dex` — лише нативна Android-обгортка (AIR runtime,
  Firebase, AppsFlyer, billing). Ігрової логіки там немає — не
  декомпілювалися.

## Де що лежить (декомпільований AS3)
Корінь: `_decompiled_swf/scripts/`
- `Application.as` — точка входу AIR.
- `ru/pragmatix/wormix/` — **увесь ігровий код (1210 файлів) з
  реальними іменами класів.** Головне:

| Пакет | Що всередині |
|---|---|
| `physics/` | `Physics.as`, `Force.as`, `ComponentForce.as`, `HitTest.as`, `CollideType.as`, `Joint.as` |
| `physics/model/` | моделі руху снарядів: `AmmoPhysicModel`, `GrenadePhysicModel` (відскок), `RopePhysicModel`, `UnitPhysicModel`, `PhysParams` (маски колізій, гравітація) |
| `weapons/` | **257 класів зброї** (`Bazooka`, `Cannon`, `ClusterGrenade`, `Crossbow`, `AirStrike`…) + базовий `Weapon.as` |
| `weapons/ammo/` | **135 класів снарядів** (`Cannonball`, `Bullet`, `AtomBomb`, `DrillMissile`…) |
| `objects/`, `game/`, `scene/`, `controller/` | ігрові об'єкти, ігровий цикл, сцена, керування |
| `messages/`, `serialization/` | мережевий протокол (бінарна серіалізація) |

## Звідки починати вивчення механіки пострілу
1. `weapons/Weapon.as` — базовий клас: `MIN_POWER`/`MAX_POWER`,
   `shoot()`, `shotParams.angle` / `shotParams.power`, зміна ходу.
2. `weapons/Bazooka.as` — найпростіший приклад: `shoot()` спавнить
   снаряд за кутом і силою.
3. `physics/model/AmmoPhysicModel.as` → `GrenadePhysicModel.as` —
   інтегрування руху, гравітація, попіксельний `hitTestPoint` по
   ландшафту, відскок (`bounceCoeff`, бітова маска сторін зіткнення).
4. `physics/PhysParams` та `CollideType` — маски колізій
   (`COLLIDES_WITH_GROUND/UNIT/AMMO`).

## Застереження
Частину **допоміжних** класів/полів обфусковано (імена виду `§_-X1j§`).
Ядро (physics, weapons, ammo, objects) читається нормально; обфусковані
залежності перенесено в `../_REVIEW_THEN_DELETE/Wormix/`.

## Чим відкривати
- Просто читати `.as` файли текстово.
- Або GUI: `C:\Projects\Game\.tools\ffdec\ffdec.bat` → відкрити
  `assets/wormix-mobile.swf` (навігація по класах, пошук, ре-декомпіляція).

## Оригінальні ассети (лишені як референс)
`assets/data/` — `maps/*.swf` (геометрія рівнів), `particles/`,
`dragonbones/` (скелети), `sounds/`, `ui/*.xml` (layout меню),
`serializers.xml`, `resconfig.xml`.
