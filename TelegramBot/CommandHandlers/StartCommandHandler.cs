using TelegramBot.Contracts;
using TelegramBot.Renderers;
using Repository.Contracts;
using Telegram.Bot.Types;

namespace TelegramBot.CommandHandlers;

public class StartCommandHandler(HeroStatsMessageRenderer renderer, IUserPreferencesCache cache) : ICommandHandler
{
    public string Command => "/start";

    public async Task HandleAsync(Message message, CancellationToken сancellationToken)
    {
        await renderer.RenderAsync(chatId: message.Chat.Id, messageId: null, prefs: cache.GetOrCreate(message.Chat.Id), ct: сancellationToken);
    }
}