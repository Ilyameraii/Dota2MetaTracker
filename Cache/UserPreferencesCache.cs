using Microsoft.Extensions.Caching.Memory;
using Repository.Contracts;
using Services.Contracts.Models;

namespace Users;

/// <summary>
///     Сервис для управления настройками пользователей в памяти.
///     Записи автоматически удаляются после месяца неактивности.
/// </summary>
public class UserPreferencesCache(IMemoryCache cache) : IUserPreferencesCache
{
    private static readonly MemoryCacheEntryOptions EntryOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromDays(30));

    /// <summary>
    ///     Возвращает настройки пользователя, создавая их при первом обращении.
    /// </summary>
    public UserPreferences GetOrCreate(long userId)
    {
        return cache.GetOrCreate(userId, entry =>
        {
            entry.SetOptions(EntryOptions);
            return new UserPreferences();
        })!;
    }   
    /// <summary>
    ///     Применяет изменения к настройкам пользователя и возвращает обновлённый объект.
    /// </summary>
    public UserPreferences Update(long userId, Action<UserPreferences> update)
    {
        var preferences = GetOrCreate(userId);
        update(preferences);
        return preferences;
    }
}