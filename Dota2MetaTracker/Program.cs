using Cache.Extensions;
using ChatBot.CallbackHandlers.Extensions;
using ChatBot.CommandHandlers.Extensions;
using ChatBot.Extensions;
using ChatBot.Extensions.DI;
using ChatBot.Renderers.Extensions;
using ChatBot.Renderers.Extensions.DI;
using Dota2MetaTracker.Extensions;
using Stratz.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddTelegramBotClient(builder.Configuration);
builder.Services.AddStratzServices(builder.Configuration);
builder.Services.AddCacheServices();
builder.Services.AddRenderers();
builder.Services.AddCallbackHandlers();
builder.Services.AddCommandHandlers();

// построение приложения
var app = builder.Build();

await app.SetupTelegramWebhook();
app.MapBotEndpoints(builder.Configuration);

app.Run();