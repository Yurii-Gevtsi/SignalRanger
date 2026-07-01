# Fair Shot Prototype

Це перший playable technical slice для `Milestone 1: Fair Shot Prototype`.

## Як запустити

1. Відкрити проєкт в Unity.
2. Відкрити `Assets/Scenes/SampleScene.unity`.
3. Натиснути Play.

Прототип створюється в runtime через `FairShotPrototypeBootstrap`, тому сцена може лишатися майже порожньою.

## Керування

- Рух мишкою або touch: aim і power.
- `Fire` button або `Space`: постріл.
- `Restart` button або `R`: перезапуск матчу.

## Що перевіряє прототип

- runtime battle setup;
- two placeholder units;
- turn switching;
- deliberate aim and fire;
- trajectory preview;
- projectile arc;
- 2D collision;
- radial damage;
- HP update;
- win/loss state;
- restart.

## Поточні обмеження

- Placeholder-візуал.
- Terrain має collision, але ще не destructible.
- Обидві сторони керуються локально гравцем.
- Немає AI, campaign, save, economy, shop, ads, IAP, backend або SDK.

## Джерело

Prototype code:

```text
Assets/_Game/Runtime/Gameplay/Prototype/FairShotPrototype.cs
```

