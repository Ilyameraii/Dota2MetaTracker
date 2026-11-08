using Entities.Models;

namespace UseCases.Contracts.Models;

public sealed record HeroStatsImageRequest
{
    public required IReadOnlyList<Hero> Heroes { get; init; }

    public required string Title { get; init; }

    public HeroProcessingOptions? Options { get; init; }

    public int RankOffset { get; init; }

    public IReadOnlyDictionary<int, byte[]>? HeroAvatars { get; init; }
}