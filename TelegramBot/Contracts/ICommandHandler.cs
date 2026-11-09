using Telegram.Bot.Types;

namespace TelegramBot.Contracts;

/// <summary>
/// Обработчик команды Telegram-бота.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Команда, которую обрабатывает данный обработчик.
    /// </summary>
    string Command { get; }

    /// <summary>
    /// Обрабатывает входящее сообщение с командой.
    /// </summary>
    Task HandleAsync(Message message, CancellationToken сancellationToken);
}