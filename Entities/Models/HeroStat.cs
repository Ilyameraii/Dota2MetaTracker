using Entities.Enums;

namespace Entities.Models;

/// <summary>
///     Сущность статистики персонажа на конкретном ранге и роли
/// </summary>
public class HeroStat
{
    /// <summary>
    ///     Идентификатор статистики - первичный ключ
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Идентификатор персонажа
    /// </summary>
    public int HeroId { get; set; }

    /// <summary>
    ///     Ранг матчей, на котором ведется статистика персонажа
    /// </summary>
    public Rank Rank { get; set; }

    /// <summary>
    ///     Роль, на которой ведется статистика персонажа
    /// </summary>
    public Role Role { get; set; }

    /// <summary>
    ///     Количество побед персонажа на конкретной роли и на конкретном ранге
    /// </summary>
    public int WinCount { get; set; }

    /// <summary>
    ///     Количество матчей на персонаже на конкретной роли и на конкретном ранге
    /// </summary>
    public int MatchCount { get; set; }

    /// <summary>
    ///     Идентификатор обновления статистики
    /// </summary>
    // Только для БД: внешний ключ и навигация
    public int MetaUpdateId { get; set; }

    /// <summary>
    ///     Навигационное свойство для связи с таблицей MetaUpdate
    /// </summary>
    public virtual MetaUpdate MetaUpdate { get; set; } = null!;
}