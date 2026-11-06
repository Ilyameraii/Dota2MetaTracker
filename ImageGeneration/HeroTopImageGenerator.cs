using UseCases.Contracts.Formatting;

namespace ImageGeneration;

public sealed class HeroTopImageGenerator : IHeroTopImageGenerator
{
    private readonly BackgroundRenderer background = new();
    private readonly HeaderRenderer header = new();
    private readonly HeroRowRenderer row = new();
    private readonly FooterRenderer footer = new();

    public byte[] Generate(HeroTopImageRequest request)
    {
        var height = HeroTopImageLayout.HeaderHeight
                     + request.Heroes.Count * HeroTopImageLayout.RowHeight
                     + HeroTopImageLayout.FooterHeight;

        using var surface = SKSurface.Create(
            new SKImageInfo(HeroTopImageLayout.Width, height, SKColorType.Rgba8888, SKAlphaType.Premul));
        var canvas = surface.Canvas;

        background.Draw(canvas, HeroTopImageLayout.Width, height);
        header.Draw(canvas, request.Title, request.Options);

        for (var i = 0; i < request.Heroes.Count; i++)
            row.Draw(canvas, request.Heroes[i], i, request.HeroAvatars, request.RankOffset);

        footer.Draw(canvas, height, request.Options);

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Png, 95);
        return data.ToArray();
    }
}