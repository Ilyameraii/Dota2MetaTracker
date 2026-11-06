using Microsoft.Extensions.DependencyInjection;
using Services.Contracts.Interfaces.Processing;
using Services.Processing;

namespace Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IHeroCalculator, HeroCalculator>();
        services.AddSingleton<IHeroStatsAggregator, HeroStatsAggregator>();
        services.AddSingleton<IHeroStatsFilterService, HeroStatsFilterService>();
        services.AddSingleton<IHeroStatsProcessor, HeroStatsProcessor>();
        return services;
    }
}