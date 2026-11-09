using Entities.Models;

namespace Repository.Contracts.Repositories;

/// <summary>
///     Репозиторий статистики персонажей
/// </summary>
public interface IHeroStatsRepository
{
    /// <summary>
    ///     Возвращает статистику персонажей для указанного обновления
    /// </summary>
    Task<IReadOnlyList<HeroStat>> GetByMetaUpdateIdAsync(int metaUpdateId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Добавляет набор записей статистики персонажей
    /// </summary>
    Task AddRangeAsync(IEnumerable<HeroStat> heroStats, CancellationToken cancellationToken = default);
}
