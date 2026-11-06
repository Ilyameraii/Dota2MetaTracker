using Services.Contracts.Enums;
using Services.Contracts.Interfaces.Processing;
using Services.Contracts.Models;
using Services.Processing.Sorting.Extensions;

namespace Services.Processing.Sorting;

public class PickrateDeltaSortStrategy : IHeroSortStategy
{
    public SortType SortType => SortType.PickrateDelta;

    public IEnumerable<Hero> Sort(IEnumerable<Hero> heroes, bool descending)
    {
        return heroes.OrderByPickrateDelta(descending);
    }
}