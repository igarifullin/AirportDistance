using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Errors;

namespace cTeleport.AirportMeasure.Core.Results
{
    public class Result
    {
        private readonly List<Error> _errors = new List<Error>();

        public IReadOnlyCollection<Error> Errors => _errors;

        public int ErrorCode => Errors.FirstOrDefault()?.Code ?? 0;

        public string ErrorMessage => Errors.FirstOrDefault()?.Message;

        public bool IsSuccess => !Errors.Any();

        public Result WithError(int errorCode) => WithError(errorCode, string.Empty);

        public Result WithError(int errorCode, string errorMessage) => WithError(new Error(errorCode, errorMessage));

        public Result WithError(params Error[] errors)
        {
            _errors.AddRange(errors);
            return this;
        }

        public ICollection<string> GetFormattedErrorsCollection()
        {
            return Errors.Select(err => $"Error code: {err.Code} message: {err.Message}").ToList();
        }

        public string GetFormattedErrors()
        {
            return string.Join(Environment.NewLine, GetFormattedErrorsCollection());
        }

        public override string ToString() =>
            $"IsSuccess: {IsSuccess}. {Environment.NewLine} Errors: {GetFormattedErrors()}";
        
        public static Result Success => new Result();

        public static Result Error(int errorCode) => new Result().WithError(errorCode);

        public static Result Error(int errorCode, string errorMessage) =>
            new Result().WithError(errorCode, errorMessage);

        public static Result<T> SuccessData<T>(T value) => new Result<T>(value);

        public static implicit operator Task<Result>(Result r) => Task.FromResult(r);
    }

    public class Result<TData> : Result
    {
        public Result()
        {
        }

        public Result(params Error[] errors) : this(default, errors)
        {
        }

        public Result(TData data, params Error[] errors)
        {
            Data = data;
        }

        public TData Data { get; }

        public static implicit operator Task<Result<TData>>(Result<TData> result) => Task.FromResult(result);

        public static implicit operator Result<TData>(TData data) => new Result<TData>(data);
        
        public static implicit operator Result<TData>(Error error) => new Result<TData>(error);

        public static implicit operator Result<TData>(Error[] errors) => new Result<TData>(errors);

        public static implicit operator Result<TData>(List<Error> errors) => new Result<TData>(errors.ToArray());
    }
}