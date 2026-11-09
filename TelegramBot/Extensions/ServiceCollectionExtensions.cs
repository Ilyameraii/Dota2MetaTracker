using Telegram.Bot;
using TelegramBot.Factories;
using TelegramBot.Formatters;
﻿using TelegramBot.BackgroundServices;

namespace TelegramBot.Extensions;

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
    
    public static IServiceCollection AddBackgroundServices(
        this IServiceCollection services)
    {
        services.AddHostedService<BotBackgroundService>();
        services.AddHostedService<HeroStatsUpdateBackgroundService>();

        return services;
    }
}