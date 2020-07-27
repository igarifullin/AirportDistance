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
            services.AddScoped<ICustomMediator, CustomMediator>();

        public static IServiceCollection AddRequests(this IServiceCollection services)
        {
            return AddMediator(services)
                .Scan(s =>
                {
                    s.FromAssemblies(typeof(ICustomMediator).Assembly)
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
    }
}