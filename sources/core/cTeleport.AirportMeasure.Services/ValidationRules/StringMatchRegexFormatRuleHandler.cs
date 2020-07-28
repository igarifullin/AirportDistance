using System.Text.RegularExpressions;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Enums;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Services.ValidationRules
{
    public class StringMatchRegexFormatRuleHandler : IValidationRuleHandler<StringMatchRegexFormatRule>
    {
        public Task<ValidationResult> ExecuteAsync(StringMatchRegexFormatRule rule)
        {
            var result = new ValidationResult();

            if (!Regex.IsMatch(rule.Value, rule.Regex))
            {
                result.WithError((int) SystemErrorCodes.InvalidRequest, "Parameter has invalid format");
            }

            return result;
        }
    }
}