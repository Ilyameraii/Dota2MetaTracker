using Repository.Contracts.Cache;
using UseCases.Contracts.InputPorts;
using UseCases.Contracts.OutputPorts.ExternalData;

namespace UseCases;

/// <summary>
///     Сервис для управления данными персонажей: обновление, сохранение, загрузка
/// </summary>
public class UpdateHeroStatsUseCase(
    IStratzApiClient source,
    IHeroStatsCache cache) : IUpdateHeroStatsUseCase
{
    public async Task UpdateNewStatsAsync(CancellationToken ct = default)
    {
        var stats = await source.GetHeroStatsAsync(ct);
        if (stats.Count == 0)
            throw new InvalidOperationException("Источник вернул пустой список героев.");

        var names = await source.GetHeroNamesAsync(ct);
        cache.Update(stats, names, DateTime.UtcNow);
    }
}