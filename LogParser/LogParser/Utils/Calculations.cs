using System;
using System.Collections.Generic;
using System.Linq;

namespace LogParser.Utils
{
    public static class Calculations
    {
        public static double CalculateStdDev(ICollection<double> enumerable, double? average = null)
        {
            var ret = 0.0;
            if (!enumerable.Any())
            {
                return ret;
            }

            var avg = average ?? enumerable.Average();
            var sum = enumerable.Sum(d => Math.Pow(d - avg, 2));
            ret = Math.Sqrt((sum) / (enumerable.Count() - 1));

            return ret;
        }
        
        public static bool InRange(double value, double reference, double deviation)
        {
            return Math.Abs(reference - value) <= deviation;
        }
        
        public static bool InRange(decimal value, decimal reference, decimal deviation)
        {
            return Math.Abs(reference - value) <= deviation;
        }
        
        public static bool InRange(long value, long reference, long deviation)
        {
            return Math.Abs(reference - value) <= deviation;
        }

        public static double CalculateAverage(double sum, long count)
        {
            return sum / count;
        }
    }
}
