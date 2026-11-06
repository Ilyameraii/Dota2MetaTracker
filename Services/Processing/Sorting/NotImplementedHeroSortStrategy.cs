using Services.Contracts.Enums;
using Services.Contracts.Interfaces.Processing;
using Services.Contracts.Models;

namespace Services.Processing.Sorting;

/// <summary>
///     Заглушка стратегии сортировки до реализации конкретных стратегий.
/// </summary>
public sealed class NotImplementedHeroSortStrategy(SortType sortType) : IHeroSortStategy
{
    public SortType SortType { get; } = sortType;

    public IEnumerable<Hero> Sort(IEnumerable<Hero> heroes, bool descending) =>
        throw new NotImplementedException();
}
