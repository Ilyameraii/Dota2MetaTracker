using System.Net.Http.Json;
using Api.Contracts;
using Entities.Enums;
using Entities.Models;
using UseCases.Contracts.InputPorts;

namespace TelegramBot.Api;

/// <summary>
///     Получение списка героев из HTTP API
/// </summary>
public class ApiHeroMetaClient(HttpClient http) : IGetHeroMetaUseCase
{
    private const int MaxHeroes = 200;

    private static readonly Dictionary<RankFlags, string> RankCodes = new()
    {
        [RankFlags.Uncalibrated] = "un",
        [RankFlags.HeraldGuardian] = "hg",
        [RankFlags.CrusaderArchon] = "ca",
        [RankFlags.LegendAncient] = "la",
        [RankFlags.DivineImmortal] = "di"
    };

    private static readonly Dictionary<RoleFlags, string> RoleCodes = new()
    {
        [RoleFlags.Safelane] = "Safelane",
        [RoleFlags.Midlane] = "Midlane",
        [RoleFlags.Offlane] = "Offlane",
        [RoleFlags.Support] = "Support",
        [RoleFlags.HardSupport] = "Hard Support"
    };

    private static readonly Dictionary<SortType, string> SortCodes = new()
    {
        [SortType.WinRate] = "wr",
        [SortType.MatchCount] = "pr",
        [SortType.Rating] = "rating",
        [SortType.WinrateDelta] = "dwr",
        [SortType.PickrateDelta] = "dpr",
        [SortType.RatingDelta] = "drt"
    };

    /// <inheritdoc/>
    public async Task<List<Hero>> ExecuteAsync(
        HeroProcessingOptions options,
        CancellationToken cancellationToken = default)
    {
        var response = await http.GetFromJsonAsync<HeroesResponse>(
            BuildUrl(options),
            cancellationToken);

        if (response is null)
            throw new InvalidOperationException("API вернул пустой ответ.");

        return response.Items;
    }

    private static string BuildUrl(HeroProcessingOptions options)
    {
        var query = new List<string>();

        var ranks = RankCodes
            .Where(pair => options.Ranks.HasFlag(pair.Key))
            .Select(pair => pair.Value)
            .ToList();

        if (ranks.Count > 0)
            query.Add($"rank={Uri.EscapeDataString(string.Join(',', ranks))}");

        var roles = RoleCodes
            .Where(pair => options.Roles.HasFlag(pair.Key))
            .Select(pair => pair.Value)
            .ToList();

        if (roles.Count > 0)
            query.Add($"role={Uri.EscapeDataString(string.Join(',', roles))}");

        var sort = SortCodes.GetValueOrDefault(options.SortBy, "rating");
        query.Add($"sort={sort}");
        query.Add($"dir={(options.IsDescending ? "desc" : "asc")}");
        query.Add($"limit={MaxHeroes}");

        return $"{ApiRoutes.Heroes}?{string.Join('&', query)}";
    }
}
