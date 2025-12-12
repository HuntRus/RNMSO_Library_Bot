using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;

namespace RNMSO_Library_Bot.Handlers;

public static partial class Handlers
{
    public static async Task StartAsync(TelegramBotClient bot, Message message)
    {
        await bot.SendMessage(message.Chat,
                        "Добро пожаловать в чат-бот библиотеки Российского национального молодёжного симфонического оркестра!\n\n" +
                        "Для получения доступа к библиотеке Вам необходимо предоставить номер телефона, привязанный к вашему аккаунту в Telegram.",
                        replyMarkup: new KeyboardButton[]
                        {
                                    KeyboardButton.WithRequestContact("✉️ Предоставить данные")
                        });
    }

    public static async Task AddUserAsync(TelegramBotClient bot, Message message)
    {
        var arguments = message.Text.Split(' ');
        if (arguments.Length != 3)
        {
            await bot.SendMessage(message.Chat, "Неверный ввод. Введите команду в формате: /adduser <номер телефона> <группа>");
        }
        else
        {
            User.Add(arguments[1], arguments[2]);

            await bot.SendMessage(message.Chat, $"Пользователь {arguments[1]} успешно добавлен в базу");
        }
    }

    public static async Task EditUserGroupAsync(TelegramBotClient bot, Message message)
    {
        var arguments = message.Text.Split(' ');
        if (arguments.Length != 3)
        {
            await bot.SendMessage(message.Chat, "Неверный ввод. Введите команду в формате: /editusergroup <номер телефона> <новая группа>");
        }
        else
        {
            User.EditGroup(arguments[1], arguments[2]);

            await bot.SendMessage(message.Chat, $"Группа пользователя {arguments[1]} успешно изменена");
        }
    }

    public static async Task RemoveUserAsync(TelegramBotClient bot, Message message)
    {
        var arguments = message.Text.Split(' ');
        if (arguments.Length != 2)
        {
            await bot.SendMessage(message.Chat, "Неверный ввод. Введите команду в формате: /removeuser <номер телефона>");
        }
        else
        {
            User.Remove(arguments[1]);

            await bot.SendMessage(message.Chat, $"Пользователь {arguments[1]} успешно удален из базы");
        }
    }

    /// <summary>
    /// Sends list of available commands for the user.
    /// </summary>
    /// <param name="bot">Current bot instance</param>
    /// <param name="message">Message being processed</param>
    /// <returns></returns>
    public static async Task HelpAsync(TelegramBotClient bot, Message message)
    {
        await bot.SendMessage(message.Chat, "<b>Список доступных команд</b>\n\n" +
            "• <code>/adduser &lt;номер телефона&gt; &lt;группа&gt;</code>\n" +
            "<i>Добавить пользователя в базу</i>\n\n" +
            "• <code>/editusergroup &lt;номер телефона&gt; &lt;новая группа&gt;</code>\n" +
            "<i>Изменить группу пользователя</i>\n\n" +
            "• <code>/removeuser &lt;номер телефона&gt;</code>\n" +
            "<i>Удалить пользователя из базы</i>", ParseMode.Html);
    }
}