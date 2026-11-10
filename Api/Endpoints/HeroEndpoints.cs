using Api.Contracts;
using Entities.Enums;
using Entities.Models;
using Images;
using Repository.Contracts.Cache;
using UseCases.Contracts.InputPorts;
using UseCases.Contracts.OutputPorts.ImageGenerating;

namespace Api.Endpoints;

/// <summary>
///     HTTP-эндпоинты для получения данных о героях
/// </summary>
public static class HeroEndpoints
{
    private const int DefaultLimit = 10;
    private const int MaxLimit = 200;
    private const int AvatarCacheSeconds = 86_400;

    private static readonly Dictionary<string, RankFlags> RankValues = new(StringComparer.OrdinalIgnoreCase)
    {
        ["un"] = RankFlags.Uncalibrated,
        ["hg"] = RankFlags.HeraldGuardian,
        ["ca"] = RankFlags.CrusaderArchon,
        ["la"] = RankFlags.LegendAncient,
        ["di"] = RankFlags.DivineImmortal
    };

    private static readonly Dictionary<string, RoleFlags> RoleValues = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Safelane"] = RoleFlags.Safelane,
        ["Midlane"] = RoleFlags.Midlane,
        ["Offlane"] = RoleFlags.Offlane,
        ["Support"] = RoleFlags.Support,
        ["Hard Support"] = RoleFlags.HardSupport,
        ["HardSupport"] = RoleFlags.HardSupport
    };

    private static readonly Dictionary<string, SortType> SortValues = new(StringComparer.OrdinalIgnoreCase)
    {
        ["wr"] = SortType.WinRate,
        ["pr"] = SortType.MatchCount,
        ["rating"] = SortType.Rating,
        ["dwr"] = SortType.WinrateDelta,
        ["dpr"] = SortType.PickrateDelta,
        ["drt"] = SortType.RatingDelta
    };

    /// <summary>
    ///     Регистрирует маршруты получения списка героев и иконки героя
    /// </summary>
    public static WebApplication MapHeroEndpoints(this WebApplication app)
    {
        app.MapGet(ApiRoutes.Heroes, GetHeroesAsync);
        app.MapGet(ApiRoutes.HeroAvatar, GetHeroAvatarAsync);
        app.MapGet(ApiRoutes.HeroWebAvatar, GetHeroWebAvatarAsync);

        return app;
    }

    private static async Task<IResult> GetHeroesAsync(
        string? rank,
        string? role,
        string? sort,
        string? dir,
        int? limit,
        IGetHeroMetaUseCase useCase,
        IHeroStatsCache cache,
        CancellationToken cancellationToken)
    {
        if (!TryParseRanks(rank, out var ranks, out var rankError))
            return Results.BadRequest(rankError);

        if (!TryParseRoles(role, out var roles, out var roleError))
            return Results.BadRequest(roleError);

        var sortBy = SortType.Rating;
        if (!string.IsNullOrWhiteSpace(sort) && !SortValues.TryGetValue(sort, out sortBy))
            return Results.BadRequest($"Неизвестное значение sort: '{sort}'.");

        var isDescending = true;
        if (!string.IsNullOrWhiteSpace(dir))
        {
            if (!string.Equals(dir, "asc", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase))
                return Results.BadRequest($"Неизвестное значение dir: '{dir}'.");

            isDescending = string.Equals(dir, "desc", StringComparison.OrdinalIgnoreCase);
        }

        var take = limit ?? DefaultLimit;
        if (take is < 1 or > MaxLimit)
            return Results.BadRequest($"Параметр limit должен быть в диапазоне 1..{MaxLimit}.");

        var options = new HeroProcessingOptions
        {
            Ranks = ranks,
            Roles = roles,
            SortBy = sortBy,
            IsDescending = isDescending
        };

        var heroes = await useCase.ExecuteAsync(options, cancellationToken);
        var response = new HeroesResponse(
            Total: heroes.Count,
            UpdatedAt: ToDateTimeOffset(cache.UpdateTime),
            Items: heroes.Take(take).ToList());

        return Results.Ok(response);
    }

    private static async Task<IResult> GetHeroAvatarAsync(
        int id,
        HttpContext httpContext,
        IHeroStatsCache cache,
        IHeroAvatarProvider avatarProvider,
        CancellationToken cancellationToken)
    {
        if (!cache.HeroesNames.ContainsKey(id))
            return Results.NotFound();

        var avatars = await avatarProvider.GetAvatarsAsync([id], cancellationToken);

        if (avatars is null
            || !avatars.TryGetValue(id, out var bytes)
            || bytes.Length == 0)
            return Results.NotFound();

        httpContext.Response.Headers.CacheControl = $"public,max-age={AvatarCacheSeconds}";

        return Results.File(bytes, "image/png");
    }

    private static async Task<IResult> GetHeroWebAvatarAsync(
        int id,
        HttpContext httpContext,
        IHeroWebAvatarProvider avatarProvider,
        CancellationToken cancellationToken)
    {
        var bytes = await avatarProvider.GetAvatarAsync(id, cancellationToken);

        if (bytes is null || bytes.Length == 0)
            return Results.NotFound();

        httpContext.Response.Headers.CacheControl = $"public,max-age={AvatarCacheSeconds}";

        return Results.File(bytes, "image/webp");
    }

    private static bool TryParseRanks(string? value, out RankFlags result, out string? error)
    {
        result = RankFlags.None;
        error = null;

        if (string.IsNullOrWhiteSpace(value))
            return true;

        foreach (var item in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (!RankValues.TryGetValue(item, out var rank))
            {
                error = $"Неизвестное значение rank: '{item}'.";
                return false;
            }

            result |= rank;
        }

        return true;
    }

    private static bool TryParseRoles(string? value, out RoleFlags result, out string? error)
    {
        result = RoleFlags.None;
        error = null;

        if (string.IsNullOrWhiteSpace(value))
            return true;

        foreach (var item in value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (!RoleValues.TryGetValue(item, out var role))
            {
                error = $"Неизвестное значение role: '{item}'.";
                return false;
            }

            result |= role;
        }

        return true;
    }

    private static DateTimeOffset ToDateTimeOffset(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => new DateTimeOffset(value),
            DateTimeKind.Local => new DateTimeOffset(value),
            _ => new DateTimeOffset(DateTime.SpecifyKind(value, DateTimeKind.Utc))
        };
    }
}
