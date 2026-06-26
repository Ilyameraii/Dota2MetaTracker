using Microsoft.Extensions.DependencyInjection;
using Repository.Contracts.Cache;

namespace Cache.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        services.AddSingleton<IHeroStatsCache, HeroStatsCache>();
        
        return services;
    }
}