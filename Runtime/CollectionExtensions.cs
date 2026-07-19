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

        /// <summary>
        /// Returns <c>true</c> when <paramref name="index"/> is within the list bounds. Works for arrays and
        /// <see cref="List{T}"/> alike, since both implement <see cref="IList{T}"/>.
        /// </summary>
        public static bool IsValidIndex<T>(this IList<T> list, int index)
        {
            return list != null && index >= 0 && index < list.Count;
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

        /// <summary>
        /// Shuffles the list in place using the Fisher–Yates algorithm and the supplied <paramref name="random"/>
        /// instead of Unity's global random generator — use this for deterministic/seeded shuffles (tests,
        /// replays) without disturbing global random state. Does nothing when the list is <c>null</c>.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, Random random)
        {
            if (list == null)
                return;
            if (random == null)
                throw new ArgumentNullException(nameof(random));

            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>
        /// Returns the index of the first element matching <paramref name="predicate"/>, or <c>-1</c> when none
        /// match. Fills the gap left by <see cref="List{T}.FindIndex(Predicate{T})"/>, which only exists on
        /// <see cref="List{T}"/> and not on the wider <see cref="IEnumerable{T}"/>.
        /// </summary>
        public static int FindIndex<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int index = 0;
            foreach (T item in source)
            {
                if (predicate(item))
                    return index;
                index++;
            }

            return -1;
        }

        /// <summary>
        /// Swaps the elements at <paramref name="a"/> and <paramref name="b"/> in place. Throws
        /// <see cref="ArgumentNullException"/> for a null list and <see cref="ArgumentOutOfRangeException"/>
        /// when either index is out of range.
        /// </summary>
        public static void Swap<T>(this IList<T> list, int a, int b)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));
            if (a < 0 || a >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(a));
            if (b < 0 || b >= list.Count)
                throw new ArgumentOutOfRangeException(nameof(b));

            (list[a], list[b]) = (list[b], list[a]);
        }

        /// <summary>
        /// Removes and returns a uniformly random element from the list (the "bag randomiser" pattern).
        /// Throws when the list is <c>null</c> or empty.
        /// </summary>
        public static T PopRandom<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                throw new InvalidOperationException("Cannot pop a random element from a null or empty list.");

            int index = UnityEngine.Random.Range(0, list.Count);
            T element = list[index];
            list.RemoveAt(index);
            return element;
        }

        /// <summary>
        /// Returns the element with the smallest key as selected by <paramref name="keySelector"/>. Fills the
        /// gap left by .NET 6's <c>Enumerable.MinBy</c>, which is absent from Unity's runtime. Throws when the
        /// sequence is <c>null</c> or empty.
        /// </summary>
        public static T MinBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return source.SelectExtreme(keySelector, wantGreater: false);
        }

        /// <summary>
        /// Returns the element with the largest key as selected by <paramref name="keySelector"/>. Fills the
        /// gap left by .NET 6's <c>Enumerable.MaxBy</c>, which is absent from Unity's runtime. Throws when the
        /// sequence is <c>null</c> or empty.
        /// </summary>
        public static T MaxBy<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector)
        {
            return source.SelectExtreme(keySelector, wantGreater: true);
        }

        private static T SelectExtreme<T, TKey>(this IEnumerable<T> source, Func<T, TKey> keySelector, bool wantGreater)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (keySelector == null)
                throw new ArgumentNullException(nameof(keySelector));

            var comparer = Comparer<TKey>.Default;
            using var enumerator = source.GetEnumerator();

            if (!enumerator.MoveNext())
                throw new InvalidOperationException("Sequence contains no elements.");

            T best = enumerator.Current;
            TKey bestKey = keySelector(best);

            while (enumerator.MoveNext())
            {
                T candidate = enumerator.Current;
                TKey candidateKey = keySelector(candidate);
                int comparison = comparer.Compare(candidateKey, bestKey);

                if (wantGreater ? comparison > 0 : comparison < 0)
                {
                    best = candidate;
                    bestKey = candidateKey;
                }
            }

            return best;
        }
    }
}
