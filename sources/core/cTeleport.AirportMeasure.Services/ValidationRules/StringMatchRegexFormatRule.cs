using cTeleport.AirportMeasure.Core;

namespace cTeleport.AirportMeasure.Services.ValidationRules
{
    public class StringMatchRegexFormatRule : IValidationRule
    {
        public string Value { get; set; }

        public string Regex { get; set; }
    }
}