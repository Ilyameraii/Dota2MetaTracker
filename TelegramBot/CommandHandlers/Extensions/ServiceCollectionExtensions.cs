using TelegramBot.Contracts;
using TelegramBot.Renderers;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot.CommandHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler,StartCommandHandler>();

        return services;
    }
}