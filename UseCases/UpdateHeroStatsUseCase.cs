using UseCases.Contracts.InputPorts;
using UseCases.Contracts.OutputPorts.Cache;
using UseCases.Contracts.OutputPorts.ExternalData;

namespace UseCases;

/// <summary>
///     Сервис для управления данными персонажей: обновление, сохранение, загрузка
/// </summary>
public class UpdateHeroStatsUseCase(
    IStratzApiService apiService,
    IStratzHeroStatsParser parser,
    IHeroStatsCache cache         
) : IUpdateHeroStatsUseCase
{
    public async Task UpdateNewStatsAsync(CancellationToken cancellationToken = default)
    {
        var statsRaw = await apiService.GetHeroesStats(cancellationToken);
        var stats = parser.ParseHeroStats(statsRaw);

        if (stats.Count == 0){
            throw new InvalidOperationException("Stratz API вернул пустой список героев.");
        }
        
        var namesRaw = await apiService.GetHeroesNames(cancellationToken);
        var names = parser.ParseHeroesNames(namesRaw);

        cache.Update(stats, names, DateTime.UtcNow);
    }
}