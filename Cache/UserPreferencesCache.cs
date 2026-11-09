using Microsoft.Extensions.Caching.Memory;
using Repository.Contracts;
using Entities.Enums;
using Entities.Models;

namespace Users;

/// <summary>
///     Сервис для управления настройками пользователей в памяти.
///     Записи автоматически удаляются после месяца неактивности.
/// </summary>
public class UserPreferencesCache(IMemoryCache cache) : IUserPreferencesCache
{
    private static readonly MemoryCacheEntryOptions EntryOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromDays(30));

    /// <inheritdoc/>
    public UserPreferences GetOrCreate(long userId)
    {
        return cache.GetOrCreate(userId, entry =>
        {
            entry.SetOptions(EntryOptions);
            return new UserPreferences();
        })!;
    }

    /// <inheritdoc/>
    public void SetPage(long userId, int page, int max)
    {
        var preferences = GetOrCreate(userId);
        preferences.PageNumber = Math.Clamp(page, 0, Math.Max(max, 0));
    }

    /// <inheritdoc/>
    public void ToggleRank(long userId, RankFlags rank)
    {
        var preferences = GetOrCreate(userId);
        preferences.ProcessingOptions.Ranks ^= rank;
        preferences.PageNumber = 0;
    }

    /// <inheritdoc/>
    public void ToggleRole(long userId, RoleFlags role)
    {
        var preferences = GetOrCreate(userId);
        preferences.ProcessingOptions.Roles ^= role;
        preferences.PageNumber = 0;
    }

    /// <inheritdoc/>
    public void SetSort(long userId, SortType sortType)
    {
        var preferences = GetOrCreate(userId);
        var options = preferences.ProcessingOptions;

        if (options.SortBy == sortType)
        {
            options.IsDescending = !options.IsDescending;
        }
        else
        {
            options.SortBy = sortType;
            options.IsDescending = true;
        }

        preferences.PageNumber = 0;
    }

    /// <inheritdoc/>
    public void Reset(long userId) => GetOrCreate(userId).Reset();

    /// <inheritdoc/>
    public void SwitchFormat(long userId) => GetOrCreate(userId).SwitchFormat();
}
