using System.Text.RegularExpressions;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Enums;

namespace cTeleport.AirportMeasure.Services.ValidationRules
{
    public class StringMatchRegexFormatRule : IValidationRule
    {
        public string Value { get; }

        public string Regex { get; }

        public int ErrorCode { get; }

        public string ErrorMessage { get; }

        public StringMatchRegexFormatRule(string value, Regex regex) : this(value, regex, (int) SystemErrorCodes.InvalidRequest, "Parameter has invalid format")
        {
        }

        public StringMatchRegexFormatRule(string value, Regex regex, string errorMessage) : this(value, regex, (int) SystemErrorCodes.InvalidRequest, errorMessage)
        {
        }

        public StringMatchRegexFormatRule(string value, Regex regex, int errorCode, string errorMessage)
        {
            Value = value;
            Regex = regex.ToString();
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}