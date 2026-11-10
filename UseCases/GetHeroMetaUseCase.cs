using Repository.Contracts.Cache;
using Services.Contracts.Interfaces.Processing;
using Entities.Models;
using UseCases.Contracts.InputPorts;

namespace UseCases;

public class GetHeroMetaUseCase(
    IHeroStatsCache cache,
    IHeroStatsProcessor processor): IGetHeroMetaUseCase
{
    public Task<List<Hero>> ExecuteAsync(
        HeroProcessingOptions options,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.FromResult(processor.GetProcessedHeroStats(
            cache.NewHeroesStats,
            cache.OldHeroesStats,
            cache.HeroesNames,
            options));
    }
}
