using Entities.Enums;
using Entities.Models;

namespace Repository.Contracts;

/// <summary>
///     Кэш настроек пользователей.
/// </summary>
public interface IUserPreferencesCache
{
    /// <summary>
    ///     Возвращает настройки пользователя, создавая их при первом обращении.
    /// </summary>
    UserPreferences GetOrCreate(long userId);

    /// <summary>
    ///     Устанавливает номер страницы в допустимых пределах.
    /// </summary>
    void SetPage(long userId, int page, int max);

    /// <summary>
    ///     Переключает фильтр по рангу.
    /// </summary>
    void ToggleRank(long userId, RankFlags rank);

    /// <summary>
    ///     Переключает фильтр по роли.
    /// </summary>
    void ToggleRole(long userId, RoleFlags role);

    /// <summary>
    ///     Устанавливает способ сортировки или меняет её направление при повторном выборе.
    /// </summary>
    void SetSort(long userId, SortType sortType);

    /// <summary>
    ///     Сбрасывает настройки пользователя до значений по умолчанию.
    /// </summary>
    void Reset(long userId);

    /// <summary>
    ///     Переключает формат отображения между изображением и текстом.
    /// </summary>
    void SwitchFormat(long userId);
}
