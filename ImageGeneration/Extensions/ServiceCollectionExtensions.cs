using Microsoft.Extensions.DependencyInjection;
using UseCases.Contracts.OutputPorts.ImageGeneration;

namespace ImageGeneration.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddImageGeneration(this IServiceCollection services)
    {
        services.AddSingleton<IHeroTopImageGenerator, HeroTopImageGenerator>();
        services.AddSingleton<IHeroAvatarProvider, NotImplementedHeroAvatarProvider>();

        return services;
    }
}
