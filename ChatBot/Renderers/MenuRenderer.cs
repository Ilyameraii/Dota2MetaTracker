using ChatBot.Builders;
using ChatBot.Constants;
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
            .Add("✅", "accept")
            .Add("❌", "decline", KeyboardButtonStyle.Danger))
        .Build();
}