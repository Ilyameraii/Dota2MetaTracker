using Entities.Models;

namespace UseCases.Contracts.OutputPorts.ExternalData;

/// <summary>
///     Сервис для взаимодействия с STRATZ GraphQL API
/// </summary>
public interface IStratzApiClient
{
    /// <summary>
    ///     Получает статистику персонажей из STRATZ API
    /// </summary>
    Task<IReadOnlyList<HeroStat>> GetHeroStatsAsync(CancellationToken ct);
    
    /// <summary>
    ///     Получает список имён персонажей из STRATZ API
    /// </summary>
    Task<IReadOnlyDictionary<int, string>> GetHeroNamesAsync(CancellationToken ct);
}