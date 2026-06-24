using ChatBot.Constants;
using ChatBot.Factories;
using Services.Contracts.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using UseCases.Contracts.OutputPorts.Cache;

namespace ChatBot.Renderers;

/// <summary>
/// Рендерер сообщения с меню для Telegram-бота.
/// </summary>
public class HeroStatsMessageRenderer(
    ITelegramBotClient bot,
    HeroStatsKeyboardFactory keyboardFactory,
    HeroStatsTextFactory textFactory,
    HeroStatsImageFactory imageFactory
)
    : BaseMessageRenderer(bot)
{
    private InlineKeyboardMarkup? currentKeyboard;
    private byte[]? currentImage;
    private string? currentText;

    /// <summary>
    /// Подготавливает данные и делегирует рендеринг базовому классу.
    /// </summary>
    public async Task RenderAsync(
        long chatId,
        int? messageId,
        UserPreferences prefs,
        CancellationToken ct = default)
    {
        
        currentKeyboard = keyboardFactory.Create(prefs);

        if (prefs.IsImageFormat)
        {
            currentImage = await imageFactory.Create(prefs);
        }
        else
        {
            currentText = textFactory.Create(prefs);
        }


        await base.RenderAsync(chatId, messageId, ct);
    }

    protected override string? BuildText() => currentText;

    protected override InlineKeyboardMarkup? BuildKeyboard() => currentKeyboard;

    protected override byte[]? BuildImage() => currentImage;
}