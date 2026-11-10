using Microsoft.Extensions.DependencyInjection;
using Repository.Contracts;
using Repository.Contracts.Cache;
using Users;

namespace Cache.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Регистрирует кэш статистики героев
    /// </summary>
    public static IServiceCollection AddHeroStatsCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IHeroStatsCache, HeroStatsCache>();

        return services;
    }

    /// <summary>
    ///     Регистрирует кэш настроек пользователей
    /// </summary>
    public static IServiceCollection AddUserPreferencesCache(this IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddSingleton<IUserPreferencesCache, UserPreferencesCache>();

        return services;
    }
}
