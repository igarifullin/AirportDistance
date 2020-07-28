using cTeleport.AirportMeasure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace cTeleport.AirportMeasure.DataAccess
{
    public static class Registry
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("redisCache");
            services.Configure<RedisConfiguration>(Microsoft.Extensions.Options.Options.DefaultName, section.Bind);
            return services.AddSingleton<ICacheableStorage, RedisCacheableStorage>();
        }
    }
}