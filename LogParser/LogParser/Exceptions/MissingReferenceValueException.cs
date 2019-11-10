using System;

namespace LogParser.Exceptions
{
    public class MissingReferenceValueException : Exception
    {
        public MissingReferenceValueException(string sensorType) : base($"Missing reference value for {sensorType}")
        {
        }
    }
}
