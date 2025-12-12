using Telegram.Bot;
using Telegram.Bot.Polling;

namespace RNMSO_Library_Bot.Handlers;

/// <summary>
/// <inheritdoc cref="RNMSO_Library_Bot.Handlers.Handlers"/>
/// </summary>
public static partial class Handlers
{
    public static async Task HandleErrorAsync(TelegramBotClient bot, Exception exception, HandleErrorSource errorSource)
    {
        Console.WriteLine(exception);
    }
}