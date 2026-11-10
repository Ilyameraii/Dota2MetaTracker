namespace Api.Contracts;

/// <summary>
///     Маршруты HTTP API
/// </summary>
public static class ApiRoutes
{
    /// <summary>
    ///     Список героев с фильтрацией и сортировкой
    /// </summary>
    public const string Heroes = "/api/heroes";

    /// <summary>
    ///     Иконка героя по идентификатору
    /// </summary>
    public const string HeroAvatar = "/api/heroes/{id:int}/avatar";
}
