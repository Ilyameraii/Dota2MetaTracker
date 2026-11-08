using ChatBot.Constants;
using ChatBot.Contracts;
using ChatBot.Renderers;
using Repository.Contracts;
using Entities.Enums;

namespace ChatBot.CallbackHandlers;

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
