namespace UseCases.Contracts.OutputPorts.ImageGenerating;

public interface IHeroAvatarProvider
{
    Task<IReadOnlyDictionary<int, byte[]>?> GetAvatarsAsync(IEnumerable<int> select, CancellationToken cancellationToken);
}