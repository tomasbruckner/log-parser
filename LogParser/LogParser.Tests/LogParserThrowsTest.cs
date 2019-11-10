using LogParser.Exceptions;
using Xunit;

namespace LogParser.Tests
{
    public class LogParserThrowsTest
    {
        [Fact]
        public void InvalidSensorValueType()
        {
            Assert.Throws<InvalidSensorValueException>(
                () => LogParser.Parse(
                    @"reference monoxide 6
monoxide mon-1
2007-04-05T22:04 5.6
2007-04-05T22:05 7
2007-04-05T22:06 9"
                )
            );
        }

        [Fact]
        public void ThrowEmptyLog()
        {
            Assert.Throws<InvalidReferenceFormatException>(
                () => LogParser.Parse("")
            );
        }

        [Fact]
        public void ThrowInvalidSensorType()
        {
            Assert.Throws<InvalidSensorTypeException>(
                () => LogParser.Parse(
                    @"reference unknown 45.0
unknown unk-1
2007-04-05T22:02 79.1
2007-04-05T22:03 75.6
2007-04-05T22:04 71.2
2007-04-05T22:05 71.4
2007-04-05T22:06 69.2
2007-04-05T22:07 65.2
2007-04-05T22:08 62.8
2007-04-05T22:09 61.4
2007-04-05T22:10 64.0
2007-04-05T22:11 67.5
2007-04-05T22:12 69.4"
                )
            );
        }

        [Fact]
        public void ThrowMissingReference()
        {
            Assert.Throws<MissingReferenceValueException>(
                () => LogParser.Parse(
                    @"reference humidity 45.0
thermometer temp-2
2007-04-05T22:02 79.1
2007-04-05T22:03 75.6
2007-04-05T22:04 71.2
2007-04-05T22:05 71.4
2007-04-05T22:06 69.2
2007-04-05T22:07 65.2
2007-04-05T22:08 62.8
2007-04-05T22:09 61.4
2007-04-05T22:10 64.0
2007-04-05T22:11 67.5
2007-04-05T22:12 69.4"
                )
            );
        }

        [Fact]
        public void ThrowMissingReferenceKeyword()
        {
            Assert.Throws<InvalidReferenceFormatException>(
                () => LogParser.Parse(
                    @"thermometer 70.0
thermometer temp-2
2007-04-05T22:02 79.1
2007-04-05T22:03 75.6
2007-04-05T22:04 71.2
2007-04-05T22:05 71.4
2007-04-05T22:06 69.2
2007-04-05T22:07 65.2
2007-04-05T22:08 62.8
2007-04-05T22:09 61.4
2007-04-05T22:10 64.0
2007-04-05T22:11 67.5
2007-04-05T22:12 69.4"
                )
            );
        }

        [Fact]
        public void ThrowMissingReferenceSensorType()
        {
            Assert.Throws<InvalidReferenceFormatException>(
                () => LogParser.Parse(
                    @"reference humidity 45.0 thermometer
thermometer temp-2
2007-04-05T22:02 79.1
2007-04-05T22:03 75.6
2007-04-05T22:04 71.2
2007-04-05T22:05 71.4
2007-04-05T22:06 69.2
2007-04-05T22:07 65.2
2007-04-05T22:08 62.8
2007-04-05T22:09 61.4
2007-04-05T22:10 64.0
2007-04-05T22:11 67.5
2007-04-05T22:12 69.4"
                )
            );
        }

        [Fact]
        public void ThrowMissingSensorDefinition()
        {
            Assert.Throws<InvalidReferenceFormatException>(
                () => LogParser.Parse(
                    @"reference
2007-04-05T22:02 79.1
2007-04-05T22:03 75.6
2007-04-05T22:04 71.2
2007-04-05T22:05 71.4
2007-04-05T22:06 69.2
2007-04-05T22:07 65.2
2007-04-05T22:08 62.8
2007-04-05T22:09 61.4
2007-04-05T22:10 64.0
2007-04-05T22:11 67.5
2007-04-05T22:12 69.4"
                )
            );
        }

        [Fact]
        public void ThrowMissingValues()
        {
            Assert.Throws<NoReadingValuesException>(
                () => LogParser.Parse(
                    @"reference thermometer 70.0
thermometer temp-1
thermometer temp-2
2007-04-05T22:02 79.1
2007-04-05T22:03 75.6
2007-04-05T22:04 71.2
2007-04-05T22:05 71.4
2007-04-05T22:06 69.2
2007-04-05T22:07 65.2
2007-04-05T22:08 62.8
2007-04-05T22:09 61.4
2007-04-05T22:10 64.0
2007-04-05T22:11 67.5
2007-04-05T22:12 69.4"
                )
            );
        }

        [Fact]
        public void ThrowNoReferenceValues()
        {
            Assert.Throws<InvalidReferenceFormatException>(
                () => LogParser.Parse(
                    @"reference
unknown unk-1
2007-04-05T22:02 79.1
2007-04-05T22:03 75.6
2007-04-05T22:04 71.2
2007-04-05T22:05 71.4
2007-04-05T22:06 69.2
2007-04-05T22:07 65.2
2007-04-05T22:08 62.8
2007-04-05T22:09 61.4
2007-04-05T22:10 64.0
2007-04-05T22:11 67.5
2007-04-05T22:12 69.4"
                )
            );
        }

        [Fact]
        public void ThrowReferanceValueWithoutSensorType()
        {
            Assert.Throws<InvalidReferenceFormatException>(
                () => LogParser.Parse(
                    @"reference humidity 45.0 25
thermometer temp-2
2007-04-05T22:02 79.1
2007-04-05T22:03 75.6
2007-04-05T22:04 71.2
2007-04-05T22:05 71.4
2007-04-05T22:06 69.2
2007-04-05T22:07 65.2
2007-04-05T22:08 62.8
2007-04-05T22:09 61.4
2007-04-05T22:10 64.0
2007-04-05T22:11 67.5
2007-04-05T22:12 69.4"
                )
            );
        }
    }
}
