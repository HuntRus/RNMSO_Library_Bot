using System.Globalization;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace RNMSO_Library_Bot.Handlers;

/// <summary>
/// Contains methods that process user's input.
/// </summary>
public static partial class Handlers
{
    public static async Task HandleUpdateAsync(TelegramBotClient bot, Update update)
    {
        if (update.CallbackQuery == null)
            return;

        var query = update.CallbackQuery;
        if (query.Data == "CurrentMonth" || query.Data == "NextMonth")
        {
            await SelectMonthAsync(bot, query);
        }
        else if (DateTime.TryParse(update.CallbackQuery.Data, CultureInfo.GetCultureInfo("ru-ru").DateTimeFormat, out DateTime asd))
        {
            await SelectConcertAsync(bot, query, asd);
        }

        await bot.AnswerCallbackQuery(update.CallbackQuery.Id);
    }

    public static async Task SelectMonthAsync(TelegramBotClient bot, CallbackQuery query)
    {
        var date = DateTime.Now.Date;
        if (query.Data == "NextMonth")
            date = date.AddMonths(1);

        var concerts = Library.Concerts.FindAll(x => x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day >= date.Day);

        if (concerts == null)
        {
            await bot.SendMessage(query.Message.Chat, "<b>В выбранный период нет программ</b>\n", Telegram.Bot.Types.Enums.ParseMode.Html);
            return;
        }

        var markup = new List<List<InlineKeyboardButton>>() { };
        foreach (var concert in concerts)
        {
            var button = new List<InlineKeyboardButton>() { concert.Date.ToString("dd.MM.yyyy") };
            markup.Add(button);
        }

        await bot.SendMessage(query.Message.Chat, "Выберите дату", replyMarkup: markup);
    }

    public static async Task SelectConcertAsync(TelegramBotClient bot, CallbackQuery query, DateTime date)
    {
        var concert = Library.Concerts.Find(x => x.Date == date);

        var markup = new List<List<InlineKeyboardButton>>() { };
        foreach (var track in concert.Tracks)
        {
            var button = new List<InlineKeyboardButton>() { track.Title };
            markup.Add(button);
        }

        await bot.SendMessage(query.Message.Chat, "Выберите программу", replyMarkup: markup);
    }
}