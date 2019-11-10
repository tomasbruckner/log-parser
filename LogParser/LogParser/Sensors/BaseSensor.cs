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
        /// <returns></returns>
        public abstract string CalculateQuality();

        /// <summary>
        ///     Returns name of the sensor type.
        /// </summary>
        /// <returns></returns>
        public new abstract string GetType();

        /// <summary>
        ///     Returns name of the sensor.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        ///     Handle one value from log.
        /// </summary>
        /// <param name="value">Value parsed from log</param>
        public abstract void HandleValue(string value);

        /// <summary>
        ///     Returns type and name of the sensor.
        /// </summary>
        /// <returns></returns>
        protected string GetFullName()
        {
            return $"{GetType()} - {GetName()}";
        }
    }
}
