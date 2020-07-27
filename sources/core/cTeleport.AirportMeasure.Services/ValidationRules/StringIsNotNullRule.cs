using cTeleport.AirportMeasure.Core;

namespace cTeleport.AirportMeasure.Services.ValidationRules
{
    public class StringIsNotNullRule : IValidationRule
    {
        public string Value { get; set; }

        public StringIsNotNullRule(string value)
        {
            Value = value;
        }
    }
}