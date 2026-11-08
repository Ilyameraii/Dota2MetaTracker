using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UseCases.Contracts.InputPorts;

namespace Dota2MetaTracker.BackgroundServices;

/// <summary>
/// Фоновая служба периодического обновления статистики героев из STRATZ.
/// </summary>
public sealed class HeroStatsUpdateBackgroundService(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration,
    ILogger<HeroStatsUpdateBackgroundService> logger) : BackgroundService
{
    private readonly TimeSpan interval =
        configuration.GetValue("HeroStats:UpdateInterval", TimeSpan.FromHours(1));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation(
            "Служба обновления статистики героев запущена. Интервал: {Interval}.", interval);

        try
        {
            // Первое обновление сразу при старте, чтобы кэш был заполнен.
            await UpdateAsync(stoppingToken);

            using var timer = new PeriodicTimer(interval);

            while (await timer.WaitForNextTickAsync(stoppingToken))
                await UpdateAsync(stoppingToken);
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("Служба обновления статистики героев остановлена.");
        }
    }

    private async Task UpdateAsync(CancellationToken ct)
    {
        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var useCase = scope.ServiceProvider.GetRequiredService<IUpdateHeroStatsUseCase>();

            await useCase.UpdateNewStatsAsync(ct);

            logger.LogInformation("Статистика героев успешно обновлена в {Time}.", DateTime.UtcNow);
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            // Не роняем хост: следующая попытка через заданный интервал.
            logger.LogError(ex, "Ошибка обновления статистики героев. Повтор через {Interval}.", interval);
        }
    }
}
