using cTeleport.AirportMeasure.Core.Enums;
using cTeleport.AirportMeasure.Core.Results;
using Microsoft.AspNetCore.Mvc;

namespace cTeleport.AirportMeasure.Api.Extensions
{
    public static class ResultExtensions
    {
        public static ObjectResult ToObjectResult(this Result result)
        {
            if (!result.IsSuccess)
            {
                switch (result.ErrorCode)
                {
                    case (int) SystemErrorCodes.NotFound:
                        return new NotFoundObjectResult(result);
                    default:
                        return new BadRequestObjectResult(result);
                }
            }
            
            return new OkObjectResult(result);
        }

        public static ObjectResult ToObjectResult<TData>(this Result<TData> result)
        {
            if (!result.IsSuccess)
            {
                switch (result.ErrorCode)
                {
                    case (int) SystemErrorCodes.NotFound:
                        return new NotFoundObjectResult(result);
                    default:
                        return new BadRequestObjectResult(result);
                }
            }
            
            return new OkObjectResult(result.Data);
        }
    }
}