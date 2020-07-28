using System;
using cTeleport.AirportMeasure.Core.Errors;

namespace cTeleport.AirportMeasure.Core.Extensions
{
    public static class EnumExtensions
    {
        public static Error AsError(this Enum errorCode)
        {
            return new Error(errorCode);
        }
    }
}