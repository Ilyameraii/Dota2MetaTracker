using Entities.Models;

namespace UseCases.Contracts.InputPorts;

public interface IGetHeroMetaUseCase
{
      Task<List<Hero>> ExecuteAsync(HeroProcessingOptions options, CancellationToken cancellationToken = default);
}
