using cTeleport.AirportMeasure.Api.Models;
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
                var error = ErrorModel.FromResult(result);
                switch (result.ErrorCode)
                {
                    case (int) SystemErrorCodes.NotFound:
                        return new NotFoundObjectResult(error);
                    default:
                        return new BadRequestObjectResult(error);
                }
            }
            
            return new OkObjectResult(result);
        }

        public static ObjectResult ToObjectResult<TData>(this Result<TData> result)
        {
            if (!result.IsSuccess)
            {
                var error = ErrorModel.FromResult(result);
                switch (result.ErrorCode)
                {
                    case (int) SystemErrorCodes.NotFound:
                        return new NotFoundObjectResult(error);
                    default:
                        return new BadRequestObjectResult(error);
                }
            }
            
            return new OkObjectResult(result.Data);
        }
    }
}