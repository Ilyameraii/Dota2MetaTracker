using TelegramBot.Constants;
using TelegramBot.Contracts;
using TelegramBot.Renderers;
using Repository.Contracts;

namespace TelegramBot.CallbackHandlers;

/// <summary>
/// Реализация <see cref="ICallbackHandler"/> для обработки перехода на указанную страницу списка.
/// </summary>
public class ToPageCallbackHandler(
    IUserPreferencesCache cache,
    HeroStatsMessageRenderer renderer) : CallbackHandlerBase(cache, renderer)
{
    /// <inheritdoc/>
    public override bool CanHandle(string callbackData) => callbackData.StartsWith(CallbackData.Prefixes.ToPage);

    /// <inheritdoc/>
    protected override bool TryUpdate(long userId, string callbackData)
    {
        if (!TryParsePageData(callbackData, out var page, out var max))
            return false;

        PreferencesCache.SetPage(userId, page, max);
        return true;
    }

    private static bool TryParsePageData(string callbackData, out int page, out int max)
    {
        page = max = 0;
        var parts = callbackData[CallbackData.Prefixes.ToPage.Length..].Split(':');

        if (parts.Length == 1 && int.TryParse(parts[0], out page))
        {
            max = int.MaxValue;
            return true;
        }

        return parts.Length == 2
               && int.TryParse(parts[0], out page)
               && int.TryParse(parts[1], out max)
               && max >= 0;
    }
}
