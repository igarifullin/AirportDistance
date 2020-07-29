using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data;
using cTeleport.AirportMeasure.Services.Integration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace cTeleport.AirportMeasure.Api.IntegrationTests
{
    public class TestWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public Mock<IAirportInformationProvider> AirportInformationProviderMock { get; }

        public TestWebApplicationFactory()
        {
            AirportInformationProviderMock = new Mock<IAirportInformationProvider>(MockBehavior.Strict);
            AirportInformationProviderMock
                .Setup(x => x.GetAirportAsync(It.IsAny<string>()))
                .Returns(Result.SuccessData(new AirportDto()));
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddTransient<IAirportInformationProvider>(c => AirportInformationProviderMock.Object);
            });
        }
    }
}
