using Services.Contracts.Enums;
using Services.Contracts.Models;

namespace Services.Contracts.Interfaces.Processing;

public interface IHeroSortStategy
{
    SortType SortType { get; }
    IEnumerable<Hero> Sort(IEnumerable<Hero> heroes, bool descending);
}