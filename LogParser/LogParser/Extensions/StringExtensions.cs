using System;
using System.Globalization;
using LogParser.Exceptions;

namespace LogParser.Extensions
{
    public static class StringExtensions
    {
        public static bool IsDateFormat(this string input)
        {
            return DateTime.TryParse(input, out _);
        }

        public static decimal TryParseDecimalSensorValue(this string input, string sensor)
        {
            return TryParse(input, sensor, () => decimal.Parse(input, CultureInfo.InvariantCulture));
        }

        public static double TryParseDoubleSensorValue(this string input, string sensor)
        {
            return TryParse(input, sensor, () => double.Parse(input, CultureInfo.InvariantCulture));
        }

        private static T TryParse<T>(string input, string sensor, Func<T> parse)
        {
            try
            {
                return parse();
            }
            catch (Exception)
            {
                throw new InvalidSensorValueException(sensor, input);
            }
        }
    }
}
