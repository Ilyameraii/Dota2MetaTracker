using UseCases.Contracts.Models;
using UseCases.Contracts.OutputPorts.ImageGenerating;
using UseCases.Contracts.OutputPorts.ImageGeneration;

namespace ImageGenerating;

public sealed class HeroStatsImageGenerator(
    IHeroAvatarProvider avatarProvider)
    : IHeroStatsImageGenerator
{
    public async Task<byte[]> GenerateAsync(
        HeroStatsImageRequest request,
        CancellationToken cancellationToken)
    {
        var avatars = await avatarProvider.GetAvatarsAsync(
            request.Heroes.Select(h => h.Id),
            cancellationToken);

        // Здесь будет реализация отрисовки через SkiaSharp.

        throw new NotImplementedException();
    }
}