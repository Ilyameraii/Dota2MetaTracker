using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.Renderers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRenderers(this IServiceCollection services)
    {
        services.AddScoped<MenuRenderer>();

        return services;
    }
}