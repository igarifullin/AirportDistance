using System.Threading;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Enums;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Services.ValidationRules
{
    public class StringIsNotNullRuleHandler : IValidationRuleHandler<StringIsNotNullRule>
    {
        public Task<ValidationResult> Handle(StringIsNotNullRule rule, CancellationToken cancellationToken)
        {
            var result = new ValidationResult();

            if (string.IsNullOrEmpty(rule.Value))
            {
                result.WithError((int) SystemErrorCodes.InvalidRequest, "Parameter is empty");
            }

            return result;
        }
    }
}