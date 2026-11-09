using Entities.Models;
using Repository.Contracts.Cache;
using Repository.Contracts.Repositories;
using UseCases.Contracts.InputPorts;
using UseCases.Contracts.OutputPorts.ExternalData;

namespace UseCases;

/// <summary>
///     Сервис для управления данными персонажей: обновление, сохранение, загрузка
/// </summary>
public class UpdateHeroStatsUseCase(
    IStratzApiClient source,
    IHeroStatsCache cache,
    IHeroStatsRepository heroStatsRepository,
    IMetaUpdateRepository metaUpdateRepository,
    IUnitOfWork unitOfWork) : IUpdateHeroStatsUseCase
{
    private static readonly TimeSpan StatsLifetime = TimeSpan.FromDays(7);

    public async Task UpdateNewStatsAsync(CancellationToken ct = default)
    {
        var stats = await source.GetHeroStatsAsync(ct);
        if (stats.Count == 0)
            throw new InvalidOperationException("Источник вернул пустой список героев.");

        var names = await source.GetHeroNamesAsync(ct);

        var now = DateTime.UtcNow;
        var cutoff = now - StatsLifetime;

        var outdatedUpdates = await metaUpdateRepository.GetOlderThanAsync(cutoff, ct);
        var oldStats = await GetOldStatsAsync(outdatedUpdates, ct);

        var metaUpdate = new MetaUpdate
        {
            DateTime = now,
            HeroStats = stats.ToList()
        };

        await metaUpdateRepository.AddAsync(metaUpdate, ct);
        metaUpdateRepository.RemoveRange(outdatedUpdates);
        await unitOfWork.SaveChangesAsync(ct);

        cache.Update(stats, oldStats, names, now);
    }

    private async Task<IReadOnlyList<HeroStat>> GetOldStatsAsync(
        IReadOnlyList<MetaUpdate> outdatedUpdates,
        CancellationToken ct)
    {
        if (outdatedUpdates.Count == 0)
            return [];

        // Список отсортирован по убыванию даты, поэтому первым идёт обновление,
        // наиболее близкое к недельной давности.
        var oldUpdate = outdatedUpdates[0];
        return await heroStatsRepository.GetByMetaUpdateIdAsync(oldUpdate.Id, ct);
    }
}
