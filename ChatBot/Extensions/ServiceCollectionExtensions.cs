using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace ChatBot.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTelegramBotClient(
    this IServiceCollection services,
    IConfiguration configuration)
{
    services.AddHttpClient("telegram_bot_client")
        .AddTypedClient<ITelegramBotClient>((httpClient, _) =>
            new TelegramBotClient(
                token: configuration["Telegram:Token"]!,
                httpClient: httpClient));
    
    services.AddScoped<BotClient>();
    
    return services;
}
}