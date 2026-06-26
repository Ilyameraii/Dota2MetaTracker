using Entities.Models;
using Repository.Contracts.Cache;

namespace Cache;

/// <inheritdoc/>
public class HeroStatsCache : IHeroStatsCache
{
    /// <inheritdoc/>
    public IReadOnlyList<HeroStat> NewHeroesStats { get; private set; } = [];

    /// <inheritdoc/>
    public IReadOnlyList<HeroStat> OldHeroesStats { get; } = [];

    /// <inheritdoc/>
    public IReadOnlyDictionary<int, string> HeroesNames { get; private set; } = new Dictionary<int, string>();

    /// <inheritdoc/>
    public DateTime UpdateTime { get; private set; }

    /// <inheritdoc/>
    public int HeroCount => HeroesNames.Count;

    /// <inheritdoc/>
    public bool IsLoaded => NewHeroesStats.Count != 0 && HeroesNames.Count != 0;

    /// <inheritdoc/>
    public void Update(IReadOnlyList<HeroStat> stats, IReadOnlyDictionary<int, string> names, DateTime updatedAt)
    {
        NewHeroesStats = stats;
        HeroesNames = names;
        UpdateTime = updatedAt;
    }
}