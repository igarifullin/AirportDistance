using cTeleport.AirportMeasure.Core.Pipelines;
using cTeleport.AirportMeasure.Core.Results;
using MediatR;

namespace cTeleport.AirportMeasure.Core
{
    public interface IValidationRule : IRequest<ValidationResult>, IPipeline<ValidationResult>
    {
        
    }
}