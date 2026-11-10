using Entities.Models;

namespace Api.Contracts;

/// <summary>
///     Ответ API со списком героев и временем последнего обновления статистики
/// </summary>
/// <param name="Total">Общее количество героев до применения ограничения выборки</param>
/// <param name="UpdatedAt">Время последнего обновления статистики</param>
/// <param name="Items">Список героев</param>
public sealed record HeroesResponse(
    int Total,
    DateTimeOffset UpdatedAt,
    List<Hero> Items);
