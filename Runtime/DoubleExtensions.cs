using System.Globalization;

namespace KidzDev.Unity.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="double"/>.
    /// </summary>
    public static class DoubleExtensions
    {
        /// <summary>Clamps the value to the inclusive range <c>[0, 1]</c>.</summary>
        public static double Clamp01(this double value)
        {
            return value < 0d ? 0d : value > 1d ? 1d : value;
        }

        /// <summary>
        /// Linearly maps the value from <c>[<paramref name="inMin"/>, <paramref name="inMax"/>]</c> to
        /// <c>[<paramref name="outMin"/>, <paramref name="outMax"/>]</c>. The result is <b>not</b> clamped;
        /// returns <paramref name="outMin"/> when the input range is degenerate (zero width).
        /// </summary>
        public static double Remap(this double value, double inMin, double inMax, double outMin, double outMax)
        {
            double denominator = inMax - inMin;
            if (denominator == 0d)
                return outMin;

            return outMin + (value - inMin) * (outMax - outMin) / denominator;
        }

        /// <summary>Returns <c>true</c> when the value is within the inclusive range <c>[min, max]</c>.</summary>
        public static bool IsBetween(this double value, double min, double max)
        {
            return value >= min && value <= max;
        }

        /// <summary>
        /// Formats the value with grouped thousands (e.g. <c>1234.5</c> → <c>"1,235"</c>), keeping
        /// <paramref name="decimals"/> fractional digits. Uses the invariant culture, so the group separator
        /// is always <c>','</c> and the decimal point <c>'.'</c> regardless of the machine's locale.
        /// </summary>
        public static string ToThousandsString(this double value, int decimals = 0)
        {
            return value.ToString("N" + decimals.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
        }
    }
}
