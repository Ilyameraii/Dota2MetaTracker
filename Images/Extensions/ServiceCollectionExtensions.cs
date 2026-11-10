using Microsoft.Extensions.DependencyInjection;
using UseCases.Contracts.OutputPorts.ImageGenerating;

namespace Images.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Регистрирует провайдер аватаров героев (встроенные ресурсы и CDN Steam)
    /// </summary>
    public static IServiceCollection AddHeroAvatarProvider(this IServiceCollection services)
    {
        services.AddHttpClient<HeroAvatarProvider>();

        services.AddSingleton<IHeroAvatarProvider>(
            sp => sp.GetRequiredService<HeroAvatarProvider>());

        return services;
    }

    /// <summary>
    ///     Регистрирует генератор изображений с подборкой героев
    /// </summary>
    public static IServiceCollection AddImageGeneration(this IServiceCollection services)
    {
        services.AddSingleton<IHeroStatsImageGenerator, HeroStatsImageGenerator>();

        return services;
    }
}
