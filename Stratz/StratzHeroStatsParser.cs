using System.Text.Json.Nodes;
using Entities.Enums;
using Entities.Models;

namespace Stratz;

/// <summary>
///     Парсер для преобразования JSON-ответа STRATZ API в модели предметной области
/// </summary>
public class StratzHeroStatsParser
{
    private static readonly Dictionary<string, Rank> RankMap = new()
    {
        ["UNCALIBRATED"] = Rank.Uncalibrated,
        ["HERALD_GUARDIAN"] = Rank.HeraldGuardian,
        ["CRUSADER_ARCHON"] = Rank.CrusaderArchon,
        ["LEGEND_ANCIENT"] = Rank.LegendAncient,
        ["DIVINE_IMMORTAL"] = Rank.DivineImmortal
    };

    private static readonly Dictionary<string, Role> RoleMap = new()
    {
        ["POSITION_1"] = Role.Safelane,
        ["POSITION_2"] = Role.Midlane,
        ["POSITION_3"] = Role.Offlane,
        ["POSITION_4"] = Role.Support,
        ["POSITION_5"] = Role.HardSupport
    };

    /// <summary>
    ///     Парсит JSON со статистикой персонажей и возвращает список HeroStat
    /// </summary>
    public List<HeroStat> ParseHeroStats(string json)
    {
        var root = JsonNode.Parse(json)
                   ?? throw new InvalidOperationException("Invalid JSON");

        var statsArray = root["data"]?["heroStats"]?["stats"]?.AsArray()
                         ?? throw new InvalidOperationException("Path data.heroStats.stats not found");

        return statsArray
            .Where(item => item is not null)
            .Select(item => ParseStatItem(item!))
            .ToList();
    }

    /// <summary>
    ///     Парсит JSON с именами персонажей и возвращает словарь id -> name
    /// </summary>
    public Dictionary<int, string> ParseHeroesNames(string json)
    {
        var root = JsonNode.Parse(json)
                   ?? throw new InvalidOperationException("Invalid JSON");

        var heroesArray = root["data"]?["constants"]?["heroes"]?.AsArray()
                          ?? throw new InvalidOperationException("Path data.constants.heroes not found");

        return heroesArray
            .Where(item => item is not null)
            .ToDictionary(
                item => item!["id"]!.GetValue<int>(),
                item => item!["displayName"]!.GetValue<string>()
            );
    }
    
    private HeroStat ParseStatItem(JsonNode item)
    {
        var rankStr = item["bracketBasicIds"]!.GetValue<string>();
        var roleStr = item["position"]!.GetValue<string>();

        if (!RankMap.TryGetValue(rankStr, out var rank))
        {
            throw new InvalidOperationException($"Unknown rank: {rankStr}");
        }

        if (!RoleMap.TryGetValue(roleStr, out var role))
        {
            throw new InvalidOperationException($"Unknown role: {roleStr}");
        }

        return new HeroStat
        {
            HeroId = item["heroId"]!.GetValue<int>(),
            Rank = rank,
            Role = role,
            WinCount = item["winCount"]!.GetValue<int>(),
            MatchCount = item["matchCount"]!.GetValue<int>()
        };
    }
}