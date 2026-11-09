using TelegramBot.Constants;
using TelegramBot.Contracts;
using TelegramBot.Renderers;
using Repository.Contracts;

namespace TelegramBot.CallbackHandlers;

/// <summary>
/// Реализация <see cref="ICallbackHandler"/> для обработки переключения формата отображения.
/// </summary>
public class SwitchFormatCallbackHandler(
    IUserPreferencesCache cache,
    HeroStatsMessageRenderer renderer) : CallbackHandlerBase(cache, renderer)
{
    /// <inheritdoc/>
    public override bool CanHandle(string callbackData) => callbackData == CallbackData.Actions.SwitchFormat;

    /// <inheritdoc/>
    protected override bool TryUpdate(long userId, string callbackData)
    {
        PreferencesCache.SwitchFormat(userId);
        return true;
    }
}
