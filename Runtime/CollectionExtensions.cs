using System;
using System.Collections.Generic;
using System.Linq;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for collections (<see cref="IEnumerable{T}"/> and <see cref="IList{T}"/>).
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>Returns <c>true</c> when the sequence is <c>null</c> or contains no elements.</summary>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true;

            // Use the known count when available to avoid allocating an enumerator.
            if (source is ICollection<T> collection)
                return collection.Count == 0;

            return !source.Any();
        }

        /// <summary>Returns a uniformly random element from the list. Throws when the list is <c>null</c> or empty.</summary>
        public static T GetRandom<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                throw new InvalidOperationException("Cannot get a random element from a null or empty list.");

            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>Returns a uniformly random element, or <c>default(T)</c> when the list is <c>null</c> or empty.</summary>
        public static T GetRandomOrDefault<T>(this IList<T> list)
        {
            return list == null || list.Count == 0
                ? default
                : list[UnityEngine.Random.Range(0, list.Count)];
        }

        /// <summary>
        /// Shuffles the list in place using the Fisher–Yates algorithm and Unity's random generator.
        /// Does nothing when the list is <c>null</c>.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list == null)
                return;

            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
