using System.Collections.Generic;
using LogParser.Constants;
using LogParser.Exceptions;
using LogParser.Interfaces;

namespace LogParser.Sensors
{
    public static class SensorFactory
    {
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
