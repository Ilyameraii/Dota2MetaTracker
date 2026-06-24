using Services.Contracts.Models;
using UseCases.Contracts.OutputPorts.Cache;

namespace ChatBot.Factories;

public class HeroStatsTextFactory(IHeroStatsCache heroStatsCache)
{
    public string Create(UserPreferences  preferences)
    {
        return string.Empty;
    }
}