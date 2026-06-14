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
    }
}
