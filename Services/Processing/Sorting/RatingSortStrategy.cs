using Services.Contracts.Enums;
using Services.Contracts.Interfaces.Processing;
using Services.Contracts.Models;
using Services.Processing.Sorting.Extensions;

namespace Services.Processing.Sorting;

public class RatingSortStrategy : IHeroSortStategy
{
    public SortType SortType => SortType.Rating;

    public IEnumerable<Hero> Sort(IEnumerable<Hero> heroes, bool descending)
    {
        return heroes.OrderByRating(descending);
    }
}