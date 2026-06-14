using System;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>Returns <c>true</c> when the string is <c>null</c> or empty.</summary>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Truncates the string to at most <paramref name="maxLength"/> characters.
        /// Returns the original value when it is <c>null</c>, empty, or already short enough.
        /// </summary>
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value;

            return value.Substring(0, Math.Max(0, maxLength));
        }

        // TODO: add more
    }
}
