using Services.Contracts.Enums;
using Services.Contracts.Interfaces.Processing;
using Services.Contracts.Models;
using Services.Processing.Sorting.Extensions;

namespace Services.Processing.Sorting;

public class MatchCountSortStrategy : IHeroSortStategy
{
    public SortType SortType => SortType.MatchCount;

    public IEnumerable<Hero> Sort(IEnumerable<Hero> heroes, bool descending)
    {
        return heroes.OrderByMatchCount(descending);
    }
}