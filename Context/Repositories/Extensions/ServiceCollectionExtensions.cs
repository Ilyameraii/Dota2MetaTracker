using Microsoft.Extensions.DependencyInjection;
using Repository.Contracts.Repositories;

namespace Context.Repositories.Extensions;

/// <summary>
///     Расширения регистрации зависимостей репозиториев
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Регистрирует репозитории статистики персонажей и обновлений
    /// </summary>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IHeroStatsRepository, HeroStatsRepository>();
        services.AddScoped<IMetaUpdateRepository, MetaUpdateRepository>();

        return services;
    }
}
