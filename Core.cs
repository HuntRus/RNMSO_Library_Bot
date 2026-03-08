using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using RNMSO_Library_Bot.Handlers;
using RNMSO_Library_Bot.Data;

var cts = new CancellationTokenSource();
var bot = new TelegramBotClient(Config.Token, cancellationToken: cts.Token);

var me = await bot.GetMe();
Console.WriteLine($"Listening for {me.Username} was started. Press «Enter» to terminate...");

bot.OnError += OnError;
bot.OnUpdate += OnUpdate;
bot.OnMessage += OnMessage;

async Task OnError(Exception exception, HandleErrorSource errorSource)
    => await Handlers.HandleErrorAsync(bot, exception, errorSource);

async Task OnUpdate(Update update)
    => await Handlers.HandleUpdateAsync(bot, update);

async Task OnMessage(Message message, UpdateType type)
    => await Handlers.HandleMessageAsync(bot, message);

Console.ReadLine();