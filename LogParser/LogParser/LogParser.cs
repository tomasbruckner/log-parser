using System;
using System.Collections.Generic;
using System.IO;
using LogParser.Enums;
using LogParser.Exceptions;
using LogParser.Extensions;
using LogParser.Interfaces;
using LogParser.ReferenceValue;
using LogParser.Sensors;
using Newtonsoft.Json;

namespace LogParser
{
    public static class LogParser
    {
        private const string Delimiter = " ";

        /// <summary>
        ///     Parses sensor type logs and returns information about their quality.
        ///     Correct format:
        ///     reference [sensor_type reference_value]+
        ///     [sensor_type sensor_name
        ///     [date value]+]+
        /// </summary>
        /// <param name="content">Log to parse</param>
        /// <returns>Information about quality of the sensor types in JSON format.</returns>
        /// <exception cref="InvalidReferenceFormatException"></exception>
        /// <exception cref="InvalidSensorTypeException"></exception>
        /// <exception cref="InvalidSensorValueException"></exception>
        /// <exception cref="MissingReferenceValueException"></exception>
        /// <exception cref="MissingSensorDefinitionException"></exception>
        /// <exception cref="NoReadingValuesException"></exception>
        public static string Parse(string content)
        {
            using (var reader = new StringReader(content))
            {
                return Parse(() => reader.ReadLine());
            }
        }

        /// <summary>
        ///     Parses sensor type logs and returns information about their quality.
        ///     Correct format:
        ///     reference [sensor_type reference_value]+
        ///     [sensor_type sensor_name
        ///     [date value]+]+
        /// </summary>
        /// <param name="filePath">Path to file</param>
        /// <returns>Information about quality of the sensor types in JSON format.</returns>
        /// <exception cref="InvalidReferenceFormatException"></exception>
        /// <exception cref="InvalidSensorTypeException"></exception>
        /// <exception cref="InvalidSensorValueException"></exception>
        /// <exception cref="MissingReferenceValueException"></exception>
        /// <exception cref="MissingSensorDefinitionException"></exception>
        /// <exception cref="NoReadingValuesException"></exception>
        public static string ParseFile(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                return Parse(() => reader.ReadLine());
            }
        }

        /// <summary>
        ///     Parses sensor type logs and returns information about their quality.
        ///     Correct format:
        ///     reference [sensor_type reference_value]+
        ///     [sensor_type sensor_name
        ///     [date value]+]+
        /// </summary>
        /// <param name="fileStream">Log to parse</param>
        /// <returns>Information about quality of the sensor types in JSON format.</returns>
        /// <exception cref="InvalidReferenceFormatException"></exception>
        /// <exception cref="InvalidSensorTypeException"></exception>
        /// <exception cref="InvalidSensorValueException"></exception>
        /// <exception cref="MissingReferenceValueException"></exception>
        /// <exception cref="MissingSensorDefinitionException"></exception>
        /// <exception cref="NoReadingValuesException"></exception>
        public static string Parse(FileStream fileStream)
        {
            using (var reader = new StreamReader(fileStream))
            {
                return Parse(() => reader.ReadLine());
            }
        }

        /// <summary>
        ///     Parsed log and returns information about sensors defined inside log.
        /// </summary>
        /// <param name="readNextLine">Generic function for parsing any stream.</param>
        /// <returns>Parsed log</returns>
        /// <exception cref="InvalidReferenceFormatException"></exception>
        /// <exception cref="InvalidSensorTypeException"></exception>
        /// <exception cref="InvalidSensorValueException"></exception>
        /// <exception cref="MissingReferenceValueException"></exception>
        /// <exception cref="MissingSensorDefinitionException"></exception>
        /// <exception cref="NoReadingValuesException"></exception>
        private static string Parse(Func<string> readNextLine)
        {
            var referenceMap = ReferenceValueParser.GetReferenceValues(readNextLine());
            var result = new Dictionary<string, string>();
            ISensor activeSensor = null;

            string line;
            while ((line = readNextLine()) != null)
            {
                if (line.Trim() == "")
                {
                    continue;
                }

                var (state, part1, part2) = TryNextState(line);

                switch (state)
                {
                    case ParseStateEnum.Sensor:
                        AddToResult(activeSensor, result);
                        activeSensor = SensorFactory.Create(referenceMap, part1, part2);
                        break;

                    case ParseStateEnum.Value:
                        if (activeSensor == null)
                        {
                            throw new MissingSensorDefinitionException();
                        }

                        activeSensor.HandleValue(part2);
                        break;

                    default:
                        throw new InvalidSensorValueException(line);
                }
            }

            AddToResult(activeSensor, result);

            return JsonConvert.SerializeObject(result);
        }

        /// <summary>
        ///     Adds information about sensor quality.
        /// </summary>
        /// <param name="sensor">Sensor instance</param>
        /// <param name="result">Map of results</param>
        private static void AddToResult(ISensor sensor, IDictionary<string, string> result)
        {
            if (sensor != null)
            {
                result.Add(
                    sensor.GetName(),
                    sensor.CalculateQuality()
                );
            }
        }

        /// <summary>
        ///     Decides next state of parsing automata.
        /// </summary>
        /// <param name="line">One line of input from log.</param>
        /// <returns>Next state of current automata.</returns>
        /// <exception cref="InvalidSensorValueException"></exception>
        private static (ParseStateEnum, string, string) TryNextState(string line)
        {
            var parts = line.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
            {
                throw new InvalidSensorValueException(line);
            }

            var state = parts[0].IsDateFormat() ? ParseStateEnum.Value : ParseStateEnum.Sensor;

            return (state, parts[0].Trim(), parts[1].Trim());
        }
    }
}
