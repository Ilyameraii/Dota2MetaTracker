namespace Images;

/// <summary>
///     Компактный WebP-аватар героя для сайта
/// </summary>
public interface IHeroWebAvatarProvider
{
    Task<byte[]?> GetAvatarAsync(int heroId, CancellationToken cancellationToken);
}
