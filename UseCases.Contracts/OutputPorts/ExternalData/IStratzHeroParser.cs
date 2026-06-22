using Entities.Models;

namespace UseCases.Contracts.OutputPorts.ExternalData;

/// <summary>
///     Парсер для преобразования JSON-ответа STRATZ API в модели предметной области
/// </summary>
public interface IStratzHeroParser
{
    /// <summary>
    ///     Парсит JSON со статистикой персонажей
    /// </summary>
    public List<HeroStat> ParseHeroStats(string json);

    /// <summary>
    ///     Парсит JSON с именами персонажей
    /// </summary>
    public Dictionary<int, string> ParseHeroesNames(string json);
}