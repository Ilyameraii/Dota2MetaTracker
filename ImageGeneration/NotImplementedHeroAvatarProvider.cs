using UseCases.Contracts.OutputPorts.ImageGeneration;

namespace ImageGeneration;

/// <summary>
///     Заглушка провайдера аватаров до реализации загрузки изображений.
/// </summary>
public sealed class NotImplementedHeroAvatarProvider : IHeroAvatarProvider
{
    public Task<IReadOnlyDictionary<int, byte[]>?> GetAvatarsAsync(
        IEnumerable<int> select,
        CancellationToken cancellationToken) =>
        throw new NotImplementedException();
}
