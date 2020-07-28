using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace cTeleport.AirportMeasure.Api.Tests
{
    public class AirportsControllerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        private WebApplicationFactory<Startup> CreateApplication()
        {
            
        }
    }
}