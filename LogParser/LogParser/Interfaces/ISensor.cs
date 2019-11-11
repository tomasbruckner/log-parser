using LogParser.Exceptions;

namespace LogParser.Interfaces
{
    public interface ISensor
    {
        /// <summary>
        ///     Calculates final quality of the specified sensor instance.
        /// </summary>
        /// <returns>Quality of sensor</returns>
        /// <exception cref="NoReadingValuesException"></exception>
        string CalculateQuality();

        /// <summary>
        ///     Returns name of the sensor type.
        /// </summary>
        /// <returns>Sensor type</returns>
        string GetType();

        /// <summary>
        ///     Returns name of the sensor.
        /// </summary>
        /// <returns>Name of sensor</returns>
        string GetName();

        /// <summary>
        ///     Handle one value from log.
        /// </summary>
        /// <param name="referenceValue">Value parsed from log</param>
        /// <exception cref="InvalidSensorValueException"></exception>
        void HandleValue(string referenceValue);
    }
}
