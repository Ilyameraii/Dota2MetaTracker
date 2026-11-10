using Cache.Extensions;
using TelegramBot.CallbackHandlers.Extensions;
using TelegramBot.CommandHandlers.Extensions;
using TelegramBot.Renderers.Extensions.DI;
using TelegramBot.Extensions;
using Images.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddTelegramBotClient(builder.Configuration);
builder.Services.AddUserPreferencesCache();
builder.Services.AddRenderers();
builder.Services.AddCallbackHandlers();
builder.Services.AddCommandHandlers();
builder.Services.AddApiClients(builder.Configuration);
builder.Services.AddImageGeneration();
builder.Services.AddFactories();
builder.Services.AddFormatters();
builder.Services.AddBackgroundServices();

// построение приложения
var app = builder.Build();

app.MapBotEndpoints(builder.Configuration);

app.Run();
