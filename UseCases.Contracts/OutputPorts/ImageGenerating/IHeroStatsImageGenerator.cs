using UseCases.Contracts.Models;

namespace UseCases.Contracts.OutputPorts.ImageGenerating;

public interface IHeroStatsImageGenerator
{
    Task<byte[]> GenerateAsync(
        HeroStatsImageRequest request,
        CancellationToken cancellationToken);
}