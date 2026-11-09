using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Builders.Extensions;

namespace TelegramBot.Builders;

/// <summary>
/// Builder для одного ряда кнопок с индивидуальными стилями.
/// </summary>
public sealed class RowBuilder
{
    private readonly List<InlineKeyboardButton> buttons = [];

    /// <summary>
    /// Добавляет кнопку с опциональным стилем.
    /// </summary>
    public RowBuilder AddButton(
        string text,
        string callbackData)
    {
        buttons.Add(InlineKeyboardButton
            .WithCallbackData(text, callbackData));
        return this;
    }
    
    /// <summary>
    /// Добавляет кнопку с опциональным стилем.
    /// </summary>
    public RowBuilder AddButton(
        string text,
        string callbackData,
        KeyboardButtonStyle style)
    {
        buttons.Add(InlineKeyboardButton
            .WithCallbackData(text, callbackData)
            .WithStyle(style));
        return this;
    }

    internal List<InlineKeyboardButton> Build() => buttons;
}