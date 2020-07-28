using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Api.Models
{
    public class ErrorModel
    {
        public string ErrorMessage { get; set; }

        public int ErrorCode { get; set; }

        public static ErrorModel FromResult(Result result)
        {
            return new ErrorModel
            {
                ErrorMessage = result.ErrorMessage,
                ErrorCode = result.ErrorCode
            };
        }
    }
}