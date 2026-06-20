namespace UseCases.Contracts.Formatting;

public interface IHeroTopImageGenerator
{
    byte[] Generate(HeroTopImageRequest request);
}
