using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts.Repositories;

namespace Context;

/// <summary>
///     Контекст базы данных для статистики персонажей и обновлений
/// </summary>
public class MetaTrackerDbContext(
    DbContextOptions<MetaTrackerDbContext> options)
    : DbContext(options), IUnitOfWork
{
    /// <summary>
    ///     Статистика персонажей
    /// </summary>
    public DbSet<HeroStat> HeroStats => Set<HeroStat>();

    /// <summary>
    ///     Обновления статистики
    /// </summary>
    public DbSet<MetaUpdate> MetaUpdates => Set<MetaUpdate>();

    /// <inheritdoc/>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        base.SaveChangesAsync(cancellationToken);

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MetaTrackerDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
