using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace RNMSO_Library_Bot.Handlers;

internal static partial class Handlers
{
    internal static async Task HandleMessageAsync(TelegramBotClient bot, Message message, UpdateType type)
    {
        if (User.IsExists(message.From!.Id))
        {
            /// menu comes next...
        }
        else if (message.Contact != null)
        {
            if (Access.IsWhitelisted(message.Contact.PhoneNumber))
            {
                await bot.SendMessage(message.Chat, "Верификация прошла успешно, вам предоставлен доступ к библиотеке.",
                    replyMarkup: new ReplyKeyboardRemove());
            }
            else if (!Access.IsWhitelisted(message.Contact.PhoneNumber))
            {
                await bot.SendMessage(message.Chat, "К сожалению, вам отказано в доступе. Ваш номер не добавлен в базу, обратитесь к администратору и повторите попытку.");
            }
        }
        else if (message.Text == "/start")
        {
            await bot.SendMessage(message.Chat,
                "Добро пожаловать в чат-бот библиотеки Российского национального молодёжного симфонического оркестра!\n\n" +
                "Для получения доступа к библиотеке Вам необходимо предоставить номер телефона, привязанный к вашему аккаунту в Telegram.",
                replyMarkup: new KeyboardButton[]
                {
                    KeyboardButton.WithRequestContact("✉️ Предоставить данные")
                });  
        }

        await bot.DeleteMessage(message.Chat, message.Id);
    }
}
