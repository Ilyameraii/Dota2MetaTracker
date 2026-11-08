using Entities.Models;

namespace UseCases.Contracts.InputPorts;

public interface IGetHeroMetaUseCase
{
      List<Hero> Execute(HeroProcessingOptions options);
}