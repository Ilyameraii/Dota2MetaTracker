using Cache.Extensions;
using ChatBot.CallbackHandlers.Extensions;
using ChatBot.CommandHandlers.Extensions;
using ChatBot.Extensions.DI;
using ChatBot.Renderers.Extensions.DI;
using Context.Extensions;
using Dota2MetaTracker.BackgroundServices;
using Dota2MetaTracker.Extensions;
using Images.Extensions;
using Services.Extensions;
using Stratz.Extensions;
using UseCases.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddTelegramBotClient(builder.Configuration);
builder.Services.AddStratzServices(builder.Configuration);
builder.Services.AddContextServices(builder.Configuration);
builder.Services.AddCacheServices();
builder.Services.AddRenderers();
builder.Services.AddCallbackHandlers();
builder.Services.AddCommandHandlers();
builder.Services.AddUseCases();
builder.Services.AddServices();
builder.Services.AddImages();
builder.Services.AddFactories();
builder.Services.AddFormatters();
builder.Services.AddBackgroundServices();

// построение приложения
var app = builder.Build();

app.MapBotEndpoints(builder.Configuration);

app.Run();