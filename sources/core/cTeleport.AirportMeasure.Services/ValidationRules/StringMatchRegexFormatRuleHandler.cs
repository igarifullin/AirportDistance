using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Data.Enums;

namespace cTeleport.AirportMeasure.Services.ValidationRules
{
    public class StringMatchRegexFormatRuleHandler : IValidationRuleHandler<StringMatchRegexFormatRule>
    {
        public Task<ValidationResult> Handle(StringMatchRegexFormatRule request, CancellationToken cancellationToken)
        {
            var result = new ValidationResult();

            if (!Regex.IsMatch(request.Value, request.Regex))
            {
                result.WithError((int) SystemErrorCodes.InvalidRequest, "Parameter has invalid format");
            }

            return result;
        }
    }
}