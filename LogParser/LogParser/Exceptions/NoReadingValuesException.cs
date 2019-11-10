using System;

namespace LogParser.Exceptions
{
    public class NoReadingValues : Exception
    {
        public NoReadingValues(string type) : base($"No reading values for type {type}")
        {
        }
    }
}
