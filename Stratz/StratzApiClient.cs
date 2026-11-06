using System.Text;
using System.Text.Json;
using Entities.Models;
using STRATZ;
using UseCases.Contracts.OutputPorts.ExternalData;

namespace Stratz;

/// <summary>
/// Сервис для взаимодействия с STRATZ GraphQL API
/// </summary>
public class StratzApiClient(HttpClient httpClient, StratzHeroStatsParser parser) : IStratzApiClient
{
    private const string GraphQlEndpoint = "https://api.stratz.com/graphql";

    public async Task<IReadOnlyList<HeroStat>> GetHeroStatsAsync(CancellationToken cancellationToken)
    {
        var raw = await PostQueryAsync(QueryOfHeroesStats(), cancellationToken);
        return parser.ParseHeroStats(raw);
    }

    public async Task<IReadOnlyDictionary<int, string>> GetHeroNamesAsync(CancellationToken cancellationToken)
    {
        var raw = await PostQueryAsync(QueryOfHeroesNames(), cancellationToken);
        return parser.ParseHeroesNames(raw);
    }

    // Общий код двух методов вынесен в один, раньше он дублировался
    private async Task<string> PostQueryAsync(string query, CancellationToken cancellationToken)
    {
        var bodyJson = JsonSerializer.Serialize(new { query });
        using var content = new StringContent(bodyJson, Encoding.UTF8, "application/json");

        using var response = await httpClient.PostAsync(GraphQlEndpoint, content, cancellationToken);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync(cancellationToken);
    }

    /// <summary>
    /// Формирует GraphQL запрос для получения имён персонажей
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
    /// Формирует GraphQL запрос для получения статистики персонажей
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