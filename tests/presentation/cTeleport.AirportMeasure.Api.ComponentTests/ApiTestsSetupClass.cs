using System;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Http;

namespace cTeleport.AirportMeasure.Api.ComponentTests
{
    [SetUpFixture]
    public class ApiTestsSetupClass
    {
        public static HttpClient HttpClient { get; private set; }

        [OneTimeSetUp]
        public async Task GlobalSetup()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            if (!Uri.TryCreate(configuration["apiUrl"], UriKind.Absolute, out var uri))
            {
                throw new ArgumentException("apiUrl should has URI format", "apiUrl");
            }
            HttpClient = new HttpClient
            {
                BaseAddress = uri
            };
        }
    }
}
