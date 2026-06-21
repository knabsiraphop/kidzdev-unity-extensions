using System;
using System.Globalization;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// The time unit used by a <see cref="RelativeTime"/> value.
    /// </summary>
    public enum RelativeTimeUnit
    {
        JustNow,
        Seconds,
        Minutes,
        Hours,
        Days,
        Weeks,
        Months,
        Years,
    }

    /// <summary>
    /// A locale-agnostic relative time result — the computed <see cref="Unit"/>, <see cref="Count"/>,
    /// and <see cref="IsFuture"/> direction. Obtain one via
    /// <see cref="DateTimeOffsetExtensions.ToRelativeTime"/>; convert to a localised string in your own
    /// code using your <c>LocalizationSystem</c>:
    /// <code>
    /// var rt = timestamp.ToRelativeTime();
    /// string text = rt.Unit == RelativeTimeUnit.JustNow
    ///     ? LocalizationSystem.GetText("time_just_now")
    ///     : LocalizationSystem.GetTextFormat(
    ///           $"time_{rt.Unit}_{(rt.IsFuture ? "future" : "past")}", rt.Count);
    /// // e.g. key "time_Minutes_past" = "{0} minutes ago"
    /// </code>
    /// <see cref="ToString"/> provides an English convenience string for the non-localised case.
    /// </summary>
    public readonly struct RelativeTime
    {
        /// <summary>The coarsest meaningful time unit for this delta.</summary>
        public RelativeTimeUnit Unit { get; }

        /// <summary>
        /// Number of <see cref="Unit"/>s. Always 0 for <see cref="RelativeTimeUnit.JustNow"/>.
        /// Month and year counts use 30-day months and 365-day years (calendar approximations).
        /// </summary>
        public int Count { get; }

        /// <summary><c>true</c> when the instant is in the future relative to the reference time.</summary>
        public bool IsFuture { get; }

        public RelativeTime(RelativeTimeUnit unit, int count, bool isFuture)
        {
            Unit = unit;
            Count = count;
            IsFuture = isFuture;
        }

        /// <summary>
        /// Buckets a signed delta (<c>value − now</c>; positive ⇒ future) into the coarsest
        /// meaningful unit. Thresholds on the absolute duration:
        /// &lt;10 s → JustNow · &lt;60 s → Seconds · &lt;60 min → Minutes · &lt;24 h → Hours ·
        /// &lt;7 d → Days · &lt;30 d → Weeks · &lt;365 d → Months · else Years.
        /// Month and year counts are approximations (30-day months, 365-day years).
        /// </summary>
        public static RelativeTime FromDelta(TimeSpan delta)
        {
            bool future = delta > TimeSpan.Zero;
            double totalSeconds = Math.Abs(delta.TotalSeconds);
            double totalMinutes = Math.Abs(delta.TotalMinutes);
            double totalHours   = Math.Abs(delta.TotalHours);
            double totalDays    = Math.Abs(delta.TotalDays);

            if (totalSeconds < 10)
                return new RelativeTime(RelativeTimeUnit.JustNow, 0, future);

            if (totalSeconds < 60)
                return new RelativeTime(RelativeTimeUnit.Seconds, (int)totalSeconds, future);

            if (totalMinutes < 60)
                return new RelativeTime(RelativeTimeUnit.Minutes, (int)totalMinutes, future);

            if (totalHours < 24)
                return new RelativeTime(RelativeTimeUnit.Hours, (int)totalHours, future);

            if (totalDays < 7)
                return new RelativeTime(RelativeTimeUnit.Days, (int)totalDays, future);

            if (totalDays < 30)
                return new RelativeTime(RelativeTimeUnit.Weeks, (int)(totalDays / 7), future);

            if (totalDays < 365)
                return new RelativeTime(RelativeTimeUnit.Months, (int)(totalDays / 30), future);

            return new RelativeTime(RelativeTimeUnit.Years, (int)(totalDays / 365), future);
        }

        /// <summary>
        /// English convenience string. For localised output use the struct fields directly with
        /// your <c>LocalizationSystem</c> (see class-level docs).
        /// <list type="bullet">
        ///   <item><description><c>JustNow</c> → <c>"just now"</c></description></item>
        ///   <item><description><c>Days</c> + Count 1, past → <c>"yesterday"</c></description></item>
        ///   <item><description><c>Days</c> + Count 1, future → <c>"tomorrow"</c></description></item>
        ///   <item><description>Past: <c>"5 minutes ago"</c> (singular: <c>"1 minute ago"</c>)</description></item>
        ///   <item><description>Future: <c>"in 5 minutes"</c> (singular: <c>"in 1 minute"</c>)</description></item>
        /// </list>
        /// </summary>
        public override string ToString()
        {
            if (Unit == RelativeTimeUnit.JustNow)
                return "just now";

            if (Unit == RelativeTimeUnit.Days && Count == 1)
                return IsFuture ? "tomorrow" : "yesterday";

            string unitName = UnitName(Unit, Count);

            return IsFuture
                ? "in " + Count.ToString(CultureInfo.InvariantCulture) + " " + unitName
                : Count.ToString(CultureInfo.InvariantCulture) + " " + unitName + " ago";
        }

        private static string UnitName(RelativeTimeUnit unit, int count)
        {
            bool plural = count != 1;
            switch (unit)
            {
                case RelativeTimeUnit.Seconds: return plural ? "seconds" : "second";
                case RelativeTimeUnit.Minutes: return plural ? "minutes" : "minute";
                case RelativeTimeUnit.Hours:   return plural ? "hours"   : "hour";
                case RelativeTimeUnit.Days:    return plural ? "days"    : "day";
                case RelativeTimeUnit.Weeks:   return plural ? "weeks"   : "week";
                case RelativeTimeUnit.Months:  return plural ? "months"  : "month";
                case RelativeTimeUnit.Years:   return plural ? "years"   : "year";
                default:                       return unit.ToString().ToLowerInvariant();
            }
        }
    }
}
