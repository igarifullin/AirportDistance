using cTeleport.AirportMeasure.Core.Results;
using MediatR;

namespace cTeleport.AirportMeasure.Core
{
    public interface IValidationRuleHandler<T> : IRequestHandler<T, ValidationResult> where T : IValidationRule
    {
    }
}