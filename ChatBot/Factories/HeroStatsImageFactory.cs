using Services.Contracts.Models;
using UseCases.Contracts.OutputPorts.Cache;

namespace ChatBot.Factories;

public class HeroStatsImageFactory(
    IHeroStatsCache heroStatsCache,
    IImageGenerator imageGenerator,
    IHeroAvatarProvider avatarProvide)
{
    public async Task<Byte[]> Create( UserPreferences prefs)
    {
        return new byte[1];
    }
}