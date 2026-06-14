namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for arrays.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>Returns <c>true</c> when the array is <c>null</c> or has zero length.</summary>
        public static bool IsNullOrEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }

        /// <summary>Returns <c>true</c> when <paramref name="index"/> is within the array bounds.</summary>
        public static bool IsValidIndex<T>(this T[] array, int index)
        {
            return array != null && index >= 0 && index < array.Length;
        }

        // TODO: add more
    }
}
