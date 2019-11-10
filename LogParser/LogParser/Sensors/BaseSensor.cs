using System.Collections.Generic;
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

        protected string GetFullName()
        {
            return $"{GetType()} - {GetName()}";
        }

        public abstract string CalculateQuality();

        public new abstract string GetType();

        public string GetName()
        {
            return _name;
        }

        public abstract void HandleValue(string value);
    }
}
