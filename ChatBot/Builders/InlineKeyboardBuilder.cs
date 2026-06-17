using ChatBot.Extensions;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatBot.Builders;

/// <summary>
/// Builder для построения inline-клавиатуры Telegram-бота.
/// </summary>
public class InlineKeyboardBuilder
{
    private readonly List<List<InlineKeyboardButton>> rows = new();

    /// <summary>
    /// Добавляет строку с одной кнопкой.
    /// </summary>
    public InlineKeyboardBuilder AddButton(string text, string callbackData)
    {
        rows.Add([InlineKeyboardButton.WithCallbackData(text, callbackData)]);
        return this;
    }

    /// <summary>
    /// Добавляет строку с несколькими кнопками.
    /// </summary>
    public InlineKeyboardBuilder AddRow(params (string text, string callbackData)[] buttons)
    {
        rows.Add(buttons.Select(b => InlineKeyboardButton.WithCallbackData(b.text, b.callbackData)).ToList());
        return this;
    }

    // <summary>
    /// Применяет стиль ко всем кнопкам последнего добавленного ряда.
    /// </summary>
    public InlineKeyboardBuilder WithStyle(KeyboardButtonStyle style)
    {
        if (rows.Count == 0)
            return this;

        rows[^1] = rows[^1].WithStyle(style).ToList();
        return this;
    }

    /// <summary>
    /// Строит клавиатуру.
    /// </summary>
    public InlineKeyboardMarkup Build() => new(rows);
}