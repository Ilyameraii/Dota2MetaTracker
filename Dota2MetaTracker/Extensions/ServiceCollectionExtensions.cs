using ChatBot.BackgroundServices;
using Dota2MetaTracker.BackgroundServices;

namespace Dota2MetaTracker.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBackgroundServices(
        this IServiceCollection services)
    {
        services.AddHostedService<BotBackgroundService>();
        services.AddHostedService<HeroStatsUpdateBackgroundService>();

        return services;
    }
}