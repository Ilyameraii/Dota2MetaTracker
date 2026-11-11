using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Contracts.Repositories;

namespace Context.Repositories;

/// <inheritdoc/>
public class MetaUpdateRepository(MetaTrackerDbContext context) : IMetaUpdateRepository
{
    /// <inheritdoc/>
    public Task<MetaUpdate?> GetLatestAsync(CancellationToken cancellationToken = default) =>
        context.MetaUpdates
            .OrderByDescending(metaUpdate => metaUpdate.DateTime)
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc/>
    public async Task<IReadOnlyList<MetaUpdate>> GetLatestAsync(
        int count,
        CancellationToken cancellationToken = default) =>
        await context.MetaUpdates
            .OrderByDescending(metaUpdate => metaUpdate.DateTime)
            .Take(count)
            .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public Task<MetaUpdate?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        context.MetaUpdates
            .FirstOrDefaultAsync(metaUpdate => metaUpdate.Id == id, cancellationToken);

    /// <inheritdoc/>
    public Task<MetaUpdate?> GetEarliestAsync(CancellationToken cancellationToken = default) =>
        context.MetaUpdates
            .OrderBy(metaUpdate => metaUpdate.DateTime)
            .FirstOrDefaultAsync(cancellationToken);

    /// <inheritdoc/>
    public Task AddAsync(MetaUpdate metaUpdate, CancellationToken cancellationToken = default) =>
        context.MetaUpdates.AddAsync(metaUpdate, cancellationToken).AsTask();

    /// <inheritdoc/>
    public async Task<IReadOnlyList<MetaUpdate>> GetOlderThanAsync(
        DateTime cutoff,
        CancellationToken cancellationToken = default) =>
        await context.MetaUpdates
            .Where(metaUpdate => metaUpdate.DateTime <= cutoff)
            .OrderByDescending(metaUpdate => metaUpdate.DateTime)
            .ToListAsync(cancellationToken);

    /// <inheritdoc/>
    public void RemoveRange(IEnumerable<MetaUpdate> metaUpdates) =>
        context.MetaUpdates.RemoveRange(metaUpdates);
}
