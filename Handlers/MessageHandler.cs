using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace RNMSO_Library_Bot.Handlers;

internal static partial class Handlers
{
    internal static async Task HandleMessageAsync(TelegramBotClient bot, Message message, UpdateType type)
    {
        if (message.Text is "/start")
        {
            await bot.SendMessage(message.Chat, "Добро пожаловать в чат-бот библиотеки Российского национального молодёжного симфонического оркестра!\n\n" +
                "Для получения доступа к библиотеке Вам необходимо предоставить номер телефона, привязанный к вашему аккаунту в Telegram.",
                replyMarkup: new KeyboardButton[]
                {
                    KeyboardButton.WithRequestContact("✉️ Предоставить данные")
                });

            await bot.DeleteMessage(message.Chat, message.Id);
        }
        else if (message.Contact != null)
        {
            await bot.SendMessage(message.Chat, "Спасибо! Верификация прошла успешно, вам предоставлен доступ к библиотеке.",
                replyMarkup: new ReplyKeyboardRemove());

            await bot.DeleteMessage(message.Chat, message.Id);
        }
        else
        {
            await bot.DeleteMessage(message.Chat, message.Id);
        }
    }
}
