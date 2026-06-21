using System;
using System.Globalization;
using System.Text;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="TimeSpan"/>.
    /// </summary>
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Formats the duration as a digital clock string for timer and cooldown displays.
        /// Negative values are treated as zero. When the total duration is under one hour the
        /// hours segment is omitted — e.g. <c>1:05:09</c> (over one hour) or <c>2:30</c> (under
        /// one hour). Days roll into the hours digit, so a 25-hour span shows <c>25:00:00</c>.
        /// </summary>
        public static string ToClockString(this TimeSpan value)
        {
            if (value < TimeSpan.Zero)
                value = TimeSpan.Zero;

            int totalHours = (int)value.TotalHours;
            int minutes = value.Minutes;
            int seconds = value.Seconds;

            if (totalHours > 0)
                return totalHours.ToString(CultureInfo.InvariantCulture)
                    + ":" + minutes.ToString("00", CultureInfo.InvariantCulture)
                    + ":" + seconds.ToString("00", CultureInfo.InvariantCulture);

            return minutes.ToString(CultureInfo.InvariantCulture)
                + ":" + seconds.ToString("00", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats the duration as a compact human-readable string showing the two most-significant
        /// non-zero units from d / h / m / s — e.g. <c>"2d 3h"</c>, <c>"1h 30m"</c>,
        /// <c>"5m 10s"</c>, <c>"45s"</c>. <see cref="TimeSpan.Zero"/> returns <c>"0s"</c>.
        /// Negative durations are formatted by magnitude with a leading <c>"-"</c>.
        /// Useful for "ready in …" / "refills in …" UI labels.
        /// </summary>
        public static string ToCompactString(this TimeSpan value)
        {
            bool negative = value < TimeSpan.Zero;
            if (negative)
                value = value.Negate();

            int days = (int)value.TotalDays;
            int hours = value.Hours;
            int minutes = value.Minutes;
            int seconds = value.Seconds;

            var sb = new StringBuilder();

            if (days > 0)
            {
                sb.Append(days.ToString(CultureInfo.InvariantCulture)).Append('d');
                if (hours > 0)
                    sb.Append(' ').Append(hours.ToString(CultureInfo.InvariantCulture)).Append('h');
            }
            else if (hours > 0)
            {
                sb.Append(hours.ToString(CultureInfo.InvariantCulture)).Append('h');
                if (minutes > 0)
                    sb.Append(' ').Append(minutes.ToString(CultureInfo.InvariantCulture)).Append('m');
            }
            else if (minutes > 0)
            {
                sb.Append(minutes.ToString(CultureInfo.InvariantCulture)).Append('m');
                if (seconds > 0)
                    sb.Append(' ').Append(seconds.ToString(CultureInfo.InvariantCulture)).Append('s');
            }
            else
            {
                sb.Append(seconds.ToString(CultureInfo.InvariantCulture)).Append('s');
            }

            return negative ? "-" + sb : sb.ToString();
        }

        /// <summary>
        /// Clamps the duration to the inclusive range
        /// <c>[<paramref name="min"/>, <paramref name="max"/>]</c>.
        /// </summary>
        public static TimeSpan Clamp(this TimeSpan value, TimeSpan min, TimeSpan max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        /// <summary>
        /// Returns <c>true</c> when the duration falls within the inclusive range
        /// <c>[<paramref name="min"/>, <paramref name="max"/>]</c>. Consistent with
        /// <c>IsBetween</c> on the numeric extension types.
        /// </summary>
        public static bool IsBetween(this TimeSpan value, TimeSpan min, TimeSpan max)
        {
            return value >= min && value <= max;
        }
    }
}
