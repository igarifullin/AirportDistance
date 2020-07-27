using System.Threading.Tasks;
using cTeleport.AirportMeasure.Api.Extensions;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Services.Pipelines;
using Microsoft.AspNetCore.Mvc;

namespace cTeleport.AirportMeasure.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirportsController : ControllerBase
    {
        private readonly ICustomMediator _mediator;

        public AirportsController(ICustomMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("distance")]
        public async Task<IActionResult> Distance(string from, string to)
        {
            var result = await _mediator.ExecuteAsync(Scenarios.CalculateDistanceBetweenAirports(from, to));

            return result.ToObjectResult();
        }
    }
}