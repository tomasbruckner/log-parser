using System.Collections.Generic;
using System.Linq;
using LogParser.Constants;
using LogParser.Exceptions;
using LogParser.Extensions;
using LogParser.Utils;

namespace LogParser.Sensors
{
    public class ThermometerSensor : BaseSensor
    {
        private const string UltraPrecise = "ultra precise";
        private const string VeryPrecise = "very precise";
        private const string Precise = "precise";

        private const double DefaultAllowedDeviation = 0.5;
        private const double DefaultUltraPreciseDeviation = 3;
        private const double DefaultVeryPreciseDeviation = 5;

        private readonly double _allowedDeviation;

        private readonly ICollection<double> _readingValues = new List<double>();
        private readonly double _reference;
        private readonly double _ultraPreciseDeviation;
        private readonly double _veryPreciseDeviation;

        public ThermometerSensor(
            string name,
            string referenceValue,
            double allowedDeviation = DefaultAllowedDeviation,
            double ultraPreciseDeviation = DefaultUltraPreciseDeviation,
            double veryPreciseDeviation = DefaultVeryPreciseDeviation
        ) : base(name)
        {
            _reference = referenceValue.TryParseDoubleSensorValue(GetFullName());
            _allowedDeviation = allowedDeviation;
            _veryPreciseDeviation = veryPreciseDeviation;
            _ultraPreciseDeviation = ultraPreciseDeviation;
        }

        public override string GetType()
        {
            return SensorTypes.ThermometerSensor;
        }

        public override void HandleValue(string value)
        {
            var number = value.TryParseDoubleSensorValue(GetFullName());
            _readingValues.Add(number);
        }

        public override string CalculateQuality()
        {
            if (!_readingValues.Any())
            {
                throw new NoReadingValuesException(GetType());
            }

            var average = _readingValues.Average();
            var isMean = Calculations.InRange(average, _reference, _allowedDeviation);
            var deviation = Calculations.CalculateStdDev(_readingValues);

            if (!isMean || deviation >= _veryPreciseDeviation)
            {
                return Precise;
            }

            return deviation < _ultraPreciseDeviation ? UltraPrecise : VeryPrecise;
        }
    }
}
