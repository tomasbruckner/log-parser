using LogParser.Constants;
using LogParser.Exceptions;
using LogParser.Extensions;
using LogParser.Utils;

namespace LogParser.Sensors
{
    public class HumiditySensor : BaseSensor
    {
        private const string Keep = "keep";
        private const string Discard = "discard";

        private const decimal DefaultDeviation = 1;

        private readonly decimal _deviation;
        private readonly decimal _reference;
        private string _state;


        public HumiditySensor(string name, string referenceValue, decimal deviation = DefaultDeviation) : base(name)
        {
            _deviation = deviation;
            _reference = referenceValue.TryParseDecimalSensorValue(GetFullName());
        }

        public override string GetType()
        {
            return SensorTypes.HumiditySensor;
        }

        public override void HandleValue(string value)
        {
            var number = value.TryParseDecimalSensorValue(GetFullName());
            if (!IsWithinReference(number))
            {
                _state = Discard;
            }
            else if (_state == null)
            {
                _state = Keep;
            }
        }

        private bool IsWithinReference(decimal value)
        {
            return Calculations.InRange(value, _reference, _deviation);
        }

        public override string CalculateQuality()
        {
            if (_state == null)
            {
                throw new NoReadingValuesException(GetType());
            }

            return _state;
        }
    }
}
