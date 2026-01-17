using RNMSO_Library_Bot.Data;
using RNMSO_Library_Bot.Data.Models;
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
        var user = Data.User.Find(query.From.Id);


        if (query.Message is null)
            return;
        else if (user is null)
        {
            await bot.AnswerCallbackQuery(query.Id, "В доступе отказано");
            await bot.DeleteMessage(query.Message.Chat, query.Message.Id);
            return;
        }
        if (query.Data is "currentMonth" or "nextMonth")
            await AnswerMonthSelectionAsync(bot, query);
        else if (DateOnly.TryParse(query.Data, Config.RegionalFormat, out var date))
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

        var concerts = Library.Concerts.FindAll(c => c.Date.Year == date.Year && c.Date.Month == date.Month);
        if (concerts.Count is 0)
        {
            await bot.AnswerCallbackQuery(query.Id, $"Нет запланированных концертов на {date.ToString("MMMM", Config.RegionalFormat)}");
            return;
        }

        var markup = new InlineKeyboardMarkup();
        foreach (var concert in concerts)
        {
            markup.AddButton(concert.FileName).AddNewRow();
        }

        await bot.EditMessageText(query.Message.Chat.Id, query.Message.Id, "Выберите дату концерта", replyMarkup: markup);
    }

    private static async Task AnswerConcertSelectionAsync(TelegramBotClient bot, CallbackQuery query, DateOnly date)
    {
        var concert = Library.Concerts.Find(c => c.Date == date);

        var compositions = concert.Compositions;
        if (compositions.Count is 0)
        {
            await bot.AnswerCallbackQuery(query.Id, $"Ноты произведений для концерта {date.ToString(Config.RegionalFormat)} ещё не загружены");
            return;
        }

        var text = $"Дата: {date}\n\n" +
            $"Выберите произведение:\n";

        var markup = new InlineKeyboardMarkup();
        for (var i = 0; i < compositions.Count; i++)
        {
            text += $"{i + 1}. {compositions[i].FileName}\n";
            markup.AddButton($"{i + 1}".ToString(), $"{date.ToString(Config.RegionalFormat)}\\{ i + 1}").AddNewRow();
        }

        await bot.EditMessageText(query.Message!.Chat, query.Message.Id, text, replyMarkup: markup);
    }

    private static async Task AnswerCompositionSelectionAsync(TelegramBotClient bot, CallbackQuery query)
    {
        var date = DateOnly.Parse(query.Data.Split("\\")[0], Config.RegionalFormat);
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

        var concert = Library.Concerts.Find(c => c.Date == date);
        var composition = concert.Compositions.Find(c => c.FileName == title);

        await bot.EditMessageText(query.Message!.Chat, query.Message.Id, $"Дата: {date.ToString(Config.RegionalFormat)}\n" +
            $"Композиция: {title}\n\n" +
            $"Внимание! Файлы исчезнут через минуту после отправки.");

        await SendPartsAsync(bot, query, composition, Data.User.Find(query.From.Id).Group);
    }

    private static async Task SendPartsAsync(TelegramBotClient bot, CallbackQuery query, Composition composition, string userGroup)
    {
        var parts = composition.Parts;
        foreach (var part in parts)
        {
            var name = part.FileName.Replace(" ", "");
            name = name.Replace(".pdf", "");
            name = name.Replace("-", "");
            name = name.Replace(".", "");

            int i = 0;
            for (; char.IsDigit(name[i]); i++)
            {
            }
            
            name = name[i..];

            if (Data.Group.FindByAlias(name).Name == userGroup)
            {
                await using Stream stream = File.OpenRead(part.FullPath);
                await bot.SendDocument(query.Message.Chat, stream);
            }
        }
    }
}