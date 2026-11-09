using TelegramBot.Constants;
using Entities.Models;

namespace TelegramBot.Extensions.Helpers;

public static class PaginationHelper
{
    public static (int start, int end) GetPageRange(
        this UserPreferences prefs,
        int totalCount)
    {
        var totalPages = (totalCount - 1) / ValueConstants.HeroesPerPage;
        var pageIndex = Math.Min(prefs.PageNumber, totalPages);
        var start = pageIndex * ValueConstants.HeroesPerPage;
        var end = Math.Min(start + ValueConstants.HeroesPerPage, totalCount);

        return (start, end);
    }
}