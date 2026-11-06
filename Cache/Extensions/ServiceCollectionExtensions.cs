using Microsoft.Extensions.DependencyInjection;
using Repository.Contracts;
using Repository.Contracts.Cache;
using Users;

namespace Cache.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IHeroStatsCache, HeroStatsCache>();
        services.AddSingleton<IUserPreferencesCache, UserPreferencesCache>();

        return services;
    }
}