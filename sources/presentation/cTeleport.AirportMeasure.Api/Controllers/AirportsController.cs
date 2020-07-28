using System.Threading.Tasks;
using cTeleport.AirportMeasure.Api.Extensions;
using cTeleport.AirportMeasure.Api.Models;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Services.Pipelines;
using Microsoft.AspNetCore.Mvc;

namespace cTeleport.AirportMeasure.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AirportsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AirportsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{iata}")]
        public async Task<IActionResult> Get(string iata)
        {
            var result = await _mediator.ExecuteAsync(Scenarios.GetAirportInformation(iata));
         
            return result.ToObjectResult();
        }

        [HttpGet]
        [Route("distance")]
        public async Task<IActionResult> Distance(string from, string to)
        {
            var result = await _mediator.ExecuteAsync(Scenarios.CalculateDistanceBetweenAirports(from, to));
            if (result.IsSuccess)
            {
                return DistanceModel.FromResult(result.Data).ToObjectResult();
            }
            return result.ToObjectResult();
        }
    }
}