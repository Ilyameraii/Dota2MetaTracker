using UseCases.Contracts.Models;
using UseCases.Contracts.OutputPorts.ImageGeneration;

namespace ImageGeneration;

public sealed class HeroTopImageGenerator : IHeroTopImageGenerator
{
    public byte[] Generate(HeroTopImageRequest request) =>
        throw new NotImplementedException();
}
