using System;
using System.Collections.Generic;
using System.Linq;

namespace LogParser.Utils
{
    public static class MathUtils
    {
        /// <summary>
        ///     Calculates standard deviation
        /// </summary>
        /// <param name="numbers">Number to be used for calculation</param>
        /// <param name="average">Average of the numbers</param>
        /// <returns>Standard deviation of specified numbers</returns>
        public static double CalculateStdDev(ICollection<double> numbers, double? average = null)
        {
            var ret = 0.0;
            if (!numbers.Any())
            {
                return ret;
            }

            var avg = average ?? numbers.Average();
            var sum = numbers.Sum(d => Math.Pow(d - avg, 2));
            ret = Math.Sqrt(sum / (numbers.Count() - 1));

            return ret;
        }

        /// <summary>
        ///     Checks if value is within range from reference +/- deviation
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="reference">Reference value</param>
        /// <param name="deviation">Deviation from reference value</param>
        /// <returns></returns>
        public static bool InRange(double value, double reference, double deviation)
        {
            return Math.Abs(reference - value) <= deviation;
        }

        /// <summary>
        ///     Checks if value is within range from reference +/- deviation
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <param name="reference">Reference value</param>
        /// <param name="deviation">Deviation from reference value</param>
        /// <returns></returns>
        public static bool InRange(decimal value, decimal reference, decimal deviation)
        {
            return Math.Abs(reference - value) <= deviation;
        }
    }
}
