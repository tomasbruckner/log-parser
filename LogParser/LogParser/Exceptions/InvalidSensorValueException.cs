using System;

namespace LogParser.Exceptions
{
    public class InvalidSensorValueException : Exception
    {
        public InvalidSensorValueException(string line) : base($"Invalid line: {line}")
        {
        }

        public InvalidSensorValueException(string sensorType, string value) : base(
            $"Invalid value: {value} for sensor: {sensorType}"
        )
        {
        }
    }
}
