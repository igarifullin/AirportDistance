using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Api.Models;
using cTeleport.AirportMeasure.Core.Enums;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Moq;

namespace cTeleport.AirportMeasure.Api.Tests
{
    public class AirportsControllerTests
    {
        private TestWebApplicationFactory _factory;

        [SetUp]
        public void Setup()
        {
            _factory = new TestWebApplicationFactory();
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        [Test]
        public async Task Get_EmptyIata_ShouldReturnNotFound()
        {
            // arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // act
            var httpResponse = await client.GetAsync("/airports//");

            // assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [TestCase("_")]
        [TestCase("404")]
        [TestCase("AT")]
        [TestCase("ATAT")]
        public async Task Get_InvalidIata_ShouldReturnError(string iata)
        {
            // arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // act
            var httpResponse = await client.GetAsync($"/airports/{iata}/");
            var error = await GetResponseAsync<ErrorModel>(httpResponse);

            // assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error, Is.Not.Null);
            Assert.That(error.ErrorCode, Is.EqualTo((int)SystemErrorCodes.InvalidRequest));

            // verify
            _factory.AirportInformationProviderMock.VerifyNoOtherCalls();
        }

        [TestCase("LED")]
        [TestCase("AMS")]
        public async Task Get_ValidIata_ShouldCallProvider(string iata)
        {
            // arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _factory.AirportInformationProviderMock
                .Setup(x => x.GetAirportAsync(iata))
                .ReturnsAsync(Result.SuccessData(new AirportDto
                {
                    Iata = iata
                }));

            // act
            var httpResponse = await client.GetAsync($"/airports/{iata}/");
            var airport = await GetResponseAsync<AirportModel>(httpResponse);

            // assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(airport, Is.Not.Null);
            Assert.That(airport.Iata, Is.EqualTo(iata));

            // verify
            _factory.AirportInformationProviderMock.Verify(x => x.GetAirportAsync(iata), Times.Once);
            _factory.AirportInformationProviderMock.VerifyNoOtherCalls();
        }

        [TestCase("AMS")]
        public async Task Get_DoubleCallSameIata_ShouldCallProviderOnceAndSecondFromCache(string iata)
        {
            // arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _factory.AirportInformationProviderMock
                .Setup(x => x.GetAirportAsync(iata))
                .ReturnsAsync(Result.SuccessData(new AirportDto
                {
                    Iata = iata
                }));

            // act
            await client.GetAsync($"/airports/{iata}/");
            var httpResponse = await client.GetAsync($"/airports/{iata}/");
            var airport = await GetResponseAsync<AirportModel>(httpResponse);

            // assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(airport, Is.Not.Null);
            Assert.That(airport.Iata, Is.EqualTo(iata));

            // verify
            _factory.AirportInformationProviderMock.Verify(x => x.GetAirportAsync(iata), Times.Once);
            _factory.AirportInformationProviderMock.VerifyNoOtherCalls();
        }

        [TestCase("LED","_")]
        [TestCase("LED","404")]
        [TestCase("LED","AT")]
        [TestCase("LED","INVALIDATA")]
        [TestCase("_","LED")]
        [TestCase("404","LED")]
        [TestCase("AT","LED")]
        [TestCase("INVALIDATA","LED")]
        public async Task Distance_InvalidIata_ShouldReturnError(string from, string to)
        {
            // arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // act
            var httpResponse = await client.GetAsync($"/airports/distance?from={from}&to={to}");
            var error = await GetResponseAsync<ErrorModel>(httpResponse);

            // assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(error, Is.Not.Null);
            Assert.That(error.ErrorCode, Is.EqualTo((int)SystemErrorCodes.InvalidRequest));

            // verify
            _factory.AirportInformationProviderMock.VerifyNoOtherCalls();
        }

        [TestCase("LED","AMS")]
        public async Task Distance_ValidData_ShouldGetDataFromProvider(string from, string to)
        {
            // arrange
            const double expectedDistance = 1102.2148091856811;
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            _factory.AirportInformationProviderMock
                .Setup(x => x.GetAirportAsync(from))
                .ReturnsAsync(Result.SuccessData(new AirportDto
                {
                    Iata = from,
                    Location = new LocationDto
                    {
                        Longitude = 30.270505,
                        Latitude = 59.799847
                    }
                }));
            _factory.AirportInformationProviderMock
                .Setup(x => x.GetAirportAsync(to))
                .ReturnsAsync(Result.SuccessData(new AirportDto
                {
                    Iata = to,
                    Location = new LocationDto
                    {
                        Longitude = 4.763385,
                        Latitude = 52.309069
                    }
                }));

            // act
            var httpResponse = await client.GetAsync($"/airports/distance?from={from}&to={to}");
            var distance = await GetResponseAsync<DistanceModel>(httpResponse);

            // assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(distance, Is.Not.Null);
            Assert.That(distance.From.Iata, Is.EqualTo(from));
            Assert.That(distance.To.Iata, Is.EqualTo(to));
            Assert.That(distance.Distance, Is.EqualTo(expectedDistance));

            // verify
            _factory.AirportInformationProviderMock.Verify(x => x.GetAirportAsync(from), Times.Once);
            _factory.AirportInformationProviderMock.Verify(x => x.GetAirportAsync(to), Times.Once);
            _factory.AirportInformationProviderMock.VerifyNoOtherCalls();
        }

        private async Task<T> GetResponseAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            try
            {
                var content = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
            catch(Exception)
            {
                return default;
            }
        }
    }
}