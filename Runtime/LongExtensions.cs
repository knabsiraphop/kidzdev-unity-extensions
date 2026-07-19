using System;
using System.Globalization;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="long"/>.
    /// </summary>
    public static class LongExtensions
    {
        /// <summary>Returns <c>true</c> when the value is within the inclusive range <c>[min, max]</c>.</summary>
        public static bool IsBetween(this long value, long min, long max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Formats the value with grouped thousands (e.g. <c>1234567</c> → <c>"1,234,567"</c>). Uses the
        /// invariant culture, so the group separator is always <c>','</c> regardless of the machine's locale.
        /// </summary>
        public static string ToThousandsString(this long value)
        {
            return value.ToString("N0", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats a byte count as a human-readable size using binary (1024-based) units — e.g.
        /// <c>1536</c> → <c>"1.5 KB"</c>. Sizes above bytes show up to two decimals; the invariant culture
        /// is used so the decimal separator is always <c>'.'</c>.
        /// </summary>
        public static string ToFileSizeString(this long bytes)
        {
            const double scale = 1024d;
            string[] units = { "B", "KB", "MB", "GB", "TB", "PB" };

            if (bytes == 0)
                return "0 B";

            bool negative = bytes < 0;
            double size = negative ? -(double)bytes : bytes;
            int unit = 0;

            while (size >= scale && unit < units.Length - 1)
            {
                size /= scale;
                unit++;
            }

            string formatted = unit == 0
                ? size.ToString("0", CultureInfo.InvariantCulture)
                : size.ToString("0.##", CultureInfo.InvariantCulture);

            return (negative ? "-" : string.Empty) + formatted + " " + units[unit];
        }

        /// <summary>
        /// Formats the value as a compact display string using K / M / B / T suffixes at the 1e3 / 1e6 / 1e9 /
        /// 1e12 thresholds (e.g. <c>1500</c> → <c>"1.5K"</c>, <c>2000000</c> → <c>"2M"</c>), keeping up to
        /// <paramref name="decimals"/> fractional digits with trailing zeros trimmed. Values below 1000 are
        /// returned as plain digits. Negative values are formatted by magnitude with a leading <c>"-"</c>.
        /// Uses the invariant culture.
        /// </summary>
        public static string ToAbbreviatedString(this long value, int decimals = 1)
        {
            bool negative = value < 0;
            double magnitude = negative ? -(double)value : value;

            string suffix;
            double scale;

            if (magnitude >= 1_000_000_000_000d) { suffix = "T"; scale = 1_000_000_000_000d; }
            else if (magnitude >= 1_000_000_000d) { suffix = "B"; scale = 1_000_000_000d; }
            else if (magnitude >= 1_000_000d) { suffix = "M"; scale = 1_000_000d; }
            else if (magnitude >= 1_000d) { suffix = "K"; scale = 1_000d; }
            else
                return value.ToString(CultureInfo.InvariantCulture);

            int fractionDigits = Math.Max(0, decimals);
            string format = fractionDigits == 0 ? "0" : "0." + new string('#', fractionDigits);
            string formatted = (magnitude / scale).ToString(format, CultureInfo.InvariantCulture);

            return (negative ? "-" : string.Empty) + formatted + suffix;
        }

        /// <summary>
        /// Converts a Unix timestamp (seconds since 1970-01-01T00:00:00Z) to a
        /// <see cref="DateTimeOffset"/> in UTC. The reverse — <c>dto.ToUnixTimeSeconds()</c>
        /// — is already a first-class BCL method on <see cref="DateTimeOffset"/>.
        /// </summary>
        public static DateTimeOffset ToDateTimeOffsetFromUnixSeconds(this long seconds)
        {
            return DateTimeOffset.FromUnixTimeSeconds(seconds);
        }

        /// <summary>
        /// Converts a Unix timestamp (milliseconds since 1970-01-01T00:00:00Z) to a
        /// <see cref="DateTimeOffset"/> in UTC. The reverse — <c>dto.ToUnixTimeMilliseconds()</c>
        /// — is already a first-class BCL method on <see cref="DateTimeOffset"/>.
        /// </summary>
        public static DateTimeOffset ToDateTimeOffsetFromUnixMilliseconds(this long milliseconds)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);
        }
    }
}
