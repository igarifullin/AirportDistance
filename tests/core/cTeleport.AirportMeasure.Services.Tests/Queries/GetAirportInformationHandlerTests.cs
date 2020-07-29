using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data;
using cTeleport.AirportMeasure.Services.Integration;
using cTeleport.AirportMeasure.Services.Queries;
using Moq;
using NUnit.Framework;

namespace cTeleport.AirportMeasure.Services.Tests.Queries
{
    [TestFixture]
    public class GetAirportInformationHandlerTests
    {
        private Mock<IAirportInformationProvider> _airportInformationProviderMock;
        private GetAirportInformationHandler _sut;

        [SetUp]
        public void Setup()
        {
            _airportInformationProviderMock = new Mock<IAirportInformationProvider>(MockBehavior.Strict);
            _sut = new GetAirportInformationHandler(_airportInformationProviderMock.Object);
        }

        [Test]
        public async Task ExecuteAsync_CallProvider()
        {
            // arrange
            const string iata = "LED";
            _airportInformationProviderMock
                .Setup(x => x.GetAirportAsync(iata))
                .ReturnsAsync(Result.SuccessData(new AirportDto {Iata = iata}));
            var query = new GetAirportInformation(iata);

            // act
            var result = await _sut.ExecuteAsync(query);

            // assert
            Assert.That(result, Is.Not.Null);

            // verify
            _airportInformationProviderMock.Verify(x => x.GetAirportAsync(iata), Times.Once);
            _airportInformationProviderMock.VerifyNoOtherCalls();
        }
    }
}