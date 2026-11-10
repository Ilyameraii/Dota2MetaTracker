using Telegram.Bot;
using TelegramBot.Api;
using TelegramBot.Factories;
using TelegramBot.Formatters;
using TelegramBot.BackgroundServices;
using UseCases.Contracts.InputPorts;
using UseCases.Contracts.OutputPorts.ImageGenerating;

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

    /// <summary>
    ///     Регистрирует HTTP-клиенты API: данные о героях и их аватары
    /// </summary>
    public static IServiceCollection AddApiClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient<ApiHeroMetaClient>(client => ConfigureApiClient(client, configuration))
            .AddStandardResilienceHandler(options => options.Retry.MaxRetryAttempts = 2);

        services.AddScoped<IGetHeroMetaUseCase>(
            sp => sp.GetRequiredService<ApiHeroMetaClient>());

        services.AddHttpClient<ApiHeroAvatarProvider>(client => ConfigureApiClient(client, configuration))
            .AddStandardResilienceHandler(options => options.Retry.MaxRetryAttempts = 2);

        services.AddSingleton<IHeroAvatarProvider>(
            sp => sp.GetRequiredService<ApiHeroAvatarProvider>());

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

        return services;
    }

    private static void ConfigureApiClient(HttpClient client, IConfiguration configuration)
    {
        var baseUrl = configuration["Api:BaseUrl"]
            ?? throw new InvalidOperationException("Не задан адрес API в секции Api:BaseUrl.");

        client.BaseAddress = new Uri($"{baseUrl.TrimEnd('/')}/");
        client.Timeout = TimeSpan.FromSeconds(10);
    }
}
