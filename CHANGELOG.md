# Changelog

All notable changes to this package are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.0] - 2026-07-19

### Added

- `AnimatorExtensions` (new class): `RestartCurrentState(layer = 0)` (replays the current state from
  normalized time 0) and `ResetToDefault()` (`Rebind()` + immediate `Update(0)`).
- `CameraExtensions` (new class): `SetLayerVisible(layer, visible)` / `SetLayerVisible(layerName, visible)`,
  `ToggleLayer(layer)` / `ToggleLayer(layerName)`, and `IsLayerVisible(layer)` — culling-mask helpers.
- `ScrollRectExtensions` (new class): `ScrollVertical(delta)` / `ScrollHorizontal(delta)` (clamped to
  `[0, 1]`) and `ScrollToTop()` / `ScrollToBottom()` / `ScrollToLeft()` / `ScrollToRight()`.
- `CollectionExtensions`: `Shuffle(this IList<T>, System.Random)` overload for deterministic/seeded
  shuffles without touching Unity's global random state, and `FindIndex(predicate)` (fills the gap left
  by `List<T>.FindIndex`, which isn't available on the wider `IEnumerable<T>`).
- `StringExtensions`: `WithColor(color)` (rich-text `<color>` wrap), `NormalizeNewlines(newline = "\n")`
  (collapses `\r\n` / `\r` / Unicode line separators to one form), and `TryToEnum<TEnum>(out result)` /
  `ToEnumOrDefault<TEnum>(fallback)` (safe enum parsing, matching the existing `TryToInt`/`ToIntOrDefault`
  shape).
- `LongExtensions.ToAbbreviatedString(decimals = 1)` and `IntExtensions.ToAbbreviatedString(decimals = 1)`:
  compact K / M / B / T display formatting (e.g. `1500` → `"1.5K"`).

### Changed

- `DictionaryExtensions.Increment` gains an optional `removeIfZero` parameter — when `true` and the
  updated total is exactly 0, the key is removed instead of stored at 0.

## [1.1.0] - 2026-06-21

### Added

- `DateTimeOffsetExtensions` (new class): `IsBetween(start, end)` (offset-aware, normalised to UTC),
  `IsSameDay(other)` (calendar-date comparison using each value's own offset), `StartOfDay()`,
  `EndOfDay()`, `StartOfWeek(firstDayOfWeek)` (defaults to Monday), `StartOfMonth()`, `EndOfMonth()`
  (all preserve the value's UTC offset), `ToIso8601String()` (round-trippable `"o"` format, invariant
  culture), `ToRelativeTime(now?)` (returns a locale-agnostic `RelativeTime` struct for use with a
  localisation system), `ToRelativeString(now?)` (English convenience string), `ToFriendlyDateString(provider?)`
  (`"MMM d, yyyy"`), and `ToFriendlyDateTimeString(provider?)` (`"MMM d, yyyy HH:mm"`).
- `RelativeTime` struct (new type): locale-agnostic relative-time result produced by
  `DateTimeOffsetExtensions.ToRelativeTime`. Fields: `Unit` (`RelativeTimeUnit` enum), `Count` (int),
  `IsFuture` (bool). `ToString()` provides an English convenience string; use the fields with your own
  localisation system to build culture-specific strings. `RelativeTime.FromDelta(TimeSpan)` is the
  factory used internally.
- `RelativeTimeUnit` enum (new type): `JustNow`, `Seconds`, `Minutes`, `Hours`, `Days`, `Weeks`,
  `Months`, `Years`.
- `TimeSpanExtensions` (new class): `ToClockString()` (digital-clock format `H:mm:ss` / `m:ss`,
  negative treated as zero, days roll into hours), `ToCompactString()` (two most-significant non-zero
  units from d/h/m/s, e.g. `"2d 3h"`, `"1h 30m"`, negative durations prefixed with `"-"`),
  `Clamp(min, max)`, and `IsBetween(min, max)`.
- `LongExtensions`: `ToDateTimeOffsetFromUnixSeconds(this long seconds)` and
  `ToDateTimeOffsetFromUnixMilliseconds(this long milliseconds)` — convert Unix timestamps to
  `DateTimeOffset` in UTC (the reverse `dto.ToUnixTimeSeconds()` / `dto.ToUnixTimeMilliseconds()` are
  already first-class BCL methods).
- Demo sample updated with a two-column on-screen readout: the existing core extensions remain in the
  left column; the new `DateTimeOffsetExtensions` and `TimeSpanExtensions` samples appear in a second
  right-hand column.

## [1.0.0] - 2026-06-14

### Added

- `StringExtensions`: `StartsWithIgnoreCase()` / `EndsWithIgnoreCase()` (ordinal), invariant-culture
  number parsing `TryToInt()` / `ToIntOrDefault()` / `TryToFloat()` / `ToFloatOrDefault()`, and
  `GetStableHashCode()` (deterministic FNV-1a hash, stable across runs and platforms).
- `CollectionExtensions`: `IsValidIndex()` (on `IList<T>`, covering arrays and lists), `Swap()`,
  `PopRandom()`, and `MinBy()` / `MaxBy()` (key selector — fills the .NET 6 gap absent from Unity's runtime).
- `DictionaryExtensions`: `Increment()` (counter / tally) and `AddOrUpdate()`.
- `IntExtensions`, `FloatExtensions`, `DoubleExtensions`, `LongExtensions`: numeric helpers including
  `Remap()`, `Clamp01()`, `Snap()`, `Approximately()` / `IsZero()`, `RoundToInt()` / `FloorToInt()` /
  `CeilToInt()`, `Wrap()`, `IsBetween()`, `ToOrdinal()`, `ToThousandsString()`, and `ToFileSizeString()`.
- `GameObjectExtensions`: `DestroySelf()`, `IsInLayerMask()`, `GetPath()`, and `RefreshLayout()`
  (forces a UGUI `LayoutRebuilder` pass — the package now references `UnityEngine.UI`).
- Demo sample: a `Demo.unity` scene plus a rewritten `ExtensionsDemo` that exercises every method and
  reports each result to the Console and an on-screen readout.

### Removed

- **Breaking:** `ArrayExtensions` — `IsNullOrEmpty()` duplicated `CollectionExtensions.IsNullOrEmpty()`
  (which already covers arrays via the `ICollection<T>.Count` fast path), and `IsValidIndex()` moved to
  `CollectionExtensions` as `this IList<T>` (covers arrays and lists). Extension-method call sites are
  unaffected — `array.IsNullOrEmpty()` / `array.IsValidIndex(i)` rebind to the collection overloads.
- **Breaking:** `StringExtensions.IsNullOrWhiteSpace(this string)` — a bare wrapper over
  `string.IsNullOrWhiteSpace`. Use `string.IsNullOrWhiteSpace(value)` directly.

## [0.2.0] - 2026-06-14

### Added

- `StringExtensions`: `IsNullOrWhiteSpace()`, `Or(fallback)`, `EqualsIgnoreCase()`,
  `ContainsIgnoreCase()`, `ToTitleCase()`, `StripRichText()` (strips Unity/TMP
  rich-text tags), and `ToColor()` / `TryToColor()` (hex / named colour parsing).
- `CollectionExtensions`: `GetRandomOrDefault()` and `Shuffle()` (in-place
  Fisher–Yates using Unity's random generator).
- `GameObjectExtensions`: `DestroyChildren()` (play-mode-aware — uses
  `DestroyImmediate` in the editor).

### Changed

- `StringExtensions.Truncate` now takes an optional `ellipsis` (default `"…"`) and
  keeps the result within `maxLength`.
- `CollectionExtensions.IsNullOrEmpty` now uses the `ICollection<T>.Count` fast path
  instead of allocating an enumerator.
- `CollectionExtensions.GetRandom` now throws a clear `InvalidOperationException` for a
  null or empty list instead of an index-out-of-range error.

### Removed

- **Breaking:** `StringExtensions.IsNullOrEmpty(this string)` — a bare wrapper over
  `string.IsNullOrEmpty`. Use `string.IsNullOrEmpty(value)`, or the new
  `IsNullOrWhiteSpace()` / `Or()` helpers instead.

## [0.1.0] - 2026-06-13

### Added

- Initial scaffold: `StringExtensions`, `CollectionExtensions`, `ArrayExtensions`,
  `DictionaryExtensions`, and `GameObjectExtensions`, each with a couple of example
  extension methods to build on.
- Demo sample showing the extension methods in use.
