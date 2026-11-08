using ChatBot.Constants;
using ChatBot.Contracts;
using ChatBot.Renderers;
using Repository.Contracts;
using Entities.Enums;

namespace ChatBot.CallbackHandlers;

/// <summary>
/// Реализация <see cref="ICallbackHandler"/> для обработки выбора способа сортировки списка.
/// </summary>
public class SortCallbackHandler(
    IUserPreferencesCache cache,
    HeroStatsMessageRenderer renderer) : CallbackHandlerBase(cache, renderer)
{
    /// <inheritdoc/>
    public override bool CanHandle(string callbackData) => callbackData.StartsWith(CallbackData.Prefixes.Sort);

    /// <inheritdoc/>
    protected override bool TryUpdate(long userId, string callbackData)
    {
        if (!TryParseEnumValue(callbackData, CallbackData.Prefixes.Sort, out SortType sortType))
            return false;

        PreferencesCache.SetSort(userId, sortType);
        return true;
    }
}
