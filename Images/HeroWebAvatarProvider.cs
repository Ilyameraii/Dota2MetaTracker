using System.Collections.Concurrent;
using SkiaSharp;
using UseCases.Contracts.OutputPorts.ImageGenerating;

namespace Images;

/// <summary>
///     Адаптер: из полноразмерного PNG героя готовит компактный WebP для сайта
/// </summary>
public class HeroWebAvatarProvider(IHeroAvatarProvider avatarProvider) : IHeroWebAvatarProvider
{
    private const int Width = 132;
    private const int Height = 76;
    private const int WebpQuality = 80;

    private readonly ConcurrentDictionary<int, byte[]> webpCache = new();

    public async Task<byte[]?> GetAvatarAsync(int heroId, CancellationToken cancellationToken)
    {
        if (webpCache.TryGetValue(heroId, out var cached))
            return cached;

        cancellationToken.ThrowIfCancellationRequested();

        var avatars = await avatarProvider.GetAvatarsAsync([heroId], cancellationToken);

        if (avatars is null
            || !avatars.TryGetValue(heroId, out var original)
            || original.Length == 0)
            return null;

        var webp = ToWebp(original);

        // Неудачное кодирование не кэшируем: следующий запрос повторит попытку.
        if (webp is null || webp.Length == 0)
            return null;

        webpCache[heroId] = webp;

        return webp;
    }

    private static byte[]? ToWebp(byte[] source)
    {
        using var bitmap = SKBitmap.Decode(source);

        if (bitmap is null)
            return null;

        using var resized = new SKBitmap(new SKImageInfo(Width, Height, SKColorType.Rgba8888, SKAlphaType.Premul));

        using (var canvas = new SKCanvas(resized))
        {
            using var paint = new SKPaint
            {
                IsAntialias = true,
                FilterQuality = SKFilterQuality.High
            };

            canvas.DrawBitmap(bitmap, GetCropRect(bitmap), new SKRect(0, 0, Width, Height), paint);
        }

        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Webp, WebpQuality);

        return data?.ToArray();
    }

    /// <summary>
    ///     Центральный кроп под соотношение сторон целевого аватара
    /// </summary>
    private static SKRect GetCropRect(SKBitmap bitmap)
    {
        var targetAspect = (float)Width / Height;
        var sourceAspect = (float)bitmap.Width / bitmap.Height;

        if (sourceAspect > targetAspect)
        {
            var cropWidth = bitmap.Height * targetAspect;
            var left = (bitmap.Width - cropWidth) / 2f;

            return new SKRect(left, 0, left + cropWidth, bitmap.Height);
        }

        var cropHeight = bitmap.Width / targetAspect;
        var top = (bitmap.Height - cropHeight) / 2f;

        return new SKRect(0, top, bitmap.Width, top + cropHeight);
    }
}
