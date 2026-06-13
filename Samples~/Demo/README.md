# Demo

A single `MonoBehaviour` (`ExtensionsDemo`) that, on `Start()`, calls a handful of the
extension methods shipped in **KidzDev Extensions** and writes the results to the Console.

## What it shows

- `string.Truncate(int)` and `string.IsNullOrEmpty()`
- `array.IsNullOrEmpty()` and `IList<T>.GetRandom()`
- `IDictionary<TKey, TValue>.GetOrAdd(key, factory)`
- `GameObject.SetLayerRecursively(int)`

## How to run

1. Create an empty scene.
2. Add an empty GameObject and attach the `ExtensionsDemo` component.
3. Press **Play** and watch the Console.
