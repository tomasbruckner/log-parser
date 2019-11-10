using System;

namespace LogParser.Exceptions
{
    public class NoReadingValuesException : Exception
    {
        public NoReadingValuesException(string type) : base($"No reading values for type {type}")
        {
        }
    }
}
