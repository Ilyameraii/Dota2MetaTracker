using Microsoft.Extensions.DependencyInjection;
using Entities.Enums;
using Services.Contracts.Interfaces.Processing;
using Services.Processing;
using Services.Processing.Sorting;

namespace Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IHeroStatsFilterService, HeroStatsFilterService>();
        services.AddScoped<IHeroStatsAggregator, HeroStatsAggregator>();
        services.AddScoped<IHeroCalculator, HeroCalculator>();
        services.AddScoped<IHeroStatsProcessor, HeroStatsProcessor>();

        services.AddSortStrategies();
        

        return services;
    }

    private static IServiceCollection AddSortStrategies(this IServiceCollection services)
    {
        services.AddScoped<IHeroSortStategy, MatchCountSortStrategy>();
        services.AddScoped<IHeroSortStategy, PickrateDeltaSortStrategy>();
        services.AddScoped<IHeroSortStategy, RatingDeltaSortStrategy>();
        services.AddScoped<IHeroSortStategy, RatingSortStrategy>();
        services.AddScoped<IHeroSortStategy, WinrateDeltaSortStrategy>();
        services.AddScoped<IHeroSortStategy, WinrateSortStrategy>();
        return services;
    }
}
