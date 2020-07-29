using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Pipelines;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data;
using cTeleport.AirportMeasure.Services.Commands;
using Moq;
using NUnit.Framework;

namespace cTeleport.AirportMeasure.Services.Tests.Commands
{
    [TestFixture]
    public class CalculateDistanceBetweenAirportsCommandHandlerTests
    {
        private Mock<IMediator> _mediatorMock;
        private CalculateDistanceBetweenAirportsCommandHandler _sut;

        [SetUp]
        public void Setup()
        {
            _mediatorMock = new Mock<IMediator>(MockBehavior.Strict);
            _mediatorMock.Setup(x =>
                    x.ExecuteAsync(It.IsAny<IPipeline<double>>()))
                .ReturnsAsync(Result.SuccessData(100.00));

            _sut = new CalculateDistanceBetweenAirportsCommandHandler(_mediatorMock.Object);
        }

        [Test]
        public async Task ExecuteAsync_ShouldCallCalculateDistanceCommand()
        {
            // arrange
            var from = new AirportDto {Iata = "LED", Location = new LocationDto()};
            var to = new AirportDto {Iata = "AMS", Location = new LocationDto()};
            var command = new CalculateDistanceBetweenAirportsCommand(from, to);

            // act
            var result = await _sut.ExecuteAsync(command);

            // assert
            Assert.That(result, Is.Not.Null);

            // verify
            _mediatorMock.Verify(
                x => x.ExecuteAsync(It.Is<IPipeline<double>>(r => r is CalculateDistanceBetweenLocationsCommand)),
                Times.Once);
        }
    }
}