using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Core
{
    public interface IValidationRuleHandler<in TValidationRule> 
        where TValidationRule : IValidationRule
    {
        Task<ValidationResult> ExecuteAsync(TValidationRule rule);
    }
}