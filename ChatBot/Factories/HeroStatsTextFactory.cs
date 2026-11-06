using ChatBot.Formatters;
using ChatBot.Models;
using Services.Contracts.Models;
namespace ChatBot.Factories;

public class HeroStatsTextFactory(
    HeroTextFormatter heroFormatter)
{
    public string Create(HeroMetaPage page)
    {
        var lines = Enumerable.Range(0, page.PageHeroes.Count)
            .Select(i => $"{page.Start + i + 1}. {heroFormatter.Format(page.PageHeroes[i])}");

        return string.Join("\n\n", lines);
    }
}