using ChatBot.Contracts;
using ChatBot.Renderers;
using Telegram.Bot.Types;

namespace ChatBot.CommandHandlers;

public class StartCommandHandler(BaseMessageRenderer renderer) : ICommandHandler
{
    public string Command => "/start";

    public async Task HandleAsync(Message message, CancellationToken сancellationToken)
    {
        var few = string.Empty;
        await renderer.RenderAsync(chatId: message.Chat.Id, cancellationToken: сancellationToken);
    }
}