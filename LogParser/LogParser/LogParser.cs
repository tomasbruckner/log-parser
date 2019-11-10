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

        public static string Parse(string content)
        {
            using (var reader = new StringReader(content))
            {
                var parsedLog = Parse(() => reader.ReadLine());

                return JsonConvert.SerializeObject(parsedLog);
            }
        }

        private static Dictionary<string, string> Parse(Func<string> readNextLine)
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

            return result;
        }

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
