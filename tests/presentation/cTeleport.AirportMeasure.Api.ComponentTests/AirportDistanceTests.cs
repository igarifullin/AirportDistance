using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Api.ComponentTests.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace cTeleport.AirportMeasure.Api.ComponentTests
{
    [TestFixture]
    public class AirportDistanceTests
    {
        private HttpClient _httpClient;

        [SetUp]
        public void Setup()
        {
            _httpClient = ApiTestsSetupClass.HttpClient;
        }

        [TestCase("LED","AMS")]
        public async Task Distance_ValidData_ShouldGetDataFromProvider(string from, string to)
        {
            // arrange
            const double expectedDistance = 1102.2148091856811;

            // act
            var httpResponse = await _httpClient.GetAsync($"/airports/distance?from={from}&to={to}");
            var distance = await GetResponseAsync<DistanceModel>(httpResponse);

            // assert
            Assert.That(httpResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(distance, Is.Not.Null);
            Assert.That(distance.From.Iata, Is.EqualTo(from));
            Assert.That(distance.To.Iata, Is.EqualTo(to));
            Assert.That(distance.Distance, Is.EqualTo(expectedDistance));
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
