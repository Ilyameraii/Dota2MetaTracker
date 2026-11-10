using System.Collections.Concurrent;
using UseCases.Contracts.OutputPorts.ImageGenerating;

namespace TelegramBot.Api;

/// <summary>
///     Получение аватаров героев из HTTP API с кэшем в памяти
/// </summary>
public class ApiHeroAvatarProvider(HttpClient http) : IHeroAvatarProvider
{
    private readonly ConcurrentDictionary<int, byte[]> avatarCache = new();

    /// <inheritdoc/>
    public async Task<IReadOnlyDictionary<int, byte[]>?> GetAvatarsAsync(
        IEnumerable<int> select,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(select);

        var heroIds = select.Distinct().ToList();
        var toLoad = heroIds
            .Where(id => !avatarCache.ContainsKey(id))
            .ToList();

        await Parallel.ForEachAsync(
            toLoad,
            cancellationToken,
            async (id, token) =>
            {
                try
                {
                    var bytes = await http.GetByteArrayAsync($"api/heroes/{id}/avatar", token);

                    if (bytes.Length > 0)
                        avatarCache[id] = bytes;
                }
                catch (Exception exception) when (
                    exception is HttpRequestException or TaskCanceledException
                    && !token.IsCancellationRequested)
                {
                    // Ошибку не кэшируем: при следующем запросе будет новая попытка.
                }
            });

        var result = new Dictionary<int, byte[]>(heroIds.Count);

        foreach (var id in heroIds)
            if (avatarCache.TryGetValue(id, out var bytes))
                result[id] = bytes;

        return result;
    }
}
