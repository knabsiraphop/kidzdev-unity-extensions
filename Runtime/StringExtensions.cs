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
        private static readonly Regex NewlinePattern = new Regex("\r\n|[\r\u2028\u2029\u0085]");

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
        /// Returns <c>true</c> when this string starts with <paramref name="prefix"/>, compared ordinally
        /// and case-insensitively — avoids the culture-sensitive default of <see cref="string.StartsWith(string)"/>.
        /// <c>null</c> on either side returns <c>false</c>.
        /// </summary>
        public static bool StartsWithIgnoreCase(this string value, string prefix)
        {
            if (value == null || prefix == null)
                return false;

            return value.StartsWith(prefix, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Returns <c>true</c> when this string ends with <paramref name="suffix"/>, compared ordinally
        /// and case-insensitively — avoids the culture-sensitive default of <see cref="string.EndsWith(string)"/>.
        /// <c>null</c> on either side returns <c>false</c>.
        /// </summary>
        public static bool EndsWithIgnoreCase(this string value, string suffix)
        {
            if (value == null || suffix == null)
                return false;

            return value.EndsWith(suffix, StringComparison.OrdinalIgnoreCase);
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

        /// <summary>
        /// Parses an integer using the invariant culture (so it is locale-independent). Returns
        /// <c>true</c> and the parsed value on success; otherwise <c>false</c> with <paramref name="result"/> set to 0.
        /// </summary>
        public static bool TryToInt(this string value, out int result)
        {
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Parses an integer using the invariant culture, returning <paramref name="fallback"/> when the
        /// string is not a valid integer. Locale-independent, unlike <see cref="int.Parse(string)"/>.
        /// </summary>
        public static int ToIntOrDefault(this string value, int fallback = 0)
        {
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result) ? result : fallback;
        }

        /// <summary>
        /// Parses a float using the invariant culture (so <c>"1.5"</c> parses regardless of the machine's
        /// decimal separator). Returns <c>true</c> and the parsed value on success; otherwise <c>false</c>
        /// with <paramref name="result"/> set to 0.
        /// </summary>
        public static bool TryToFloat(this string value, out float result)
        {
            return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out result);
        }

        /// <summary>
        /// Parses a float using the invariant culture, returning <paramref name="fallback"/> when the string
        /// is not a valid number. Avoids the locale decimal-separator bug of <see cref="float.Parse(string)"/>.
        /// </summary>
        public static float ToFloatOrDefault(this string value, float fallback = 0f)
        {
            return float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var result) ? result : fallback;
        }

        /// <summary>
        /// Wraps the string in a Unity / TextMeshPro rich-text <c>&lt;color&gt;</c> tag, including the
        /// closing tag.
        /// </summary>
        public static string WithColor(this string value, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{value}</color>";
        }

        /// <summary>
        /// Collapses every newline variant (<c>"\r\n"</c>, <c>"\r"</c>, <c>"\n"</c>, and the Unicode line
        /// separator, paragraph separator, and next-line characters) to a single <paramref name="newline"/>
        /// (defaults to <c>"\n"</c>). Returns the input unchanged when it is <c>null</c> or empty.
        /// </summary>
        public static string NormalizeNewlines(this string value, string newline = "\n")
        {
            if (string.IsNullOrEmpty(value))
                return value;

            return NewlinePattern.Replace(value, newline ?? string.Empty);
        }

        /// <summary>
        /// Parses <paramref name="value"/> as <typeparamref name="TEnum"/>. Returns <c>true</c> and the parsed
        /// value on success; otherwise <c>false</c> with <paramref name="result"/> set to <c>default</c>.
        /// </summary>
        public static bool TryToEnum<TEnum>(this string value, out TEnum result, bool ignoreCase = true) where TEnum : struct, Enum
        {
            return Enum.TryParse(value, ignoreCase, out result);
        }

        /// <summary>
        /// Parses <paramref name="value"/> as <typeparamref name="TEnum"/>, returning <paramref name="fallback"/>
        /// when the string is not a valid member of the enum.
        /// </summary>
        public static TEnum ToEnumOrDefault<TEnum>(this string value, TEnum fallback = default, bool ignoreCase = true) where TEnum : struct, Enum
        {
            return Enum.TryParse(value, ignoreCase, out TEnum result) ? result : fallback;
        }

        /// <summary>
        /// A deterministic 32-bit hash (FNV-1a) that is stable across runs and platforms — unlike
        /// <see cref="string.GetHashCode()"/>, which is randomised per process. Safe for save keys, stable
        /// IDs, and content addressing. Returns 0 for <c>null</c>.
        /// </summary>
        public static int GetStableHashCode(this string value)
        {
            if (value == null)
                return 0;

            unchecked
            {
                const uint offsetBasis = 2166136261;
                const uint prime = 16777619;

                uint hash = offsetBasis;
                for (int i = 0; i < value.Length; i++)
                    hash = (hash ^ value[i]) * prime;

                return (int)hash;
            }
        }
    }
}
