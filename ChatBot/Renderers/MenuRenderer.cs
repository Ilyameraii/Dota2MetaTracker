using ChatBot.Builders;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace ChatBot.Renderers;

/// <summary>
/// Рендерер сообщения с меню для Telegram-бота.
/// </summary>
public class MenuRenderer(ITelegramBotClient bot) : BaseRenderer(bot)
{
    /// <inheritdoc/>
    protected override string Text => "🏋️ Выберите действие: ";

    /// <inheritdoc/>
    protected override InlineKeyboardMarkup BuildKeyboard() => new InlineKeyboardBuilder()
        .AddRow(row => row
            .AddButton("✅", "accept")
            .AddButton("❌", "decline", KeyboardButtonStyle.Danger))
        .Build();
}