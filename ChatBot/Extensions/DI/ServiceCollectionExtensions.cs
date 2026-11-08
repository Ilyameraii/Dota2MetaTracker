using ChatBot.BackgroundServices;
using ChatBot.Factories;
using ChatBot.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;

namespace ChatBot.Extensions.DI;

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
        services.AddHostedService<BotBackgroundService>();

        return services;
    }
    
    public static IServiceCollection AddFactories(this IServiceCollection services)
    {
        services.AddSingleton<HeroStatsKeyboardFactory>();
        services.AddSingleton<HeroStatsTextFactory>();
        services.AddSingleton<HeroStatsImageFactory>();
        return services;
    }
    
    public static IServiceCollection AddFormatters(this IServiceCollection services)
    {
        services.AddSingleton<HeroTextFormatter>();
        return services;
    }
}