using Microsoft.Extensions.DependencyInjection;
using UseCases.Contracts.InputPorts;

namespace UseCases.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddSingleton<IGetHeroMetaUseCase, GetHeroMetaUseCase>();
        return services;
    }
}