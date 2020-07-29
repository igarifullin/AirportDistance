using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Enums;
using cTeleport.AirportMeasure.Data;
using cTeleport.AirportMeasure.Services.Integration.Impl;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;

namespace cTeleport.AirportMeasure.Services.Tests.Integration
{
    [TestFixture]
    public class AirportInformationProviderTests
    {
        private AirportInformationProvider _sut;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())  
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new AirportDto()))  
                });
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            httpClient.BaseAddress = new Uri("http://localhost/");
            _sut = new AirportInformationProvider(httpClient);
        }

        [Test]
        public async Task GetAirportAsync_GetInternalServerError_ShouldReturnError()
        {
            // arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())  
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // act
            var result = await _sut.GetAirportAsync("LED");

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
            Assert.That(result.Errors.First().Code, Is.EqualTo((int) SystemErrorCodes.SystemError));
        }

        [Test]
        public async Task GetAirportAsync_GetEmptyContent_ShouldReturnError()
        {
            // arrange
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())  
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            // act
            var result = await _sut.GetAirportAsync("LED");

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Errors, Is.Not.Empty);
            Assert.That(result.Errors.First().Code, Is.EqualTo((int) SystemErrorCodes.InvalidRequest));
        }

        [Test]
        public async Task GetAirportAsync_Success_ShouldReturnAirportModel()
        {
            // arrange
            const string iata = "LED";
            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new AirportDto
                    {
                        Iata = iata
                    }))
                });

            // act
            var result = await _sut.GetAirportAsync(iata);

            // assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Errors, Is.Empty);
            Assert.That(result.Data, Is.Not.Null);
            Assert.That(result.Data.Iata, Is.EqualTo(iata));
        }
    }
}