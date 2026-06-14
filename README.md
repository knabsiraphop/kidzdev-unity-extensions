# KidzDev Unity Extensions

Handy C# / Unity extension methods — strings, collections, arrays, dictionaries, and more.

A small, dependency-free scaffold of `static` extension classes you can drop into any
Unity 6000.0+ project and grow over time.

## Installation

Install via the Unity Package Manager using a Git URL pinned to a release tag:

1. Open **Window > Package Manager**.
2. Click **+ > Add package from git URL…**
3. Enter:

   ```
   https://github.com/knabsiraphop/kidzdev-unity-extensions.git#v0.2.0
   ```

Or add it directly to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.kidzdev.unity.extensions": "https://github.com/knabsiraphop/kidzdev-unity-extensions.git#v0.2.0"
  }
}
```

## Features

- **`StringExtensions`** — `IsNullOrWhiteSpace()`, `Truncate(int, ellipsis)`, `Or(fallback)`, `EqualsIgnoreCase()`, `ContainsIgnoreCase()`, `ToTitleCase()`, `StripRichText()`, `ToColor()` / `TryToColor()`
- **`CollectionExtensions`** — `IsNullOrEmpty<T>()`, `GetRandom<T>()`, `GetRandomOrDefault<T>()`, `Shuffle<T>()`
- **`ArrayExtensions`** — `IsNullOrEmpty<T>()`, `IsValidIndex<T>(int)`
- **`DictionaryExtensions`** — `GetOrAdd<TKey, TValue>(...)`
- **`GameObjectExtensions`** — `SetLayerRecursively(int)`, `GetOrAddComponent<T>()`, `DestroyChildren()`

All methods live in the `KidzDev.Unity.Extensions` namespace and the auto-referenced
`KidzDev.Unity.Extensions` assembly, so they are available everywhere without extra `using`s
beyond the namespace import.

## Sample

A **Demo** sample ships with the package. Import it from the Package Manager:

**Package Manager > KidzDev Extensions > Samples > Demo > Import**

It adds an `ExtensionsDemo` MonoBehaviour that calls a few of the methods and logs the
results to the Console.

## License

[MIT](LICENSE.md) © 2026 KidzDev
