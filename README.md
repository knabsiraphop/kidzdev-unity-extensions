# KidzDev Unity Extensions

Handy C# / Unity extension methods — strings, collections, arrays, dictionaries, and more.

A small, focused set of `static` extension classes you can drop into any Unity 6000.0+
project and grow over time. Its only assembly reference is `UnityEngine.UI` (a default
Unity package), used by `GameObjectExtensions.RefreshLayout`.

## Installation

Install via the Unity Package Manager using a Git URL pinned to a release tag:

1. Open **Window > Package Manager**.
2. Click **+ > Add package from git URL…**
3. Enter:

   ```
   https://github.com/knabsiraphop/kidzdev-unity-extensions.git#v1.1.0
   ```

Or add it directly to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.kidzdev.unity.extensions": "https://github.com/knabsiraphop/kidzdev-unity-extensions.git#v1.1.0"
  }
}
```

## Features

- **`StringExtensions`** — `Truncate(int, ellipsis)`, `Or(fallback)`, `EqualsIgnoreCase()`, `ContainsIgnoreCase()`, `StartsWithIgnoreCase()` / `EndsWithIgnoreCase()`, `ToTitleCase()`, `StripRichText()`, `ToColor()` / `TryToColor()`, `TryToInt()` / `ToIntOrDefault()`, `TryToFloat()` / `ToFloatOrDefault()`, `GetStableHashCode()`
- **`CollectionExtensions`** — `IsNullOrEmpty<T>()`, `IsValidIndex<T>(int)`, `GetRandom<T>()`, `GetRandomOrDefault<T>()`, `Shuffle<T>()`, `Swap<T>(int, int)`, `PopRandom<T>()`, `MinBy<T, TKey>()` / `MaxBy<T, TKey>()`
- **`DictionaryExtensions`** — `GetOrAdd<TKey, TValue>(...)`, `Increment<TKey>()`, `AddOrUpdate<TKey, TValue>(...)`
- **`IntExtensions`** — `Wrap(length)`, `IsBetween(min, max)`, `ToThousandsString()`, `ToOrdinal()`
- **`FloatExtensions`** — `Approximately()`, `IsZero()`, `Clamp01()`, `Remap(...)`, `Snap(step)`, `RoundToInt()` / `FloorToInt()` / `CeilToInt()`, `IsBetween()`, `ToThousandsString(decimals)`
- **`DoubleExtensions`** — `Clamp01()`, `Remap(...)`, `IsBetween()`, `ToThousandsString(decimals)`
- **`LongExtensions`** — `IsBetween()`, `ToThousandsString()`, `ToFileSizeString()`
- **`GameObjectExtensions`** — `SetLayerRecursively(int)`, `GetOrAddComponent<T>()`, `DestroyChildren()`, `DestroySelf()`, `IsInLayerMask(LayerMask)`, `GetPath()`, `RefreshLayout()`

All methods live in the `KidzDev.Unity.Extensions` namespace and the auto-referenced
`KidzDev.Unity.Extensions` assembly, so they are available everywhere without extra `using`s
beyond the namespace import.

## Sample

A **Demo** sample ships with the package. Import it from the Package Manager:

**Package Manager > KidzDev Extensions > Samples > Demo > Import**

It includes a `Demo.unity` scene with an `ExtensionsDemo` MonoBehaviour that exercises every
extension method and reports each result to the Console and an on-screen readout.

## License

[MIT](LICENSE.md) © 2026 KidzDev
