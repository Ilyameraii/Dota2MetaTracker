using ChatBot.Contracts;
using ChatBot.Renderers;
using Repository.Contracts;
using Telegram.Bot.Types;

namespace ChatBot.CommandHandlers;

public class StartCommandHandler(HeroStatsMessageRenderer renderer, IUserPreferencesCache cache) : ICommandHandler
{
    public string Command => "/start";

    public async Task HandleAsync(Message message, CancellationToken сancellationToken)
    {
        await renderer.RenderAsync(chatId: message.Chat.Id, messageId: message.Id, prefs: cache.GetOrCreate(message.Chat.Id), ct: сancellationToken);
    }
}