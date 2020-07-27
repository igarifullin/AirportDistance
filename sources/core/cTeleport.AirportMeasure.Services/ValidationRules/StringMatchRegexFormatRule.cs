using System.Text.RegularExpressions;
using cTeleport.AirportMeasure.Core;

namespace cTeleport.AirportMeasure.Services.ValidationRules
{
    public class StringMatchRegexFormatRule : IValidationRule
    {
        public string Value { get; set; }

        public string Regex { get; set; }

        public StringMatchRegexFormatRule(string value, string regex)
        {
            Value = value;
            Regex = regex;
        }

        public StringMatchRegexFormatRule(string value, Regex regex)
        {
            Value = value;
            Regex = regex.ToString();
        }
    }
}