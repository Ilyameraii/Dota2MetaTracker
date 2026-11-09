namespace Repository.Contracts.Repositories;

/// <summary>
///     Единица работы для сохранения изменений в базе данных
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    ///     Сохраняет все отслеживаемые изменения
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
