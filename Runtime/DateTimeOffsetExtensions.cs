using System;
using System.Globalization;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DateTimeOffset"/>.
    /// </summary>
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// Returns <c>true</c> when the value falls within the inclusive range
        /// <c>[<paramref name="start"/>, <paramref name="end"/>]</c>. Comparisons are
        /// offset-aware (values are normalised to UTC before comparing), so two instants at
        /// different offsets that represent the same moment compare as equal.
        /// </summary>
        public static bool IsBetween(this DateTimeOffset value, DateTimeOffset start, DateTimeOffset end)
        {
            return value >= start && value <= end;
        }

        /// <summary>
        /// Returns <c>true</c> when both values fall on the same calendar date <em>using each
        /// value's own UTC offset</em>. If the two values may carry different offsets, normalise
        /// them first (e.g. <c>.ToUniversalTime()</c>) to compare against the same timezone.
        /// Useful for "already claimed today" daily-reward checks when all values share the same
        /// regional offset.
        /// </summary>
        public static bool IsSameDay(this DateTimeOffset value, DateTimeOffset other)
        {
            return value.Date == other.Date;
        }

        /// <summary>
        /// Returns midnight at the start of the day, preserving the value's UTC offset.
        /// </summary>
        public static DateTimeOffset StartOfDay(this DateTimeOffset value)
        {
            return new DateTimeOffset(value.Date, value.Offset);
        }

        /// <summary>
        /// Returns the very last tick of the day (one tick before the next midnight), preserving
        /// the value's UTC offset. Pairs with <see cref="StartOfDay"/>.
        /// </summary>
        public static DateTimeOffset EndOfDay(this DateTimeOffset value)
        {
            return value.StartOfDay().AddDays(1).AddTicks(-1);
        }

        /// <summary>
        /// Returns midnight at the start of the week that contains this value, preserving the
        /// value's UTC offset. <paramref name="firstDayOfWeek"/> defaults to
        /// <see cref="DayOfWeek.Monday"/> (the convention used by most non-US weekly resets);
        /// pass <see cref="DayOfWeek.Sunday"/> for Sunday-week systems.
        /// </summary>
        public static DateTimeOffset StartOfWeek(this DateTimeOffset value, DayOfWeek firstDayOfWeek = DayOfWeek.Monday)
        {
            int diff = ((int)value.DayOfWeek - (int)firstDayOfWeek + 7) % 7;
            return value.StartOfDay().AddDays(-diff);
        }

        /// <summary>
        /// Returns midnight on the first day of the month, preserving the value's UTC offset.
        /// </summary>
        public static DateTimeOffset StartOfMonth(this DateTimeOffset value)
        {
            return new DateTimeOffset(value.Year, value.Month, 1, 0, 0, 0, value.Offset);
        }

        /// <summary>
        /// Returns the very last tick of the month, preserving the value's UTC offset.
        /// Pairs with <see cref="StartOfMonth"/>.
        /// </summary>
        public static DateTimeOffset EndOfMonth(this DateTimeOffset value)
        {
            return value.StartOfMonth().AddMonths(1).AddTicks(-1);
        }

        /// <summary>
        /// Formats the value as a round-trippable ISO 8601 string (e.g.
        /// <c>"2026-06-21T14:30:00.0000000+07:00"</c>) using the invariant culture. Safe for
        /// serialisation to save files, logs, and network payloads — no locale ambiguity.
        /// </summary>
        public static string ToIso8601String(this DateTimeOffset value)
        {
            return value.ToString("o", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Computes a locale-agnostic <see cref="RelativeTime"/> value (unit + count + direction)
        /// relative to <paramref name="now"/> (defaults to <see cref="DateTimeOffset.UtcNow"/>).
        /// Use the returned struct's fields to build a localised string with your own localization
        /// system — for example:
        /// <code>
        /// var rt = timestamp.ToRelativeTime();
        /// string text = rt.Unit == RelativeTimeUnit.JustNow
        ///     ? LocalizationSystem.GetText("time_just_now")
        ///     : LocalizationSystem.GetTextFormat(
        ///           $"time_{rt.Unit}_{(rt.IsFuture ? "future" : "past")}", rt.Count);
        /// // e.g. key "time_Minutes_past" = "{0} minutes ago"
        /// </code>
        /// For an English convenience string, use <see cref="ToRelativeString"/> instead.
        /// </summary>
        public static RelativeTime ToRelativeTime(this DateTimeOffset value, DateTimeOffset? now = null)
        {
            return RelativeTime.FromDelta(value - (now ?? DateTimeOffset.UtcNow));
        }

        /// <summary>
        /// Returns an English relative-time string (e.g. <c>"5 minutes ago"</c>, <c>"in 2 hours"</c>,
        /// <c>"yesterday"</c>, <c>"just now"</c>). For localised output, use <see cref="ToRelativeTime"/>
        /// and map the returned struct to your localization keys.
        /// </summary>
        public static string ToRelativeString(this DateTimeOffset value, DateTimeOffset? now = null)
        {
            return value.ToRelativeTime(now).ToString();
        }

        /// <summary>
        /// Formats the date as <c>"MMM d, yyyy"</c> (e.g. <c>"Jun 21, 2026"</c>) using
        /// <paramref name="provider"/> (defaults to <see cref="CultureInfo.InvariantCulture"/>).
        /// Pass a <see cref="CultureInfo"/> mapped from your app's current language to get
        /// localised month names (e.g. <c>new CultureInfo("th-TH")</c>).
        /// </summary>
        public static string ToFriendlyDateString(this DateTimeOffset value, IFormatProvider provider = null)
        {
            return value.ToString("MMM d, yyyy", provider ?? CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Formats the date and time as <c>"MMM d, yyyy HH:mm"</c> (e.g. <c>"Jun 21, 2026 14:30"</c>)
        /// using <paramref name="provider"/> (defaults to <see cref="CultureInfo.InvariantCulture"/>).
        /// Pass a <see cref="CultureInfo"/> mapped from your app's current language to get
        /// localised month names.
        /// </summary>
        public static string ToFriendlyDateTimeString(this DateTimeOffset value, IFormatProvider provider = null)
        {
            return value.ToString("MMM d, yyyy HH:mm", provider ?? CultureInfo.InvariantCulture);
        }
    }
}
