namespace UseCases.Contracts.InputPorts;

/// <summary>
///     Сервис для управления данными персонажей
/// </summary>
public interface IUpdateHeroStatsUseCase
{
    /// <summary>
    ///     Обновляет данные о персонажах из STRATZ API
    /// </summary>
    Task UpdateNewStatsAsync(CancellationToken cancellationToken = default);
}