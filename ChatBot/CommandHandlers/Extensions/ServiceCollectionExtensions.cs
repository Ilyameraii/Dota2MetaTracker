using ChatBot.Contracts;
using ChatBot.Renderers;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.CommandHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler,StartCommandHandler>();

        return services;
    }
}