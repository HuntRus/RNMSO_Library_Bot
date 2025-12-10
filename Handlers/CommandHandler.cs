using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RNMSO_Library_Bot.Handlers;

internal static partial class Handlers
{
    internal static async Task StartAsync(TelegramBotClient bot, Message message)
    {
        await bot.SendMessage(message.Chat,
                        "Добро пожаловать в чат-бот библиотеки Российского национального молодёжного симфонического оркестра!\n\n" +
                        "Для получения доступа к библиотеке Вам необходимо предоставить номер телефона, привязанный к вашему аккаунту в Telegram.",
                        replyMarkup: new KeyboardButton[]
                        {
                                    KeyboardButton.WithRequestContact("✉️ Предоставить данные")
                        });
    }

    internal static async Task AddUserAsync(TelegramBotClient bot, Message message)
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

    internal static async Task EditUserGroupAsync(TelegramBotClient bot, Message message)
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

    internal static async Task RemoveUserAsync(TelegramBotClient bot, Message message)
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
}