using System;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex RichTextTagPattern = new Regex("<[^>]*>");

        /// <summary>Returns <c>true</c> when the string is <c>null</c>, empty, or only whitespace.</summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// Truncates the string so its length never exceeds <paramref name="maxLength"/>, appending
        /// <paramref name="ellipsis"/> when characters were dropped. Returns the original value when it
        /// is <c>null</c>, empty, or already short enough.
        /// </summary>
        public static string Truncate(this string value, int maxLength, string ellipsis = "…")
        {
            if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
                return value;

            if (maxLength <= 0)
                return string.Empty;

            ellipsis ??= string.Empty;

            // Ellipsis alone does not fit — return as much of it as the budget allows.
            if (ellipsis.Length >= maxLength)
                return ellipsis.Substring(0, maxLength);

            return value.Substring(0, maxLength - ellipsis.Length) + ellipsis;
        }

        /// <summary>Returns <paramref name="fallback"/> when this string is <c>null</c> or empty; otherwise the string.</summary>
        public static string Or(this string value, string fallback)
        {
            return string.IsNullOrEmpty(value) ? fallback : value;
        }

        /// <summary>Ordinal, case-insensitive equality — no <c>ToLower()</c> allocation.</summary>
        public static bool EqualsIgnoreCase(this string value, string other)
        {
            return string.Equals(value, other, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns <c>true</c> when this string contains <paramref name="substring"/>, compared ordinally
        /// and case-insensitively. <c>null</c> on either side returns <c>false</c>.
        /// </summary>
        public static bool ContainsIgnoreCase(this string value, string substring)
        {
            if (value == null || substring == null)
                return false;

            return value.IndexOf(substring, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// Converts the string to Title Case using the invariant culture. The input is lower-cased first,
        /// so already-upper-case words (e.g. "iOS") are normalised rather than left untouched.
        /// </summary>
        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(value.ToLowerInvariant());
        }

        /// <summary>
        /// Removes Unity / TextMeshPro rich-text tags (<c>&lt;b&gt;</c>, <c>&lt;color=...&gt;</c>,
        /// <c>&lt;size=...&gt;</c>, …), leaving the visible text. Anything between <c>&lt;</c> and
        /// <c>&gt;</c> is stripped, so plain text containing angle brackets is affected too.
        /// </summary>
        public static string StripRichText(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return RichTextTagPattern.Replace(value, string.Empty);
        }

        /// <summary>
        /// Parses an HTML / hex colour string (e.g. <c>"#RRGGBB"</c>, <c>"#RRGGBBAA"</c>, or a named
        /// colour) into <paramref name="color"/>. Returns <c>false</c> when the string is not a valid colour.
        /// </summary>
        public static bool TryToColor(this string value, out Color color)
        {
            return ColorUtility.TryParseHtmlString(value, out color);
        }

        /// <summary>
        /// Parses an HTML / hex colour string (see <see cref="TryToColor"/>), returning
        /// <paramref name="fallback"/> when the string is not a valid colour.
        /// </summary>
        public static Color ToColor(this string value, Color fallback = default)
        {
            return ColorUtility.TryParseHtmlString(value, out var color) ? color : fallback;
        }
    }
}
