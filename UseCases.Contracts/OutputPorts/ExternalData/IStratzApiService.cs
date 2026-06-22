namespace UseCases.Contracts.OutputPorts.ExternalData;

/// <summary>
///     Сервис для взаимодействия с STRATZ GraphQL API
/// </summary>
public interface IStratzApiService
{
    /// <summary>
    ///     Получает статистику персонажей из STRATZ API
    /// </summary>
    public Task<string> GetHeroesStats(CancellationToken cancellationToken);

    /// <summary>
    ///     Получает список имён персонажей из STRATZ API
    /// </summary>
    public Task<string> GetHeroesNames(CancellationToken cancellationToken);
}