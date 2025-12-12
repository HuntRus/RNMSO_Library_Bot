using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace RNMSO_Library_Bot.Handlers;

/// <summary>
/// <inheritdoc cref="RNMSO_Library_Bot.Handlers.Handlers"/>
/// </summary>
public static partial class Handlers
{
    /// <summary>
    /// Process message from a user.
    /// </summary>
    /// <param name="bot">Current bot instance</param>
    /// <param name="message">Message being processed</param>
    /// <returns></returns>
    public static async Task HandleMessageAsync(TelegramBotClient bot, Message message)
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

                else if (message.Text.StartsWith("/help"))
                    await HelpAsync(bot, message);
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
                if (userByNumber.Id == default)
                {
                    User.EditId(phoneNumber, id);
                    await bot.SendMessage(message.Chat, "<b>Доступ предоставлен</b>\n" +
                        "Верификация пройдена, теперь вы можете воспользоваться функционалом чат-бота.\n", ParseMode.Html,
                        replyMarkup: new ReplyKeyboardRemove());
                }
                else
                {
                    await bot.SendMessage(message.Chat, "<b>В доступе отказано</b>\n" +
                        "Данный номер телефона привязан к другому аккаунту, нажмите на кнопку «Предоставить данные».\n" +
                        "Если вы снова увидите это сообщение - обратитесь к администратору.", ParseMode.Html);
                }
            }
            else
            {
                await bot.SendMessage(message.Chat, "<b>В доступе отказано</b>\n" +
                    "Ваш номер телефона в базе не найден, обратитесь к администратору и повторите попытку.", ParseMode.Html);
            }
        }
    }
}
