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
            return source == null || !source.Any();
        }

        /// <summary>Returns a uniformly random element from the list.</summary>
        public static T GetRandom<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        // TODO: add more
    }
}
