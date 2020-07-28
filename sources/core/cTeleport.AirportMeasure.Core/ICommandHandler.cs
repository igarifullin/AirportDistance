using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Results;

namespace cTeleport.AirportMeasure.Core
{
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task<Result> ExecuteAsync(TCommand command);
    }

    public interface ICommandHandler<in TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        Task<Result<TResult>> ExecuteAsync(TCommand command);
    }
}