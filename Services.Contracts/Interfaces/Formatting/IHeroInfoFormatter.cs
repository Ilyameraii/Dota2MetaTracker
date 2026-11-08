using Entities.Models;

namespace Services.Contracts.Interfaces.Formatting;

/// <summary>
///     Форматировщик информации о персонажах
/// </summary>
public interface IHeroInfoFormatter
{
    /// <summary>
    ///     Форматирует информацию о персонаже (имя, винрейт и пикрейт) с показателем динамики изменения
    /// </summary>
    string FormatWithDelta(Hero hero);
}