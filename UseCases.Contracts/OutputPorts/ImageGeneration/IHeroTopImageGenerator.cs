namespace UseCases.Contracts.OutputPorts.Formatting;

public interface IHeroTopImageGenerator
{
    byte[] Generate(HeroTopImageRequest request);
}
