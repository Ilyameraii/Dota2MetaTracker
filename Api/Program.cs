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

var allowedOrigins = (builder.Configuration["Cors:Origins"] ?? string.Empty)
    .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

builder.Services.AddOpenApi();

if (allowedOrigins.Length > 0)
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
            policy.WithOrigins(allowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod());
    });
}

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

if (allowedOrigins.Length > 0)
{
    app.UseCors();
}

app.MapHeroEndpoints();

app.Run();
