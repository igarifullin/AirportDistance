using System.Collections.Generic;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Data;
using cTeleport.AirportMeasure.Services.Commands;
using NUnit.Framework;

namespace cTeleport.AirportMeasure.Services.Tests.Commands
{
    [TestFixture]
    public class CalculateDistanceBetweenLocationsCommandHandlerTests
    {
        private CalculateDistanceBetweenLocationsCommandHandler _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new CalculateDistanceBetweenLocationsCommandHandler();
        }

        private static IEnumerable<TestCaseData> Cases()
        {
            yield return new TestCaseData(
                // AMS
                new LocationDto
                {
                    Latitude = 52.3105,
                    Longitude = 4.7683
                },
                // LED
                new LocationDto
                {
                    Latitude = 59.8029,
                    Longitude = 30.2678
                },
                1101.9608252837247);
        }

        [TestCaseSource(nameof(Cases))]
        public async Task ExecuteAsync_CalculateDistance(LocationDto from, LocationDto to, double expectedDistance)
        {
            // arrange
            var command = new CalculateDistanceBetweenLocationsCommand(from, to);

            // act
            var result= await _sut.ExecuteAsync(command);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Data, Is.EqualTo(expectedDistance));
        }
    }
}