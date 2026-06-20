using ChatBot;
using ChatBot.CommandHandlers;
using ChatBot.Contracts;
using ChatBot.Renderers;
using Telegram.Bot;
using Telegram.Bot.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, _) =>
        new TelegramBotClient(builder.Configuration["Telegram:Token"]!, httpClient));

// Bot client
builder.Services.AddScoped<BotClient>();

// Renderers
builder.Services.AddScoped<MenuRenderer>();

// Callback handlers
//...

// Command handlers
builder.Services.AddScoped<ICommandHandler, StartCommandHandler>();


var app = builder.Build();

// Регистрируем Webhook у Telegram
using (var scope = app.Services.CreateScope())
{
    var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
    var webhookUrl = builder.Configuration["Telegram:WebhookUrl"]!;
    var secretToken = builder.Configuration["Telegram:SecretToken"]!;
    
    await botClient.SetWebhook(
        url: webhookUrl,
        secretToken: secretToken);
}


app.MapPost("/bot", async (
    Update update,
    BotClient botClient,
    HttpRequest request,
    CancellationToken ct) =>
{
    var secretToken = builder.Configuration["Telegram:SecretToken"];

    if (request.Headers["X-Telegram-Bot-Api-Secret-Token"] != secretToken)
    {
        return Results.Unauthorized(); // 401, отклоняем запрос
    }
    
    await botClient.HandleUpdateAsync(update, ct);
    return Results.Ok();
});

app.Run();