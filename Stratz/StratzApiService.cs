using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using STRATZ;
using UseCases.Contracts.OutputPorts.ExternalData;

namespace Stratz;

/// <summary>
///     Сервис для взаимодействия с STRATZ GraphQL API
/// </summary>
public class StratzApiService(HttpClient httpClient) : IStratzApiService
{
    private const string GraphQlEndpoint = "https://api.stratz.com/graphql";
    /// <summary>
    ///     Получает статистику персонажей из STRATZ API
    /// </summary>
    public async Task<string> GetHeroesStats()
    {
        var query = QueryOfHeroesStats();

        // Оборачиваем запрос в JSON объект
        var bodyObj = new { query };
        var bodyJson = JsonSerializer.Serialize(bodyObj);
        var content = new StringContent(bodyJson, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(GraphQlEndpoint, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    ///     Получает список имён персонажей из STRATZ API
    /// </summary>
    public async Task<string> GetHeroesNames()
    {
        var query = QueryOfHeroesNames();

        // Оборачиваем запрос в JSON объект
        var bodyObj = new { query };
        var bodyJson = JsonSerializer.Serialize(bodyObj);
        var content = new StringContent(bodyJson, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(GraphQlEndpoint, content);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    ///     Формирует GraphQL запрос для получения имён персонажей
    /// </summary>
    private string QueryOfHeroesNames()
    {
        var dotaQueryQueryBuilder = new DotaQueryQueryBuilder()
            .WithConstants(new ConstantQueryQueryBuilder()
                .WithHeroes(new HeroTypeQueryBuilder()
                    .WithId()
                    .WithDisplayName()
                )
            );
        return dotaQueryQueryBuilder.Build();
    }

    /// <summary>
    ///     Формирует GraphQL запрос для получения статистики персонажей
    /// </summary>
    private string QueryOfHeroesStats()
    {
        var dotaQueryQueryBuilder = new DotaQueryQueryBuilder()
            .WithHeroStats(new HeroStatsQueryQueryBuilder()
                .WithStats(
                    new HeroPositionTimeDetailTypeQueryBuilder()
                        .WithHeroId()
                        .WithBracketBasicIds()
                        .WithPosition()
                        .WithWinCount()
                        .WithMatchCount(),
                    groupByPosition: true,
                    groupByBracket: true
                )
            );

        return dotaQueryQueryBuilder.Build();
    }
}