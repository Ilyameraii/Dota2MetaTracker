using TelegramBot.Constants;
using TelegramBot.Contracts;
using TelegramBot.Renderers;
using Repository.Contracts;
using Entities.Enums;

namespace TelegramBot.CallbackHandlers;

/// <summary>
/// Реализация <see cref="ICallbackHandler"/> для обработки выбора фильтра по рангу.
/// </summary>
public class RankCallbackHandler(
    IUserPreferencesCache cache,
    HeroStatsMessageRenderer renderer) : CallbackHandlerBase(cache, renderer)
{
    /// <inheritdoc/>
    public override bool CanHandle(string callbackData) => callbackData.StartsWith(CallbackData.Prefixes.Rank);

    /// <inheritdoc/>
    protected override bool TryUpdate(long userId, string callbackData)
    {
        if (!TryParseEnumValue(callbackData, CallbackData.Prefixes.Rank, out RankFlags rank))
            return false;

        PreferencesCache.ToggleRank(userId, rank);
        return true;
    }
}
