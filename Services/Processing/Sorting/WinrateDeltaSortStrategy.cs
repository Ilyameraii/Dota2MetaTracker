using Services.Contracts.Enums;
using Services.Contracts.Interfaces.Processing;
using Services.Contracts.Models;
using Services.Processing.Sorting.Extensions;

namespace Services.Processing.Sorting;

public class WinrateDeltaSortStrategy : IHeroSortStategy
{
    public SortType SortType => SortType.WinrateDelta;

    public IEnumerable<Hero> Sort(IEnumerable<Hero> heroes, bool descending)
    {
        return heroes.OrderByWinrateDelta(descending);
    }
}