using System;
using System.Linq;
using System.Reflection;
using cTeleport.AirportMeasure.Core.Pipelines;
using Microsoft.Extensions.DependencyInjection;

namespace cTeleport.AirportMeasure.Core
{
    public static class Registry
    {
        public static IServiceCollection AddMediator(this IServiceCollection services) =>
            services.AddScoped<IMediator, Mediator>();

        public static IServiceCollection AddRequests(this IServiceCollection services)
        {
            return AddMediator(services)
                .Scan(s =>
                {
                    s.FromAssemblies(typeof(IMediator).Assembly)
                        .AddClasses(classes => classes.Where(c =>
                        {
                            var allInterfaces = c.GetInterfaces();
                            return
                                Enumerable.Any<Type>(allInterfaces, y =>
                                    IntrospectionExtensions.GetTypeInfo(y).IsGenericType &&
                                    IntrospectionExtensions.GetTypeInfo(y).GetGenericTypeDefinition() ==
                                    typeof(IPipelineItemHandler<>)) ||
                                Enumerable.Any<Type>(allInterfaces, y =>
                                    IntrospectionExtensions.GetTypeInfo(y).IsGenericType &&
                                    IntrospectionExtensions.GetTypeInfo(y).GetGenericTypeDefinition() ==
                                    typeof(IPipelineItemHandler<,>));
                        }))
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();
                })
                .AddTransient(typeof(CacheableQueryHandler<,>));
        }
        
        public static IServiceCollection AddAllCommands(this IServiceCollection services, Assembly assembly)
            =>
                services
                    .Scan(s =>
                    {
                        s.FromAssemblies(assembly)
                            .AddClasses(classes => classes.Where(c =>
                            {
                                var allInterfaces = c.GetInterfaces();
                                return
                                    Enumerable.Any<Type>(allInterfaces, y =>
                                        IntrospectionExtensions.GetTypeInfo(y).IsGenericType && IntrospectionExtensions.GetTypeInfo(y).GetGenericTypeDefinition() ==
                                        typeof(ICommandHandler<>)) ||
                                    Enumerable.Any<Type>(allInterfaces, y =>
                                        IntrospectionExtensions.GetTypeInfo(y).IsGenericType && IntrospectionExtensions.GetTypeInfo(y).GetGenericTypeDefinition() ==
                                        typeof(ICommandHandler<,>));
                            }))
                            .AsSelf()
                            .AsImplementedInterfaces()
                            .WithTransientLifetime();
                    });
        
        public static IServiceCollection AddAllValidations(this IServiceCollection services, Assembly assembly)
            => services
                .Scan(s =>
                {
                    s.FromAssemblies(assembly)
                        .AddClasses(classes => classes.Where(c =>
                        {
                            var allInterfaces = c.GetInterfaces();
                            return
                                Enumerable.Any<Type>(allInterfaces, y =>
                                    IntrospectionExtensions.GetTypeInfo(y).IsGenericType && IntrospectionExtensions.GetTypeInfo(y).GetGenericTypeDefinition() ==
                                    typeof(IValidationRuleHandler<>));
                        }))
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();
                });

        public static IServiceCollection AddAllQueries(this IServiceCollection services, Assembly assembly)
            =>
                services.Scan(s =>
                {
                    s.FromAssemblies(assembly)
                        .AddClasses(classes => classes.Where(c =>
                        {
                            var allInterfaces = c.GetInterfaces();
                            return
                                Enumerable.Any<Type>(allInterfaces, y =>
                                    IntrospectionExtensions.GetTypeInfo(y).IsGenericType && IntrospectionExtensions.GetTypeInfo(y).GetGenericTypeDefinition() ==
                                    typeof(IQueryHandler<,>));
                        }))
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .WithTransientLifetime();
                });
    }
}