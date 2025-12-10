using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace RNMSO_Library_Bot.Handlers;

internal static partial class Handlers
{
    internal static async Task HandleMessageAsync(TelegramBotClient bot, Message message, UpdateType type)
    {
        var id = message.From!.Id;
        var userById = User.Get(id);

        if (message.Text != null)
        {
            if (userById != null && userById.Group == "Librarian")
            {
                if (message.Text.StartsWith("/adduser"))
                    await AddUserAsync(bot, message);

                else if (message.Text.StartsWith("/editusergroup"))
                    await EditUserGroupAsync(bot, message);

                else if (message.Text.StartsWith("/removeuser"))
                    await RemoveUserAsync(bot, message);
            }
            else if (message.Text == "/start")
            {
                await StartAsync(bot, message);
            }
        }
        else if (message.Contact != null && userById == null)
        {
            var phoneNumber = message.Contact.PhoneNumber;
            var userByNumber = User.Get(phoneNumber);

            if (userByNumber != null)
            {
                User.EditId(phoneNumber, id);
                await bot.SendMessage(message.Chat, "Верификация прошла успешно, вам предоставлен доступ к библиотеке.",
                    replyMarkup: new ReplyKeyboardRemove());
            }
            else
            {
                var answer = await bot.SendMessage(message.Chat, "К сожалению, вам отказано в доступе. Ваш номер не добавлен в базу, обратитесь к администратору и повторите попытку.");

                await Task.Delay(10000);
                await bot.DeleteMessage(answer.Chat, answer.MessageId);
            }
        }

        await bot.DeleteMessage(message.Chat, message.Id);
    }
}
