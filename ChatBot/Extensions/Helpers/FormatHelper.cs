using System.Globalization;

namespace ChatBot.Extensions.Helpers;

public static class FormatHelper
{
    public static string FormatDelta(this double delta)
    {
        delta = Math.Round(delta * 100, 2);
        var sign = delta switch
        {
            < 0 => "-",
            >= 0 => "+",
            _ => string.Empty
        };
        return $"{sign}{Math.Abs(delta).ToString("F2", CultureInfo.InvariantCulture)}";
    }
}