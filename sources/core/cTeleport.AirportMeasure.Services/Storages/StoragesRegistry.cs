using cTeleport.AirportMeasure.Core;
using cTeleport.AirportMeasure.Services.Storages.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace cTeleport.AirportMeasure.Services.Storages
{
    public static class StoragesRegistry
    {
        public static IServiceCollection AddInMemoryCacheableStorage(this IServiceCollection services)
            => services.AddSingleton<ICacheableStorage, MemoryCacheableStorage>();
    }
}