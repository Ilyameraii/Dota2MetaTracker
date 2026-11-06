using SkiaSharp;

namespace ImageGeneration;

 internal sealed class StatRenderer
{
    public void Draw(SKCanvas canvas, float x, float centerY,
        string value, string label, double delta,
        SKColor valueColor, SKColor labelColor)
    {
        using var valuePaint = TextPaint(valueColor, 24, bold: true);
        using var labelPaint = TextPaint(labelColor, 14, bold: false);

        canvas.DrawText(value, x, centerY + 4, valuePaint);
        canvas.DrawText($" {label}", x + valuePaint.MeasureText(value), centerY + 4, labelPaint);

        DrawDelta(canvas, delta, x, centerY + 22, 13);
    }
}