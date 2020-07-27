using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using cTeleport.AirportMeasure.Core.Enums;
using cTeleport.AirportMeasure.Core.Extensions;
using cTeleport.AirportMeasure.Core.Pipelines;
using cTeleport.AirportMeasure.Core.Results;
using cTeleport.AirportMeasure.Utils;
using Microsoft.Extensions.Logging;

namespace cTeleport.AirportMeasure.Core
{
    public interface ICustomMediator
    {
        Task<Result<TData>> ExecuteAsync<TData>(IPipeline<TData> pipeline);

        Task<Result> ExecuteAsync(IPipeline pipeline);
    }

    public class CustomMediator : ICustomMediator
    {
        private readonly ILogger<CustomMediator> _logger;
        private readonly IServiceProvider _serviceProvider;

        public CustomMediator(IServiceProvider serviceProvider,
            ILogger<CustomMediator> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public virtual async Task<Result<TData>> ExecuteAsync<TData>(IPipeline<TData> pipeline)
        {
            Task<Result<TData>> resultTask;
            switch (pipeline)
            {
                case ICommand<TData> command:
                    resultTask = ExecuteCommandAsync<TData>(command);
                    break;

                case IQuery<TData> query:
                    resultTask = ExecuteQueryAsync<TData>(query);
                    break;

                case IValidationRule validationRule:
                    resultTask = ValidateAsync<TData>(validationRule);
                    break;

                default:
                    resultTask = ExecutePipelineAsync(pipeline);
                    break;
            }

            return await resultTask;
        }

        public virtual async Task<Result> ExecuteAsync(IPipeline pipeline)
        {
            var pipelineType = typeof(IPipeline<>);
            var genericPipelineType = pipeline.GetType().GetInterfaces()
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == pipelineType);
            if (genericPipelineType != null)
            {
                var dataType = genericPipelineType.GetGenericArguments().First();
                var methodInfo = GetType()
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                    .Where(m => m.Name == nameof(ICustomMediator.ExecuteAsync))
                    .Single(m => m.IsGenericMethod)
                    .MakeGenericMethod(dataType);

                var convertMethodInfo = GetType()
                    .GetMethod(nameof(Convert),
                        BindingFlags.Static | BindingFlags.NonPublic);

                Check.NotNull(convertMethodInfo, nameof(Convert));

                var convertGenericMethodInfo = convertMethodInfo.MakeGenericMethod(dataType);

                var taskResult = methodInfo.Invoke(this, new object[] { pipeline });
                return await (Task<Result>) convertGenericMethodInfo.Invoke(null, new[] {taskResult});
            }

            Task<Result> resultTask;
            switch (pipeline)
            {
                case ICommand command:
                    resultTask = ExecuteCommandAsync(command);
                    break;

                default:
                    resultTask = ExecutePipelineAsync(pipeline);
                    break;
            }

            return await resultTask;
        }
        
        private Task<Result<TData>> ExecuteCommandAsync<TData>(IPipeline command)
        {
            var commandHandlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TData));
            var methodInfo = commandHandlerType.GetMethod(nameof(ICommandHandler<ICommand<TData>, TData>.Handle),
                BindingFlags.Public | BindingFlags.Instance);

            Check.NotNull(methodInfo, nameof(ICommandHandler<ICommand<TData>, TData>.Handle));

            var commandHandler = _serviceProvider.GetService(commandHandlerType);
            var cancellationToken = _serviceProvider.GetService(typeof(CancellationToken));

            Check.NotNull(commandHandler, $"command handler not found in DI container by type {commandHandlerType.FullName}");

            return (Task<Result<TData>>)methodInfo.Invoke(commandHandler, new object[] { command, cancellationToken });
        }

        private async Task<Result> ExecuteCommandAsync(IPipeline command)
        {
            var commandType = command.GetType();
            var commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(commandType);
            var methodInfo = commandHandlerType.GetMethod(nameof(ICommandHandler<ICommand>.Handle),
                BindingFlags.Public | BindingFlags.Instance);

            Check.NotNull(methodInfo, nameof(ICommandHandler<ICommand>.Handle));

            var commandHandler = _serviceProvider.GetService(commandHandlerType);
            var cancellationToken = _serviceProvider.GetService(typeof(CancellationToken));
            
            Check.NotNull(commandHandler, $"command handler not found in DI container by type {commandHandlerType.FullName}");

            return await (Task<Result>)methodInfo.Invoke(commandHandler, new object[] { command, cancellationToken });
        }

        private async Task<Result<TData>> ValidateAsync<TData>(IValidationRule validationRule)
        {
            var validationRuleType = validationRule.GetType();
            var validationRuleHandlerType = typeof(IValidationRuleHandler<>).MakeGenericType(validationRuleType);
            var methodInfo = validationRuleHandlerType.GetMethod(nameof(IValidationRuleHandler<IValidationRule>.Handle),
                BindingFlags.Public | BindingFlags.Instance);

            Check.NotNull(methodInfo, nameof(IValidationRuleHandler<IValidationRule>.Handle));

            var validationRuleHandler = _serviceProvider.GetService(validationRuleHandlerType);
            var cancellationToken = _serviceProvider.GetService(typeof(CancellationToken));
            
            Check.NotNull(validationRuleHandler, $"handler not found in DI container by type {validationRuleHandlerType.FullName}");

            var result = await (Task<ValidationResult>)methodInfo.Invoke(validationRuleHandler, new object[] {validationRule, cancellationToken});

            return result as Result<TData>;
        }

        private Task<Result<TData>> ExecuteQueryAsync<TData>(IPipeline query)
        {
            if (query is ICacheableQuery<TData> cacheableQuery)
            {
                return ExecuteCacheableQueryAsync(cacheableQuery);
            }

            var queryHandlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TData));
            var methodInfo = queryHandlerType.GetMethod(nameof(IQueryHandler<IQuery<TData>, TData>.Handle),
                BindingFlags.Public | BindingFlags.Instance);

            Check.NotNull(methodInfo, nameof(IQueryHandler<IQuery<TData>, TData>.Handle));

            var queryHandler = _serviceProvider.GetService(queryHandlerType);
            var cancellationToken = _serviceProvider.GetService(typeof(CancellationToken));

            if (queryHandler == null)
            {
                _logger.LogError($"Handler not found for type {queryHandlerType.FullName}");
                return Task.FromResult(new Result<TData>(SystemErrorCodes.SystemError.AsError()));
            }

            return (Task<Result<TData>>)methodInfo.Invoke(queryHandler, new object[] { query, cancellationToken });
        }

        private Task<Result<TData>> ExecuteCacheableQueryAsync<TData>(ICacheableQuery<TData> query)
        {
            var queryHandlerType = typeof(CacheableQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TData));
            var queryHandler = _serviceProvider.GetService(queryHandlerType);

            if (queryHandler == null)
            {
                _logger.LogError($"Handler not found for type {queryHandlerType.FullName}");
                return Task.FromResult(new Result<TData>(SystemErrorCodes.SystemError.AsError()));
            }
            var cancellationToken = _serviceProvider.GetService(typeof(CancellationToken));

            var methodInfo = queryHandlerType.GetMethod(nameof(CacheableQueryHandler<ICacheableQuery<TData>, TData>.ExecuteAsync),
                BindingFlags.Public | BindingFlags.Instance);

            Check.NotNull(methodInfo, nameof(CacheableQueryHandler<ICacheableQuery<TData>, TData>.ExecuteAsync));

            return (Task<Result<TData>>)methodInfo.Invoke(queryHandler, new object[] { query, cancellationToken });
        }

        private Task<Result<TData>> ExecutePipelineAsync<TData>(IPipeline<TData> pipeline)
        {
            var pipelineItemHandlerType = typeof(IPipelineItemHandler<,>).MakeGenericType(pipeline.GetType(), typeof(TData));
            var methodInfo = pipelineItemHandlerType.GetMethod(nameof(IPipelineItemHandler<IPipeline<TData>, TData>.ExecuteAsync),
                BindingFlags.Public | BindingFlags.Instance);

            Check.NotNull(methodInfo, nameof(IPipelineItemHandler<IPipeline<TData>, TData>.ExecuteAsync));

            var pipelineItemHandler = _serviceProvider.GetService(pipelineItemHandlerType);

            Check.NotNull(pipelineItemHandler, $"handler not found in DI container by type {pipelineItemHandlerType.FullName}");

            return (Task<Result<TData>>)methodInfo.Invoke(pipelineItemHandler, new object[] { pipeline });
        }

        private Task<Result> ExecutePipelineAsync(IPipeline pipeline)
        {
            var pipelineItemHandlerType = typeof(IPipelineItemHandler<>).MakeGenericType(pipeline.GetType());
            var methodInfo = pipelineItemHandlerType.GetMethod(nameof(IPipelineItemHandler<IPipeline>.ExecuteAsync),
                BindingFlags.Public | BindingFlags.Instance);

            Check.NotNull(methodInfo, nameof(IPipelineItemHandler<IPipeline>.ExecuteAsync));

            var pipelineItemHandler = _serviceProvider.GetService(pipelineItemHandlerType);

            Check.NotNull(pipelineItemHandler, $"handler not found in DI container by type {pipelineItemHandlerType.FullName}");

            return (Task<Result>)methodInfo.Invoke(pipelineItemHandler, new object[] { pipeline });
        }
    }
}