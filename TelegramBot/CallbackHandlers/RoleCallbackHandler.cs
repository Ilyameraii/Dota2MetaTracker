using TelegramBot.Constants;
using TelegramBot.Contracts;
using TelegramBot.Renderers;
using Repository.Contracts;
using Entities.Enums;

namespace TelegramBot.CallbackHandlers;

/// <summary>
/// Реализация <see cref="ICallbackHandler"/> для обработки выбора фильтра по роли.
/// </summary>
public class RoleCallbackHandler(
    IUserPreferencesCache cache,
    HeroStatsMessageRenderer renderer) : CallbackHandlerBase(cache, renderer)
{
    /// <inheritdoc/>
    public override bool CanHandle(string callbackData) => callbackData.StartsWith(CallbackData.Prefixes.Role);

    /// <inheritdoc/>
    protected override bool TryUpdate(long userId, string callbackData)
    {
        if (!TryParseEnumValue(callbackData, CallbackData.Prefixes.Role, out RoleFlags role))
            return false;

        PreferencesCache.ToggleRole(userId, role);
        return true;
    }
}
