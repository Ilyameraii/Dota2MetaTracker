using Microsoft.Extensions.DependencyInjection;
using UseCases.Contracts.OutputPorts.Cache;

namespace Cache.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        services.AddSingleton<IHeroStatsCache, HeroStatsCache>();
        
        return services;
    }
}