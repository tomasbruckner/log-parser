using System;

namespace LogParser.Exceptions
{
    public class InvalidReferenceFormatException: Exception
    {
        public const string MissingReference = "Missing reference";
        public const string MissingReferenceKeyword = "Missing reference keyword";
        public const string InvalidNumberOfParameters = "Invalid number of parameters";
        public const string DuplicateSensorType = "Duplicate sensor type";
        
        public InvalidReferenceFormatException(string message) : base(message)
        {
        }
    }
}
