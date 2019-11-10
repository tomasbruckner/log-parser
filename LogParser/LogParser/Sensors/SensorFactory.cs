using System.Collections.Generic;
using LogParser.Constants;
using LogParser.Exceptions;
using LogParser.Interfaces;

namespace LogParser.Sensors
{
    public static class SensorFactory
    {
        /// <summary>
        ///     Factory for sensor types
        /// </summary>
        /// <param name="referenceMap">Map of reference values for each sensor</param>
        /// <param name="type">Current sensor type</param>
        /// <param name="name">Name of sensor</param>
        /// <returns>Instance of correct sensor</returns>
        /// <exception cref="MissingReferenceValueException"></exception>
        /// <exception cref="InvalidSensorTypeException"></exception>
        public static ISensor Create(IDictionary<string, string> referenceMap, string type, string name)
        {
            if (!referenceMap.ContainsKey(type))
            {
                throw new MissingReferenceValueException(type);
            }

            var referenceValue = referenceMap[type];

            switch (type)
            {
                case SensorTypes.HumiditySensor:
                    return new HumiditySensor(name, referenceValue);
                case SensorTypes.CarbonMonoxideSensor:
                    return new CarbonMonoxideSensor(name, referenceValue);
                case SensorTypes.ThermometerSensor:
                    return new ThermometerSensor(name, referenceValue);
                default:
                    throw new InvalidSensorTypeException();
            }
        }
    }
}
