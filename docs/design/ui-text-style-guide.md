# UI Text Style Guide

## Rule

All new runtime UI text must be styled through `Game.UI.SciFiTextStyle.Apply(...)`.
Do not hardcode TMP font, size, color, bold state, wrapping, or spacing in a screen
controller unless a new reusable role/token is first added to `SciFiTextStyle`.

## Fonts

- Body/UI font: Exo 2.
- Large heading font: Orbitron.
- Orbitron is only for large first-read labels: main CTA text, screen titles,
  result titles, and other prominent headings.
- Exo 2 is for buttons, counters, descriptions, stats, tooltips, modal text,
  shop text, battle hints, and any dense UI copy.

Source font resource paths:

- `Assets/_Game/Content/Resources/Fonts/Exo2.ttf`
- `Assets/_Game/Content/Resources/Fonts/Orbitron.ttf`

Optional prebuilt TMP SDF asset paths:

- `Assets/_Game/Content/Resources/Fonts/Exo2 SDF.asset`
- `Assets/_Game/Content/Resources/Fonts/Orbitron SDF.asset`

`SciFiTextStyle` first tries the prebuilt TMP SDF asset. If it does not exist
yet, it loads the `.ttf` source from `Resources/Fonts` and creates a TMP font
asset at runtime, so UI must not fall back to the default LiberationSans font.

## Roles

Use the closest `GameTextRole` instead of inventing per-screen styles:

- `DisplayTitle`: biggest title/CTA/result text, Orbitron, gold.
- `ScreenTitle`: screen or modal title, Orbitron, main white.
- `Button`: ordinary button labels, Exo 2, bold, main white.
- `Body`: descriptions, hints, rows, stat labels, Exo 2, regular, main white.
- `Caption`: secondary help text, Exo 2, muted blue-gray.
- `Numeric`: timers and important numeric counters, Exo 2, gold.
- `CurrencyGold`: coin price/value text.
- `CurrencyCrystal`: crystal price/value text.
- `Success`: positive combat/progression values.
- `Warning`: locked, error, damage, or destructive action text.
- `Disabled`: inactive labels.

## Size Ladder

Default role sizes are centralized in `SciFiTextStyle.SizeFor(...)`.
Screen-specific code may pass a size override to fit an existing asset frame,
but the value should stay on this ladder unless there is a layout reason:

- 56: display title / primary CTA.
- 42-47: screen title and result title.
- 30-36: buttons, timer, important counters.
- 24-28: regular body and stat text.
- 19-22: captions and dense secondary copy.

## Color Discipline

Use `SciFiTextStyle` colors for text:

- main white-blue for readable primary text;
- muted blue-gray for secondary copy;
- cyan for unlocks, tech emphasis, and crystal-adjacent UI;
- gold for primary rewards, prices, timers, and result emphasis;
- green only for success/health-positive values;
- red only for damage, locked states, destructive actions, and errors.

New colors should be added as named tokens in `SciFiTextStyle`; do not scatter
new `new Color(...)` values across UI controllers for text.
