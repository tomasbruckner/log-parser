using LogParser.Exceptions;
using LogParser.Interfaces;

namespace LogParser.Sensors
{
    public abstract class BaseSensor : ISensor
    {
        private readonly string _name;

        protected BaseSensor(string name)
        {
            _name = name;
        }

        /// <summary>
        ///     Calculates final quality of the specified sensor instance.
        /// </summary>
        /// <returns>Quality of sensor</returns>
        /// <exception cref="NoReadingValuesException"></exception>
        public abstract string CalculateQuality();

        /// <summary>
        ///     Returns name of the sensor type.
        /// </summary>
        /// <returns>Sensor type</returns>
        public new abstract string GetType();

        /// <summary>
        ///     Returns name of the sensor.
        /// </summary>
        /// <returns>Name of sensor</returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        ///     Handle one value from log.
        /// </summary>
        /// <param name="referenceValue">Value parsed from log</param>
        /// <exception cref="InvalidSensorValueException"></exception>
        public abstract void HandleValue(string referenceValue);

        /// <summary>
        ///     Returns type and name of the sensor.
        /// </summary>
        /// <returns>Sensor name and type</returns>
        protected string GetFullName()
        {
            return $"{GetType()} - {GetName()}";
        }
    }
}
