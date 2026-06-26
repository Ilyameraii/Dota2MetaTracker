using ChatBot.Models;
using UseCases.Contracts.Models;
using UseCases.Contracts.OutputPorts.ImageGeneration;

namespace ChatBot.Factories;

public class HeroStatsImageFactory(
    IHeroTopImageGenerator imageGenerator,
    IHeroAvatarProvider avatarProvider)
{
    public async Task<Byte[]> Create(
        HeroMetaPage page,
        CancellationToken cancellationToken)
    {
        var avatars = await avatarProvider.GetAvatarsAsync(page.PageHeroes.Select(h => h.Id), cancellationToken);

        return imageGenerator.Generate(
            new HeroTopImageRequest
            {
                Heroes = page.PageHeroes,
                HeroAvatars = avatars,
                Title = $"ТОП-{page.End}",
                Options = page.Preferences.ProcessingOptions,
                RankOffset = page.Start,
            }
        );
    }
}