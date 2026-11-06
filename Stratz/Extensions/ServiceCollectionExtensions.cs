using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UseCases.Contracts.OutputPorts.ExternalData;

namespace Stratz.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddStratzServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<IStratzApiClient, StratzApiClient>(client =>
        {
            client.DefaultRequestHeaders.Add("User-Agent", "STRATZ_API");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", configuration["StratzApi:Token"]);
        });

        services.AddSingleton<StratzHeroStatsParser>();

        return services;
    }
}