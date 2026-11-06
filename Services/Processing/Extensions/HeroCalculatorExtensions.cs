using Services.Contracts.Models;

namespace Services.Processing.Extensions;

public static class HeroCalculatorExtensions
{
    public static Hero WithWinrate(this Hero hero)
    {
        var winRate = hero.MatchCount > 0
            ? (double)hero.WinCount / hero.MatchCount
            : 0;

        return hero with { WinRate = winRate };
    }

    public static Hero WithPickRate(this Hero hero, int totalMatchCount)
    {
        var pickRate = totalMatchCount > 0
            ? (double)hero.MatchCount / totalMatchCount
            : 0;

        return hero with { PickRate = pickRate };
    }

    public static Hero WithRating(this Hero hero)
    {
        var rating = Math.Pow(hero.WinRate, 2) * hero.PickRate;

        return hero with { Rating = rating };
    }

    public static Hero WithDeltas(this Hero hero, Hero previous) =>
        hero with
        {
            WinRateDelta = CalculateWinrateDelta(hero, previous),
            PickRateDelta = CalculatePickrateDelta(hero, previous),
            RatingDelta = CalculateRatingDelta(hero, previous)
        };

    private static double CalculateWinrateDelta(Hero hero, Hero previous) => hero.WinRate - previous.WinRate;

    private static double CalculatePickrateDelta(Hero hero, Hero previous) => hero.PickRate - previous.PickRate;

    private static double CalculateRatingDelta(Hero hero, Hero previous) => hero.Rating - previous.Rating;
}