using Entities.Models;

namespace Repository.Contracts.Repositories;

/// <summary>
///     Репозиторий обновлений статистики
/// </summary>
public interface IMetaUpdateRepository
{
    /// <summary>
    ///     Возвращает последнее обновление статистики или <see langword="null" />, если обновлений нет
    /// </summary>
    Task<MetaUpdate?> GetLatestAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Возвращает указанное количество последних обновлений статистики
    /// </summary>
    Task<IReadOnlyList<MetaUpdate>> GetLatestAsync(int count, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Возвращает обновление статистики по идентификатору или <see langword="null" />, если оно не найдено
    /// </summary>
    Task<MetaUpdate?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Добавляет новое обновление статистики
    /// </summary>
    Task AddAsync(MetaUpdate metaUpdate, CancellationToken cancellationToken = default);
}
