using Repository.Contracts.Cache;
using Services.Contracts.Interfaces.Processing;
using Services.Contracts.Models;
using UseCases.Contracts.InputPorts;

namespace UseCases;

public class GetHeroMetaUseCase(
    IHeroStatsCache cache,
    IHeroStatsProcessor processor): IGetHeroMetaUseCase
{
    public List<Hero> Execute(HeroProcessingOptions options)
    {
        return processor.GetProcessedHeroStats(
            cache.NewHeroesStats, 
            cache.OldHeroesStats, 
            cache.HeroesNames, 
            options);
    }
}