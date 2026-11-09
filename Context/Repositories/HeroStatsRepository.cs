using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts.Repositories;

namespace Context.Repositories;

/// <inheritdoc/>
public class HeroStatsRepository(MetaTrackerDbContext context) : IHeroStatsRepository
{
    /// <inheritdoc/>
    public async Task<IReadOnlyList<HeroStat>> GetByMetaUpdateIdAsync(
        int metaUpdateId,
        CancellationToken cancellationToken = default) =>
        await context.HeroStats
            .AsNoTracking()
            .Where(heroStat => heroStat.MetaUpdateId == metaUpdateId)
            .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public Task AddRangeAsync(
        IEnumerable<HeroStat> heroStats,
        CancellationToken cancellationToken = default) =>
        context.HeroStats.AddRangeAsync(heroStats, cancellationToken);
}
