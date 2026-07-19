# KidzDev Unity Extensions

Handy C# / Unity extension methods — strings, collections, dictionaries, numerics, date & time, GameObjects, Animators, Cameras, and ScrollRects.

A small, focused set of `static` extension classes you can drop into any Unity 6000.0+
project and grow over time. Its only assembly reference is `UnityEngine.UI` (a default
Unity package), used by `GameObjectExtensions.RefreshLayout`.

## Installation

Install via the Unity Package Manager using a Git URL pinned to a release tag:

1. Open **Window > Package Manager**.
2. Click **+ > Add package from git URL…**
3. Enter:

   ```
   https://github.com/knabsiraphop/kidzdev-unity-extensions.git#v1.2.0
   ```

Or add it directly to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.kidzdev.unity.extensions": "https://github.com/knabsiraphop/kidzdev-unity-extensions.git#v1.2.0"
  }
}
```

All methods live in the `KidzDev.Unity.Extensions` namespace and the auto-referenced
`KidzDev.Unity.Extensions` assembly, so they are available project-wide with a single `using`.

---

## Features

### String

**`StringExtensions`**

| Method | Description |
|---|---|
| `Truncate(int, ellipsis?)` | Clip to max length, appending `"…"` (customisable) |
| `Or(fallback)` | Return fallback when null or empty |
| `EqualsIgnoreCase(other)` | Ordinal case-insensitive equality — no allocation |
| `ContainsIgnoreCase(sub)` | Ordinal case-insensitive `Contains` |
| `StartsWithIgnoreCase(prefix)` | Ordinal case-insensitive `StartsWith` |
| `EndsWithIgnoreCase(suffix)` | Ordinal case-insensitive `EndsWith` |
| `ToTitleCase()` | Title Case via invariant culture |
| `StripRichText()` | Remove Unity / TMP rich-text tags |
| `TryToColor(out Color)` / `ToColor(fallback)` | Parse hex / named colour |
| `TryToInt(out int)` / `ToIntOrDefault(fallback)` | Invariant-culture integer parse |
| `TryToFloat(out float)` / `ToFloatOrDefault(fallback)` | Invariant-culture float parse |
| `TryToEnum<TEnum>(out result, ignoreCase?)` / `ToEnumOrDefault<TEnum>(fallback, ignoreCase?)` | Safe enum parsing |
| `WithColor(color)` | Wrap in a rich-text `<color>` tag |
| `NormalizeNewlines(newline?)` | Collapse `\r\n` / `\r` / Unicode line separators to one form |
| `GetStableHashCode()` | Deterministic FNV-1a hash — stable across runs and platforms |

---

### Collections

**`CollectionExtensions`** — works on `IEnumerable<T>` and `IList<T>` (arrays + lists)

| Method | Description |
|---|---|
| `IsNullOrEmpty<T>()` | Null-safe empty check — uses `Count` fast path when available |
| `IsValidIndex<T>(int)` | Bounds check on any `IList<T>` |
| `GetRandom<T>()` | Uniform random element — throws on empty |
| `GetRandomOrDefault<T>()` | Uniform random element — returns `default` on empty |
| `Shuffle<T>()` | In-place Fisher–Yates using Unity's RNG |
| `Shuffle<T>(System.Random)` | In-place Fisher–Yates using a supplied `Random` — deterministic/seeded shuffles without touching Unity's global random state |
| `Swap<T>(int, int)` | Swap two elements by index |
| `PopRandom<T>()` | Remove and return a random element (bag-randomiser pattern) |
| `MinBy<T, TKey>(selector)` / `MaxBy<T, TKey>(selector)` | Min/max by key — fills the .NET 6 gap absent from Unity |
| `FindIndex<T>(predicate)` | Index of the first match — fills the gap left by `List<T>.FindIndex` on `IEnumerable<T>` |

**`DictionaryExtensions`**

| Method | Description |
|---|---|
| `GetOrAdd(key, factory)` | Return or create-and-insert via factory |
| `GetOrAdd(key)` | Return or create-and-insert via `new TValue()` |
| `Increment(key, amount?, removeIfZero?)` | Counter / tally — treats missing key as 0; when `removeIfZero` is `true` and the new total is 0, the key is removed instead of stored |
| `AddOrUpdate(key, addValue, updateFactory)` | Insert or update with a transform |

---

### Numeric

**`IntExtensions`**

| Method | Description |
|---|---|
| `Wrap(length)` | Wrap index into `[0, length)` — handles negatives correctly |
| `IsBetween(min, max)` | Inclusive range check |
| `ToThousandsString()` | `1234567` → `"1,234,567"` (invariant culture) |
| `ToOrdinal()` | `1` → `"1st"`, `2` → `"2nd"`, `13` → `"13th"` |
| `ToAbbreviatedString(decimals?)` | `1500` → `"1.5K"` compact K/M/B display formatting |

**`FloatExtensions`**

| Method | Description |
|---|---|
| `Approximately(other)` | Tolerant equality via `Mathf.Approximately` |
| `IsZero(epsilon?)` | Within epsilon of zero |
| `Clamp01()` | Clamp to `[0, 1]` |
| `Remap(inMin, inMax, outMin, outMax)` | Linear range mapping (unclamped) |
| `Snap(step)` | Round to nearest grid step |
| `RoundToInt()` / `FloorToInt()` / `CeilToInt()` | Integer conversions |
| `IsBetween(min, max)` | Inclusive range check |
| `ToThousandsString(decimals?)` | Invariant-culture thousands-grouped string |

**`DoubleExtensions`**

| Method | Description |
|---|---|
| `Clamp01()` | Clamp to `[0, 1]` |
| `Remap(inMin, inMax, outMin, outMax)` | Linear range mapping (unclamped) |
| `IsBetween(min, max)` | Inclusive range check |
| `ToThousandsString(decimals?)` | Invariant-culture thousands-grouped string |

**`LongExtensions`**

| Method | Description |
|---|---|
| `IsBetween(min, max)` | Inclusive range check |
| `ToThousandsString()` | Invariant-culture thousands-grouped string |
| `ToFileSizeString()` | `1536` → `"1.5 KB"` (binary 1024-based units) |
| `ToAbbreviatedString(decimals?)` | `1500` → `"1.5K"` compact K/M/B/T display formatting |
| `ToDateTimeOffsetFromUnixSeconds()` | Unix seconds → `DateTimeOffset` UTC |
| `ToDateTimeOffsetFromUnixMilliseconds()` | Unix milliseconds → `DateTimeOffset` UTC |

---

### Date & Time

**`DateTimeOffsetExtensions`**

Structural helpers (preserve offset):

| Method | Description |
|---|---|
| `IsBetween(start, end)` | Offset-aware inclusive range check |
| `IsSameDay(other)` | Same calendar date using each value's own offset |
| `StartOfDay()` | Midnight, same offset |
| `EndOfDay()` | Last tick of the day, same offset |
| `StartOfWeek(firstDay?)` | Midnight of week start (default Monday) |
| `StartOfMonth()` | First day of month, midnight |
| `EndOfMonth()` | Last tick of month |
| `ToIso8601String()` | Round-trippable `"o"` format, invariant culture |

Display formatters:

| Method | Description |
|---|---|
| `ToRelativeTime(now?)` | → `RelativeTime` struct — locale-agnostic, ready for your localization keys |
| `ToRelativeString(now?)` | → English convenience string: `"5 minutes ago"`, `"in 2 hours"`, `"yesterday"` |
| `ToFriendlyDateString(provider?)` | → `"Jun 21, 2026"` — pass a `CultureInfo` for localized month names |
| `ToFriendlyDateTimeString(provider?)` | → `"Jun 21, 2026 14:30"` |

**`RelativeTime` struct** — returned by `ToRelativeTime()`:

```csharp
public readonly struct RelativeTime
{
    public RelativeTimeUnit Unit   { get; }  // JustNow / Seconds / Minutes / Hours / Days / Weeks / Months / Years
    public int              Count  { get; }
    public bool             IsFuture { get; }
}
```

Use the struct fields to build a localised string with your own system:

```csharp
var rt = timestamp.ToRelativeTime();
string text = rt.Unit == RelativeTimeUnit.JustNow
    ? LocalizationSystem.GetText("time_JustNow")
    : LocalizationSystem.GetTextFormat($"time_{rt.Unit}_{(rt.IsFuture ? "future" : "past")}", rt.Count);
// key "time_Minutes_past" = "{0} minutes ago"
```

**`TimeSpanExtensions`**

| Method | Description |
|---|---|
| `ToClockString()` | Digital timer: `"1:05:09"` / `"2:30"` — negatives show `"0:00"` |
| `ToCompactString()` | Two most-significant units: `"2d 3h"`, `"1h 30m"`, `"45s"`, `"-1h 30m"` |
| `Clamp(min, max)` | Clamp to a duration range |
| `IsBetween(min, max)` | Inclusive range check |

---

### GameObject

**`GameObjectExtensions`**

| Method | Description |
|---|---|
| `SetLayerRecursively(int)` | Set layer on this object and all descendants |
| `GetOrAddComponent<T>()` | Return existing component or add one |
| `DestroyChildren()` | Destroy all children — play-mode aware (`Destroy` vs `DestroyImmediate`) |
| `DestroySelf()` | Destroy this object — play-mode aware |
| `IsInLayerMask(LayerMask)` | Safe bit-mask layer test |
| `GetPath()` | Full hierarchy path: `"Canvas/Panel/Button"` |
| `RefreshLayout()` | Force immediate UGUI `LayoutRebuilder` pass |

---

### Animator

**`AnimatorExtensions`**

| Method | Description |
|---|---|
| `RestartCurrentState(layer?)` | Replay the current state on `layer` from normalized time 0 |
| `ResetToDefault()` | `Rebind()` + immediate `Update(0)` so the reset pose is visible without waiting a frame |

---

### Camera

**`CameraExtensions`** — culling-mask layer helpers

| Method | Description |
|---|---|
| `SetLayerVisible(int, bool)` / `SetLayerVisible(string, bool)` | Show or hide a layer in the camera's culling mask, by index or name |
| `ToggleLayer(int)` / `ToggleLayer(string)` | Toggle whether a layer is included in the culling mask |
| `IsLayerVisible(int)` | Whether a layer is included in the culling mask |

---

### ScrollRect

**`ScrollRectExtensions`**

| Method | Description |
|---|---|
| `ScrollVertical(delta)` / `ScrollHorizontal(delta)` | Apply a normalized delta to the scroll position, clamped to `[0, 1]` |
| `ScrollToTop()` / `ScrollToBottom()` | Snap `verticalNormalizedPosition` to `1` / `0` |
| `ScrollToLeft()` / `ScrollToRight()` | Snap `horizontalNormalizedPosition` to `0` / `1` |

---

## Sample

A **Demo** sample ships with the package. Import it from the Package Manager:

**Package Manager > KidzDev Extensions > Samples > Demo > Import**

It includes a `Demo.unity` scene with an `ExtensionsDemo` MonoBehaviour that exercises every
extension method. Results appear in a **two-column on-screen readout** (core extensions on the
left, date & time extensions on the right) and in the Console.

---

## Authorship

Built with [Claude Code](https://claude.com/claude-code), Anthropic's AI coding agent: the design, direction, and review are human ([@knabsiraphop](https://github.com/knabsiraphop)); most of the implementation code was written by Claude under that direction. All code is original — nothing copied from or bundled with third-party sources.

## License

[MIT](LICENSE.md) © 2026 KidzDev
