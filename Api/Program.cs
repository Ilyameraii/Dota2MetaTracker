using System.Text.Json.Serialization;
using Api.BackgroundServices;
using Api.Endpoints;
using Cache.Extensions;
using Context.Extensions;
using Images.Extensions;
using Services.Extensions;
using Stratz.Extensions;
using UseCases.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddStratzServices(builder.Configuration);
builder.Services.AddContextServices(builder.Configuration);
builder.Services.AddHeroStatsCache();
builder.Services.AddServices();
builder.Services.AddUseCases();
builder.Services.AddHeroAvatarProvider();
builder.Services.AddHostedService<HeroStatsUpdateBackgroundService>();

var app = builder.Build();

app.MapHeroEndpoints();

app.Run();
