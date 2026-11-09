using Microsoft.Extensions.DependencyInjection;
using UseCases.Contracts.OutputPorts.ImageGenerating;

namespace Images.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImages(this IServiceCollection services)
    {
        services.AddHttpClient<HeroAvatarProvider>();

        services.AddSingleton<IHeroAvatarProvider>(
            sp => sp.GetRequiredService<HeroAvatarProvider>());

        services.AddSingleton<IHeroStatsImageGenerator, HeroStatsImageGenerator>();

        return services;
    }
}