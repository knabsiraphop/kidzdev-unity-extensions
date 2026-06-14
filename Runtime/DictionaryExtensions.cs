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

        /// <summary>
        /// Adds <paramref name="amount"/> to the integer stored for <paramref name="key"/> (treating a missing
        /// key as 0) and returns the new total. The tally / counter pattern.
        /// </summary>
        public static int Increment<TKey>(this IDictionary<TKey, int> dictionary, TKey key, int amount = 1)
        {
            dictionary.TryGetValue(key, out var current);
            int updated = current + amount;
            dictionary[key] = updated;
            return updated;
        }

        /// <summary>
        /// Inserts <paramref name="addValue"/> when <paramref name="key"/> is absent; otherwise replaces the
        /// existing value with the result of <paramref name="updateFactory"/>. Returns the stored value.
        /// </summary>
        public static TValue AddOrUpdate<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            TKey key,
            TValue addValue,
            Func<TKey, TValue, TValue> updateFactory)
        {
            TValue value = dictionary.TryGetValue(key, out var existing)
                ? updateFactory(key, existing)
                : addValue;

            dictionary[key] = value;
            return value;
        }
    }
}
