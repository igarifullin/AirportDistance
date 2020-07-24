using System.Threading;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Core.Results;
using MediatR;
using ICommand = cTeleport.AirportMeasure.Core.ICommand;

namespace cTeleport.AirportMeasure.Utils
{
    public static class MediatorExtensions
    {
        public static Task<Result<TResponse>> ExecuteAsync<TResponse, TCommand>(this IMediator mediator, TCommand command) where TCommand : ICommand<TResponse>
        {
            return mediator.Send(command, CancellationToken.None);
        }

        public static Task<Result> ExecuteAsync<TCommand>(this IMediator mediator, TCommand command) where TCommand : ICommand
        {
            return mediator.Send(command, CancellationToken.None);
        }

        public static Task<Result<TResponse>> QueryAsync<TResponse, TQuery>(this IMediator mediator, TQuery query)
            where TQuery : IQuery<TResponse>
        {
            return mediator.Send(query, CancellationToken.None);
        }

        public static async Task<Result> ExecuteAsync<TRequest>(this IMediator mediator, params TRequest[] requests)
            where TRequest : IRequest<Result>
        {
            foreach (var request in requests)
            {
                var result = await mediator.Send(request);
                if (!result.IsSuccess)
                {
                    return result;
                }
            }

            return Result.Success;
        }
    }
}