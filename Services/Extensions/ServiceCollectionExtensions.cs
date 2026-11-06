using Microsoft.Extensions.DependencyInjection;
using Services.Contracts.Enums;
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

        services.AddScoped<IHeroSortStategy>(_ => new NotImplementedHeroSortStrategy(SortType.WinRate));
        services.AddScoped<IHeroSortStategy>(_ => new NotImplementedHeroSortStrategy(SortType.WinrateDelta));
        services.AddScoped<IHeroSortStategy>(_ => new NotImplementedHeroSortStrategy(SortType.MatchCount));
        services.AddScoped<IHeroSortStategy>(_ => new NotImplementedHeroSortStrategy(SortType.PickrateDelta));
        services.AddScoped<IHeroSortStategy>(_ => new NotImplementedHeroSortStrategy(SortType.Rating));
        services.AddScoped<IHeroSortStategy>(_ => new NotImplementedHeroSortStrategy(SortType.RatingDelta));

        return services;
    }
}
