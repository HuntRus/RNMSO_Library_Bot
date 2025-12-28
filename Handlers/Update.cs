using RNMSO_Library_Bot.Library;
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
        if (update.CallbackQuery is null)
            return;

        var query = update.CallbackQuery;
        if (query.Message is null)
            return;
        if (query.Data is "currentMonth" or "nextMonth")
            await AnswerMonthSelectionAsync(bot, query);
        else if (DateOnly.TryParse(query.Data, Configuration.RegionalFormat, out var date))
            await AnswerConcertSelectionAsync(bot, query, date);
        else if (query.Data.Split("\\").Length == 2)
            await AnswerCompositionSelectionAsync(bot, query);

        await bot.AnswerCallbackQuery(update.CallbackQuery.Id);
    }

    private static async Task AnswerMonthSelectionAsync(TelegramBotClient bot, CallbackQuery query)
    {
        var date = DateOnly.FromDateTime(DateTime.UtcNow);
        if (query.Data is "nextMonth")
            date = date.AddMonths(1);

        var concerts = Library.Library.Concerts.FindAll(c => c.Date.Year == date.Year && c.Date.Month == date.Month);
        if (concerts.Count is 0)
        {
            await bot.SendMessage(query.Message.Chat, $"Нет запланированных концертов на {date.ToString("MMMM", Configuration.RegionalFormat)}.");
            return;
        }

        var markup = new InlineKeyboardMarkup();
        foreach (var concert in concerts)
        {
            markup.AddButton(concert.Title).AddNewRow();
        }

        await bot.EditMessageText(query.Message.Chat.Id, query.Message.Id, "Выберите дату концерта", replyMarkup: markup);
    }

    private static async Task AnswerConcertSelectionAsync(TelegramBotClient bot, CallbackQuery query, DateOnly date)
    {
        var concert = Library.Library.Concerts.Find(c => c.Date == date);

        var compositions = concert.Compositions;
        if (compositions.Count is 0)
        {
            await bot.SendMessage(query.Message!.Chat, $"Ноты произведений для концерта {date.ToString(Configuration.RegionalFormat)} ещё не загружены.");
            return;
        }

        var text = $"Дата: {date}\n\n" +
            $"Выберите произведение:\n";

        var markup = new InlineKeyboardMarkup();
        for (var i = 0; i < compositions.Count; i++)
        {
            text += $"{i + 1}. {compositions[i].Title}\n";
            markup.AddButton($"{i + 1}".ToString(), $"{date.ToString(Configuration.RegionalFormat)}\\{ i + 1}").AddNewRow();
        }

        await bot.EditMessageText(query.Message!.Chat, query.Message.Id, text, replyMarkup: markup);
    }

    private static async Task AnswerCompositionSelectionAsync(TelegramBotClient bot, CallbackQuery query)
    {
        var date = DateOnly.Parse(query.Data.Split("\\")[0], Configuration.RegionalFormat);
        var selection = query.Data.Split("\\")[1];
        var lines = query.Message.Text.Split("\n");

        string title = null;
        foreach (var line in lines)
        {
            if (line is null or "")
                continue;

            if (char.IsDigit(line[0]) && line[0] == selection[0])
            {
                title = line[3..];
                break;
            }
        }

        var concert = Library.Library.Concerts.Find(c => c.Date == date);
        var composition = concert.Compositions.Find(c => c.Title == title);

        await bot.EditMessageText(query.Message!.Chat, query.Message.Id, $"Дата: {date.ToString(Configuration.RegionalFormat)}\n" +
            $"Композиция: {title}\n\n" +
            $"Внимание! Файлы исчезнут через минуту после отправки.");

        await SendPartsAsync(bot, query, composition, User.Get(query.From.Id).Group);
    }

    private static async Task SendPartsAsync(TelegramBotClient bot, CallbackQuery query, Composition composition, string userGroup)
    {
        var parts = composition.Parts;
        foreach (var part in parts)
        {
            var name = part.Title.Replace(" ", "");
            if (char.IsDigit(name[0]))
                name = name[2..];

            name = name.Replace(".pdf", "");

            if (User.GetGroupFromFilename(name) == userGroup)
            {
                await using Stream stream = File.OpenRead(part.FilePath);
                await bot.SendDocument(query.Message.Chat, stream);
            }
        }
    }
}