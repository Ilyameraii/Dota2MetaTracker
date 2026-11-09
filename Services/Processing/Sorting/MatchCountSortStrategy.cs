using Entities.Enums;
using Services.Contracts.Interfaces.Processing;
using Entities.Models;
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