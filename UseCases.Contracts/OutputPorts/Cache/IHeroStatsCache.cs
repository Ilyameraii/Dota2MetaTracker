using Entities.Models;

namespace UseCases.Contracts.OutputPorts.Cache;

/// <summary>
///     Кэш для хранения данных о персонажах и времени последнего обновления
/// </summary>  
public interface IHeroStatsCache
{
    /// <summary>
    ///     Новая статистика персонажей
    /// </summary>
    IReadOnlyList<HeroStat> NewHeroesStats { get; }

    /// <summary>
    ///     Старая статистика персонажей
    /// </summary>
    public IReadOnlyList<HeroStat> OldHeroesStats { get; }

    /// <summary>
    ///     Словарь соответствия идентификаторов и имен персонажей
    /// </summary>
    IReadOnlyDictionary<int, string> HeroesNames { get; }
    
    /// <summary>
    ///     Время последнего обновления
    /// </summary>
    DateTime UpdateTime { get; }
    
    /// <summary>
    ///     Количество персонажей в статистике
    /// </summary>
    int HeroCount { get; }
    
    /// <summary>
    ///     Проверка загрузки данных
    /// </summary>
    bool IsLoaded { get; }

    /// <summary>
    ///     Обновление статистики
    /// </summary>
    /// <param name="stats">Новая статистика персонажей</param>
    /// <param name="names">Имена персонажей</param>
    /// <param name="updatedAt">Время обновления</param>
    void Update(IReadOnlyList<HeroStat> stats, IReadOnlyDictionary<int, string> names, DateTime updatedAt);
}