using System;
using System.Globalization;
using LogParser.Exceptions;

namespace LogParser.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Checks if string is in correct datetime format
        /// </summary>
        /// <param name="input">date as string</param>
        /// <returns></returns>
        public static bool IsDateFormat(this string input)
        {
            return DateTime.TryParse(input, out _);
        }

        /// <summary>
        ///     Parses decimal sensor value
        /// </summary>
        /// <param name="input">Decimal as string</param>
        /// <param name="sensor">Sensor type</param>
        /// <returns>Parsed decimal</returns>
        /// <exception cref="InvalidSensorValueException"></exception>
        public static decimal TryParseDecimalSensorValue(this string input, string sensor)
        {
            return TryParse(input, sensor, () => decimal.Parse(input, CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Parsed double sensor value
        /// </summary>
        /// <param name="input">Double as string</param>
        /// <param name="sensor">Sensor type</param>
        /// <returns>Parsed double</returns>
        /// <exception cref="InvalidSensorValueException"></exception>
        public static double TryParseDoubleSensorValue(this string input, string sensor)
        {
            return TryParse(input, sensor, () => double.Parse(input, CultureInfo.InvariantCulture));
        }

        /// <summary>
        ///     Generic function for parsing string
        /// </summary>
        /// <param name="input">To be parsed string valued</param>
        /// <param name="sensor">Sensor type</param>
        /// <param name="parse">Parsing function</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidSensorValueException"></exception>
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
