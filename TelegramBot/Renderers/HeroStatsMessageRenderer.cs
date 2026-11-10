using TelegramBot.Factories;
using TelegramBot.Models;
using Entities.Models;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using UseCases.Contracts.InputPorts;

namespace TelegramBot.Renderers;

/// <summary>
/// Рендерер сообщения с меню для Telegram-бота.
/// </summary>
public class HeroStatsMessageRenderer(
    ITelegramBotClient bot,
    IGetHeroMetaUseCase useCase,
    HeroStatsKeyboardFactory keyboardFactory,
    HeroStatsTextFactory textFactory,
    HeroStatsImageFactory imageFactory,
    ILogger<HeroStatsMessageRenderer> logger
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
        List<Hero> heroes;

        try
        {
            heroes = await useCase.ExecuteAsync(prefs.ProcessingOptions, ct);
        }
        catch (Exception ex) when (!ct.IsCancellationRequested)
        {
            logger.LogError(ex, "Не удалось получить данные о героях из API.");

            await SendMessageAsync(
                chatId,
                "Не удалось получить данные о героях. Сервис временно недоступен, попробуйте позже.",
                ct);

            return;
        }

        var page = HeroMetaPage.Create(prefs, heroes);

        // Синхронизируем сохранённую страницу с фактически отображаемой:
        // список мог сократиться из-за фильтров или обновления статистики.
        prefs.PageNumber = page.PageIndex;

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
