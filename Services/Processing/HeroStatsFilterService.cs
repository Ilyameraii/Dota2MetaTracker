using Entities.Models;
using Entities.Enums;
using Services.Contracts.Interfaces.Processing;
using Services.Processing.Extensions;

namespace Services.Processing;

/// <summary>
///     Сервис для фильтрации статистики персонажей по рангам и ролям
/// </summary>
public class HeroStatsFilterService : IHeroStatsFilterService
{
    /// <summary>
    ///     Применяет фильтры к статистике персонажей
    /// </summary>
    public IReadOnlyList<HeroStat> ApplyFilters(IReadOnlyList<HeroStat> heroStats, RankFlags ranks = RankFlags.None,
        RoleFlags roles = RoleFlags.None)
    {
        var result = heroStats.AsEnumerable();

        if (ranks != RankFlags.None) result = result.Where(h => h.Rank.IsIncludedIn(ranks));

        if (roles != RoleFlags.None) result = result.Where(h => h.Role.IsIncludedIn(roles));

        return result.ToList();
    }
}