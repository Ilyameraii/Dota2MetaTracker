using System.Globalization;
using ChatBot.Extensions.Helpers;
using ChatBot.Helpers;
using Services.Contracts.Models;

namespace ChatBot.Formatters;

/// <summary>
///     Форматировщик информации о персонажах для отображения
/// </summary>
public class HeroTextFormatter
{
    /// <summary>
    ///     Форматирует информацию о персонаже (имя, винрейт и пикрейт)
    /// </summary>
    public string Format(Hero hero)
    {
        var winRate = hero.WinRate;
        var pickRate = hero.PickRate;

        return
            $"<b>{hero.Name}</b> - <b>{(winRate * 100).ToString("F2", CultureInfo.InvariantCulture)}%</b> " +
            $" побед, <b>{(pickRate * 100).ToString("F2", CultureInfo.InvariantCulture)}%</b>  выборов";
    }
    
    /// <summary>
    ///     Форматирует информацию о персонаже (имя, винрейт и пикрейт)
    /// </summary>
    public string FormatWithDelta(Hero hero)
    {
        var winRate = hero.WinRate;
        var pickRate = hero.PickRate;

        var winRateDelta = hero.WinRateDelta;
        var pickRateDelta = hero.PickRateDelta;

        return
            $"<b>{hero.Name}</b> - <b>{(winRate * 100).ToString("F2", CultureInfo.InvariantCulture)}%</b> " +
            $"(<b>{winRateDelta.FormatDelta()} %</b>) побед, " +
            $"<b>{(pickRate * 100).ToString("F2", CultureInfo.InvariantCulture)}%</b> " +
            $"(<b>{pickRateDelta.FormatDelta()} %</b>) выборов";
    }
}