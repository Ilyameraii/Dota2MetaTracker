using Services.Contracts.Models;

namespace Services.Contracts.Interfaces.Processing;

public interface IHeroCalculator
{
    public Hero Calculate(Hero hero, int totalMatchCount, Hero? previous = null);

    public IEnumerable<Hero> CalculateAll(
        IEnumerable<Hero> heroes,
        int totalMatchCount,
        IEnumerable<Hero>? previousHeroes = null);
}