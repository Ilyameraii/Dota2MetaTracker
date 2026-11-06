using UseCases.Contracts.Models;

namespace UseCases.Contracts.OutputPorts.ImageGeneration;

public interface IHeroTopImageGenerator
{
    byte[] Generate(HeroTopImageRequest request);
}
