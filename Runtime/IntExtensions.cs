using System;
using System.Globalization;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="int"/>.
    /// </summary>
    public static class IntExtensions
    {
        /// <summary>
        /// Wraps the index into the half-open range <c>[0, <paramref name="length"/>)</c>, handling negatives
        /// correctly (unlike the <c>%</c> operator). Ideal for cycling array indices or menu selection.
        /// Throws when <paramref name="length"/> is not positive.
        /// </summary>
        public static int Wrap(this int index, int length)
        {
            if (length <= 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be positive.");

            int result = index % length;
            return result < 0 ? result + length : result;
        }

        /// <summary>Returns <c>true</c> when the value is within the inclusive range <c>[min, max]</c>.</summary>
        public static bool IsBetween(this int value, int min, int max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Formats the value with grouped thousands (e.g. <c>1234567</c> → <c>"1,234,567"</c>). Uses the
        /// invariant culture, so the group separator is always <c>','</c> regardless of the machine's locale.
        /// </summary>
        public static string ToThousandsString(this int value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats the value as a compact display string using K / M / B suffixes. See
        /// <see cref="LongExtensions.ToAbbreviatedString(long, int)"/> for the full formatting rules.
        /// </summary>
        public static string ToAbbreviatedString(this int value, int decimals = 1)
        {
            return ((long)value).ToAbbreviatedString(decimals);
        }

        /// <summary>
        /// Returns the value with its English ordinal suffix appended — <c>1</c> → <c>"1st"</c>,
        /// <c>2</c> → <c>"2nd"</c>, <c>13</c> → <c>"13th"</c>. Formatted with the invariant culture.
        /// </summary>
        public static string ToOrdinal(this int value)
        {
            int lastTwo = Math.Abs(value % 100);
            string suffix;

            if (lastTwo >= 11 && lastTwo <= 13)
            {
                suffix = "th";
            }
            else
            {
                switch (lastTwo % 10)
                {
                    case 1: suffix = "st"; break;
                    case 2: suffix = "nd"; break;
                    case 3: suffix = "rd"; break;
                    default: suffix = "th"; break;
                }
            }

            return value.ToString(CultureInfo.InvariantCulture) + suffix;
        }
    }
}
