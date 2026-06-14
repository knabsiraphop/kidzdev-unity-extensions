# Changelog

All notable changes to this package are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
