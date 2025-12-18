using System.Globalization;
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
    /// Sends a welcome message.
    /// </summary>
    /// <param name="bot">Current bot instance</param>
    /// <param name="message">Message being processed</param>
    /// <returns></returns>
    public static async Task StartAsync(TelegramBotClient bot, Message message)
    {
        await bot.SendMessage(message.Chat,
            "<b>Добро пожаловать в чат-бот библиотеки Российского национального молодёжного симфонического оркестра!</b>\n\n" +
            "Для получения доступа к библиотеке Вам необходимо предоставить <i>номер телефона, привязанный к вашему аккаунту в Telegram.</i>", ParseMode.Html,
            replyMarkup: new KeyboardButton[] { KeyboardButton.WithRequestContact("✉️ Предоставить данные") });
    }

    /// <summary>
    /// Sends a menu.
    /// </summary>
    /// <param name="bot">Current bot instance</param>
    /// <param name="message">Message being processed</param>
    /// <returns></returns>
    public static async Task MenuAsync(TelegramBotClient bot, Message message)
    {
        var now = DateTime.Now;

        var currentMonth = now.ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-ru"));
        var nextMonth = now.AddMonths(1).ToString("MMMM yyyy", CultureInfo.CreateSpecificCulture("ru-ru"));

        var currentMonthString = char.ToUpper(currentMonth[0]) + currentMonth[1..];
        var nextMonthString = char.ToUpper(nextMonth[0]) + nextMonth[1..];

        await bot.SendMessage(message.Chat,
            "Выберите месяц", ParseMode.Html,
            replyMarkup: new InlineKeyboardButton[][]
            {
                [(currentMonthString, "CurrentMonth")],
                [(nextMonthString, "NextMonth")]
            });
    }

    /// <summary>
    /// Adds a user to the database.
    /// </summary>
    /// <param name="bot">Current bot instance</param>
    /// <param name="message">Message being processed</param>
    /// <returns></returns>
    public static async Task AddUserAsync(TelegramBotClient bot, Message message)
    {
        var arguments = message.Text!.Split(' ');
        if (arguments.Length != 3)
        {
            await bot.SendMessage(message.Chat, "<b>Команда введена неверно</b>\n" +
                "Введите команду в формате:\n<code>/adduser &lt;номер телефона&gt &lt;группа&gt;</code>", ParseMode.Html);
        }
        else if (arguments[1].StartsWith('+') || arguments[1].Length != 11)
        {
            await bot.SendMessage(message.Chat, "<b>Номер телефона введён некорректно</b>\n" +
                "Необходимо вводить номер телефона <i>начиная с цифры 7 и без знака + перед ним: </i>" +
                "<i>79123456789</i>", ParseMode.Html);
        }
        else if (User.GroupList.Contains(arguments[2]) == false)
        {
            await bot.SendMessage(message.Chat, "<b>Неверно указана группа</b>\n" +
                "Ознакомиться с полным списком групп можно с помощью команды /start.", ParseMode.Html);
        }
        else
        {
            User.Add(arguments[1], arguments[2]);

            // check for null

            await bot.SendMessage(message.Chat, $"<b>Пользователь <code>{arguments[1]}</code> добавлен в базу.</b>", ParseMode.Html);
        }
    }

    /// <summary>
    /// Changes a user's group.
    /// </summary>
    /// <param name="bot">Current bot instance</param>
    /// <param name="message">Message being processed</param>
    /// <returns></returns>
    public static async Task EditUserGroupAsync(TelegramBotClient bot, Message message)
    {
        var arguments = message.Text!.Split(' ');
        if (arguments.Length != 3)
        {
            await bot.SendMessage(message.Chat, "<b>Команда введена неверно</b>\n" +
                "Введите команду в формате:\n<code>/editusergroup &lt;номер телефона&gt; &lt;новая группа&gt;</code>", ParseMode.Html);
        }
        else if (arguments[1].StartsWith('+') || arguments[1].Length != 11)
        {
            await bot.SendMessage(message.Chat, "<b>Номер телефона введён некорректно</b>\n" +
                "Необходимо вводить номер телефона <i>начиная с цифры 7 и без знака + перед ним: </i>" +
                "<i>79123456789</i>", ParseMode.Html);
        }
        else if (User.GroupList.Contains(arguments[2]) == false)
        {
            await bot.SendMessage(message.Chat, "<b>Неверно указана группа</b>\n" +
                "Ознакомиться с полным списком групп можно с помощью команды /start.", ParseMode.Html);
        }
        else
        {
            User.EditGroup(arguments[1], arguments[2]);

            await bot.SendMessage(message.Chat, $"<b>Группа пользователя <code>{arguments[1]}</code> изменена.</b>", ParseMode.Html);
        }
    }

    /// <summary>
    /// Removes a user from the database.
    /// </summary>
    /// <param name="bot">Current bot instance</param>
    /// <param name="message">Message being processed</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public static async Task RemoveUserAsync(TelegramBotClient bot, Message message)
    {
        var arguments = message.Text!.Split(' ');
        if (arguments.Length != 2)
        {
            await bot.SendMessage(message.Chat, "<b>Команда введена неверно</b>\n" +
                "Введите команду в формате:\n<code>/removeuser &lt;номер телефона&gt;</code>", ParseMode.Html);
        }
        else if (arguments[1].StartsWith('+') || arguments[1].Length != 11)
        {
            await bot.SendMessage(message.Chat, "<b>Номер телефона введён некорректно</b>\n" +
                "Необходимо вводить номер телефона <i>начиная с цифры 7 и без знака + перед ним: </i>" +
                "<i>79123456789</i>", ParseMode.Html);
        }
        else
        {
            User.Remove(arguments[1]);

            await bot.SendMessage(message.Chat, $"<b>Пользователь <code>{arguments[1]}</code> удален из базы.</b>", ParseMode.Html);
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
        await bot.SendMessage(message.Chat, "<b>Справка</b>\n\n" +
            "Необходимо вводить номер телефона <i>начиная с цифры 7 и без знака + перед ним:</i>\n" +
            "<i>79123456789</i>\n\n" +
            "Указывать группу нужно с тем же названием и в том же регистре, что и в списке далее. " +
            "В ином случае <i>пользователь не сможет получить ноты.</i>\n\n" +
            "На данный момент доступны группы " +
            "<code>FirstViolin</code>, <code>SecondViolin</code>, " +
            "<code>Viola</code>, <code>Cello</code>, " +
            "<code>Contrabass</code>, <code>Flute</code>, " +
            "<code>Oboe</code>, <code>Clarinet</code>, " +
            "<code>Bassoon</code>, <code>Horn</code>, " +
            "<code>Trumpet</code>, <code>TromboneAndTuba</code>, " +
            "<code>Percussion</code>, <code>HarpAndKeyboard</code>, <code>OtherInstruments</code>\n\n" +
            "<b>Список доступных команд</b>\n\n" +
            "• <code>/adduser &lt;номер телефона&gt; &lt;группа&gt;</code>\n" +
            "<i>Добавить пользователя в базу</i>\n\n" +
            "• <code>/editusergroup &lt;номер телефона&gt; &lt;новая группа&gt;</code>\n" +
            "<i>Изменить группу пользователя</i>\n\n" +
            "• <code>/removeuser &lt;номер телефона&gt;</code>\n" +
            "<i>Удалить пользователя из базы</i>\n\n", ParseMode.Html);
    }
}