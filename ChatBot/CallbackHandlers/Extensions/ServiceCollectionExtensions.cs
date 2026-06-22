using ChatBot.Contracts;
using ChatBot.Renderers;
using Microsoft.Extensions.DependencyInjection;

namespace ChatBot.CallbackHandlers.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCallbackHandlers(this IServiceCollection services)
    {
        services.AddScoped<ICallbackHandler, ClearOptionsCallbackHandler>();
        services.AddScoped<ICallbackHandler, RankCallbackHandler>();
        services.AddScoped<ICallbackHandler, RoleCallbackHandler>();
        services.AddScoped<ICallbackHandler, SortCallbackHandler>();
        services.AddScoped<ICallbackHandler, SwitchFormatCallbackHandler>();
        services.AddScoped<ICallbackHandler, ToPageCallbackHandler>();

        return services;
    }
}