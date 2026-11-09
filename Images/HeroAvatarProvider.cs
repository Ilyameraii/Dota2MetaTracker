using System.Collections.Concurrent;
using Repository.Contracts.Cache;
using UseCases.Contracts.OutputPorts.ImageGenerating;

namespace Images;

public class HeroAvatarProvider(HttpClient http, IHeroStatsCache cache) : IHeroAvatarProvider
{
    private readonly ConcurrentDictionary<int, byte[]> avatarCache = new();

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
                var name = cache.HeroesNames.GetValueOrDefault(id) ?? string.Empty;
                var urlName = name.ToLowerInvariant().Replace(' ', '_').Replace("-", "");
                var url = $"https://cdn.cloudflare.steamstatic.com/apps/dota2/images/dota_react/heroes/{urlName}.png";

                var bytes = ImageResourceProvider.GetHeroIcon(urlName);

                if (bytes is null || bytes.Length == 0)
                {
                    try
                    {
                        bytes = await http.GetByteArrayAsync(url, token);
                    }
                    catch (Exception exception) when (
                        exception is HttpRequestException or TaskCanceledException
                        && !token.IsCancellationRequested)
                    {
                        bytes = [];
                    }
                }

                avatarCache[id] = bytes ?? [];
            });

        var result = new Dictionary<int, byte[]>(heroIds.Count);

        foreach (var id in heroIds)
        {
            if (avatarCache.TryGetValue(id, out var bytes))
            {
                result[id] = bytes;
            }
        }

        return result;
    }
}
