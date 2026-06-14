using System;
using System.Collections.Generic;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Returns the value stored for <paramref name="key"/>, creating and inserting a new
        /// value with <paramref name="factory"/> when the key is not present.
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            Func<TKey, TValue> factory)
        {
            if (dictionary.TryGetValue(key, out var existing))
                return existing;

            var created = factory(key);
            dictionary[key] = created;
            return created;
        }

        /// <summary>
        /// Returns the value stored for <paramref name="key"/>, creating and inserting a new
        /// default-constructed value when the key is not present.
        /// </summary>
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key)
            where TValue : new()
        {
            if (dictionary.TryGetValue(key, out var existing))
                return existing;

            var created = new TValue();
            dictionary[key] = created;
            return created;
        }

        // TODO: add more
    }
}
