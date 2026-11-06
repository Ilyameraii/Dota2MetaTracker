using UseCases.Contracts.OutputPorts.ImageGeneration;

namespace ImageGenerating;

public class HeroAvatarProvider() : IHeroAvatarProvider
{
    public Task<IReadOnlyDictionary<int, byte[]>?> GetAvatarsAsync(IEnumerable<int> select, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}