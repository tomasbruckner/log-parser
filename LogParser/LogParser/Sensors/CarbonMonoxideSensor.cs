using LogParser.Constants;
using LogParser.Exceptions;
using LogParser.Extensions;
using LogParser.Utils;

namespace LogParser.Sensors
{
    public class CarbonMonoxideSensor : BaseSensor
    {
        private const string Keep = "keep";
        private const string Discard = "discard";

        private const long DefaultDeviation = 3;

        private readonly long _deviation;
        private readonly double _reference;
        private string _state;

        public CarbonMonoxideSensor(string name, string referenceValue, long deviation = DefaultDeviation) : base(name)
        {
            _deviation = deviation;
            _reference = referenceValue.TryParseDoubleSensorValue(GetFullName());
        }

        public override string GetType()
        {
            return SensorTypes.CarbonMonoxideSensor;
        }

        public override void HandleValue(string referenceValue)
        {
            var number = referenceValue.TryParseLongSensorValue(GetType());

            if (!IsWithinReference(number))
            {
                _state = Discard;
            }
            else if (_state == null)
            {
                _state = Keep;
            }
        }

        private bool IsWithinReference(long value)
        {
            return MathUtils.InRange(value, _reference, _deviation);
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
