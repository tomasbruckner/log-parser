using System;
using System.Collections.Generic;
using System.Linq;
using LogParser.Exceptions;

namespace LogParser.ReferenceValue
{
    public static class ReferenceValueParser
    {
        private const string Reference = "reference";
        private const string Delimiter = " ";

        /// <summary>
        ///     Parses reference values for sensors.
        /// </summary>
        /// <param name="line">Reference line parsed from log.</param>
        /// <returns>Map of sensor types and their reference values</returns>
        /// <exception cref="InvalidReferenceFormatException"></exception>
        public static IDictionary<string, string> GetReferenceValues(string line)
        {
            var parts = TryParseFormat(line);
            var references = new Dictionary<string, string>();

            for (var i = 1; i < parts.Count(); i += 2)
            {
                var key = parts[i - 1];
                if (references.ContainsKey(key))
                {
                    throw new InvalidReferenceFormatException(InvalidReferenceFormatException.DuplicateSensorType);
                }

                references.Add(key, parts[i]);
            }


            return references;
        }

        /// <summary>
        ///     Validates correct reference format. Correct format should be:
        ///     reference [sensor_type reference_value]+
        /// </summary>
        /// <param name="referenceLine">Reference information</param>
        /// <returns></returns>
        /// <exception cref="InvalidReferenceFormatException"></exception>
        private static string[] TryParseFormat(string referenceLine)
        {
            if (referenceLine == null)
            {
                throw new InvalidReferenceFormatException(InvalidReferenceFormatException.MissingReference);
            }

            var parts = referenceLine.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 3 || parts.Length % 2 == 0)
            {
                throw new InvalidReferenceFormatException(InvalidReferenceFormatException.InvalidNumberOfParameters);
            }

            if (parts[0] != Reference)
            {
                throw new InvalidReferenceFormatException(InvalidReferenceFormatException.MissingReferenceKeyword);
            }

            return parts
                .Skip(1)
                .Select(o => o.Trim())
                .ToArray();
        }
    }
}
