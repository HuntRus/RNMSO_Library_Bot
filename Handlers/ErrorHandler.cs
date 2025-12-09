using Telegram.Bot;
using Telegram.Bot.Polling;

namespace RNMSO_Library_Bot.Handlers;

internal static partial class Handlers
{
    internal static async Task HandleErrorAsync(TelegramBotClient bot, Exception exception, HandleErrorSource errorSource)
    {
        Console.WriteLine(exception);
    }
}