using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot.Renderers.Extensions.DI;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRenderers(this IServiceCollection services)
    {
        services.AddScoped<HeroStatsMessageRenderer>();
        return services;
    }
}