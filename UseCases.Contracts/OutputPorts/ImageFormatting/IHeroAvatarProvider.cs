namespace UseCases.Contracts.OutputPorts.ImageGeneration;

public interface IHeroAvatarProvider
{
    Task<IReadOnlyDictionary<int, byte[]>?> GetAvatarsAsync(IEnumerable<int> select, CancellationToken cancellationToken);
}