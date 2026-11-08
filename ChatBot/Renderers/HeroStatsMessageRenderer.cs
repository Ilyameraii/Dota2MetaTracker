using ChatBot.Factories;
using ChatBot.Models;
using Entities.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using UseCases.Contracts.InputPorts;

namespace ChatBot.Renderers;

/// <summary>
/// Рендерер сообщения с меню для Telegram-бота.
/// </summary>
public class HeroStatsMessageRenderer(
    ITelegramBotClient bot,
    IGetHeroMetaUseCase useCase,
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
        var heroes = useCase.Execute(prefs.ProcessingOptions);
        var page = HeroMetaPage.Create(prefs, heroes);
        
        currentKeyboard = keyboardFactory.Create(page);

        if (prefs.IsImageFormat)
        {
            currentImage = await imageFactory.Create(page, ct);
        }
        else
        {
            currentText = textFactory.Create(page);
        }


        await base.RenderAsync(chatId, messageId, ct);
    }

    protected override string? BuildText() => currentText;

    protected override InlineKeyboardMarkup? BuildKeyboard() => currentKeyboard;

    protected override byte[]? BuildImage() => currentImage;
}