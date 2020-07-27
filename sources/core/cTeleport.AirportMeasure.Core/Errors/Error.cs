using System;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Core.Errors
{
    public class Error
    {
        public int Code { get; }

        public string Message { get; }

        public Error(Enum code) : this(Convert.ToInt32(code))
        {
        }
        
        public Error(int code) : this(code, code.ToString())
        {
        }

        public Error(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public static implicit operator Task<Result>(Error error) => Task.FromResult(Result.Error(error));
    }
}