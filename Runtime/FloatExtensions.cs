using System.Globalization;
using UnityEngine;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="float"/>.
    /// </summary>
    public static class FloatExtensions
    {
        /// <summary>
        /// Tolerant float equality via <see cref="Mathf.Approximately"/> — use this instead of <c>==</c>,
        /// which is unreliable for values produced by floating-point arithmetic.
        /// </summary>
        public static bool Approximately(this float value, float other)
        {
            return Mathf.Approximately(value, other);
        }

        /// <summary>Returns <c>true</c> when the value is within <paramref name="epsilon"/> of zero.</summary>
        public static bool IsZero(this float value, float epsilon = 1e-6f)
        {
            return Mathf.Abs(value) <= epsilon;
        }

        /// <summary>Clamps the value to the inclusive range <c>[0, 1]</c>.</summary>
        public static float Clamp01(this float value)
        {
            return Mathf.Clamp01(value);
        }

        /// <summary>
        /// Linearly maps the value from the input range <c>[<paramref name="inMin"/>, <paramref name="inMax"/>]</c>
        /// to the output range <c>[<paramref name="outMin"/>, <paramref name="outMax"/>]</c>. The result is
        /// <b>not</b> clamped — values outside the input range extrapolate. Returns <paramref name="outMin"/>
        /// when the input range is degenerate (zero width).
        /// </summary>
        public static float Remap(this float value, float inMin, float inMax, float outMin, float outMax)
        {
            float denominator = inMax - inMin;
            if (Mathf.Approximately(denominator, 0f))
                return outMin;

            return outMin + (value - inMin) * (outMax - outMin) / denominator;
        }

        /// <summary>
        /// Rounds the value to the nearest multiple of <paramref name="step"/> (grid snapping). Returns the
        /// value unchanged when <paramref name="step"/> is zero.
        /// </summary>
        public static float Snap(this float value, float step)
        {
            if (step == 0f)
                return value;

            return Mathf.Round(value / step) * step;
        }

        /// <summary>Rounds to the nearest <see cref="int"/> (banker's rounding via <see cref="Mathf.RoundToInt"/>).</summary>
        public static int RoundToInt(this float value)
        {
            return Mathf.RoundToInt(value);
        }

        /// <summary>Returns the largest <see cref="int"/> less than or equal to the value.</summary>
        public static int FloorToInt(this float value)
        {
            return Mathf.FloorToInt(value);
        }

        /// <summary>Returns the smallest <see cref="int"/> greater than or equal to the value.</summary>
        public static int CeilToInt(this float value)
        {
            return Mathf.CeilToInt(value);
        }

        /// <summary>Returns <c>true</c> when the value is within the inclusive range <c>[min, max]</c>.</summary>
        public static bool IsBetween(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Formats the value with grouped thousands (e.g. <c>1234.5</c> → <c>"1,235"</c>), keeping
        /// <paramref name="decimals"/> fractional digits. Uses the invariant culture, so the group separator
        /// is always <c>','</c> and the decimal point <c>'.'</c> regardless of the machine's locale.
        /// </summary>
        public static string ToThousandsString(this float value, int decimals = 0)
        {
            return value.ToString("N" + decimals.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
        }
    }
}
