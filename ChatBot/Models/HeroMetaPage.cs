using ChatBot.Constants;
using Services.Contracts.Models;

namespace ChatBot.Models;

public class HeroMetaPage
{
    public required UserPreferences Preferences { get; init; }
    public required List<Hero> PageHeroes { get; init; }
    public required int PageIndex { get; init; }
    public required int TotalPages { get; init; }
    public required int Start { get; init; }
    public required int End { get; init; }

    public static HeroMetaPage Create(UserPreferences prefs, List<Hero> heroes)
    {
        var totalPages = (heroes.Count - 1) / ValueConstants.HeroesPerPage;
        var pageIndex = Math.Min(prefs.PageNumber, totalPages);
        var start = pageIndex * ValueConstants.HeroesPerPage;
        var end = Math.Min(start + ValueConstants.HeroesPerPage, heroes.Count);

        return new HeroMetaPage
        {
            Preferences = prefs,
            PageHeroes = heroes.GetRange(start, end - start),
            PageIndex = pageIndex,
            TotalPages = totalPages,
            Start = start,
            End = end
        };
    }
}