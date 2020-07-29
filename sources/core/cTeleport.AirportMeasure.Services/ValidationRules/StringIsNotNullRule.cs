using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Enums;

namespace cTeleport.AirportMeasure.Services.ValidationRules
{
    public class StringIsNotNullRule : IValidationRule
    {
        public string Value { get; }

        public int ErrorCode { get; }

        public string ErrorMessage { get; }

        public StringIsNotNullRule(string value) : this(value, (int)SystemErrorCodes.InvalidRequest, "Parameter is empty")
        {
        }

        public StringIsNotNullRule(string value, string errorMessage) : this(value, (int)SystemErrorCodes.InvalidRequest, errorMessage)
        {
        }

        public StringIsNotNullRule(string value, int errorCode, string errorMessage)
        {
            Value = value;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
        }
    }
}