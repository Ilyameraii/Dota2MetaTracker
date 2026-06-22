using System.Net.Http.Headers;
using ChatBot;
using ChatBot.CommandHandlers;
using ChatBot.Contracts;
using ChatBot.Extensions;
using ChatBot.Extensions.DI;
using ChatBot.Renderers;
using Dota2MetaTracker.Extensions;
using Stratz;
using Stratz.Extensions;
using Telegram.Bot;
using Telegram.Bot.Types;
using UseCases.Contracts.OutputPorts.ExternalData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddTelegramBotClient(builder.Configuration);

// Bot client
builder.Services.AddScoped<BotClient>();

// Renderers
builder.Services.AddScoped<MenuRenderer>();

// Callback handlers
//...

// Command handlers
builder.Services.AddScoped<ICommandHandler, StartCommandHandler>();
builder.Services.AddStratzServices(builder.Configuration);

var app = builder.Build();

await app.SetupTelegramWebhook();
app.MapBotEndpoints(builder.Configuration);

app.Run();