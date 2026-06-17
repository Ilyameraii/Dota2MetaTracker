using Telegram.Bot.Types.ReplyMarkups;

namespace ChatBot.Extensions;

/// <summary>
/// Методы расширения для <see cref="InlineKeyboardButton"/>.
/// </summary>
public static class InlineKeyboardButtonExtensions
{
    /// <summary>
    /// Применяет стиль ко всем кнопкам коллекции.
    /// </summary>
    public static IEnumerable<InlineKeyboardButton> WithStyle(
        this IEnumerable<InlineKeyboardButton> buttons,
        KeyboardButtonStyle style)
    {
        foreach (var button in buttons)
        {
            button.Style = style;
            yield return button;
        }
    }
}