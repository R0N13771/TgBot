// See https://aka.ms/new-console-template for more information
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
Dictionary<string, string> phoneBook = new Dictionary<string, string>();
bool Wait_your_name = false;
string name = "";

using var cts = new CancellationTokenSource();
var bot = new TelegramBotClient("7256504999:AAG7sHn4YlFvRPM2pU6dwR7dgeuuUi6BrQI", cancellationToken: cts.Token);
var me = await bot.GetMe();
bot.OnError += OnError;
bot.OnMessage += OnMessage;
bot.OnUpdate += OnUpdate;

Console.WriteLine($"@{me.Username} is running... Press Enter to terminate");
Console.ReadLine();
cts.Cancel(); // stop the bot

// method to handle errors in polling or in your OnMessage/OnUpdate code
async Task OnError(Exception exception, HandleErrorSource source)
{
    Console.WriteLine(exception); // just dump the exception to the console
}

// method that handle messages received by the bot:
async Task OnMessage(Message msg, UpdateType type)
{
    if (msg.Text == "/start")
    {

        bool phoneExists1 = phoneBook.ContainsKey(Convert.ToString(msg.Chat.Id));
        if (phoneExists1)
        {
            await bot.SendMessage(msg.Chat, $"Привет {name} ");
        }
        else
        {
            await bot.SendMessage(msg.Chat, "Введите свое имя");
            Wait_your_name = true;
        }
    }
    else if (msg.Text == "Здарова")
    {
        await bot.SendMessage(msg.Chat, "Здарова!");

    }
    else if (Wait_your_name)
    {
        name = msg.Text;
        var replyMarkup = new ReplyKeyboardMarkup(true)
       .AddButtons("Борщ", "Солянка");
        var send = await bot.SendMessage(msg.Chat, $"Здарова , {name}!", replyMarkup: replyMarkup);
        Wait_your_name = false;
        phoneBook.Add(Convert.ToString(msg.Chat.Id), name);
    }
    else if (msg.Text == "Борщ")
    {
        var replyMarkup = new ReplyKeyboardMarkup(true);
      var send = await bot.SendMessage(msg.Chat, "Ингредиенты: \r\nМясо на кости (у меня говядина) - 400 г\r\nСвёкла - 200 г\r\nКапуста белокочанная - 150 г\r\nПерец болгарский красный - 100 г\r\nКартофель - 1-2 крупных\r\nМорковь - 100 г\r\nЛук репчатый - 100 г\r\nЧеснок - 1-2 зубчика\r\nТоматы в собственном cоку - 2 ст. ложки\r\nЛимон - 1 долька\r\nУксус винный белый (для отвара) - 1 ст. ложка\r\nПерец душистый или перец чёрный молотый\r\nЛавровый лист\r\nЧеснок сушёный\r\nСоль\r\nСахар (если овощи несладкие)\r\nЗелень свежая\r\nВода - 2 л для бульона + 200 мл для отвара \r\n https://www.youtube.com/watch?v=L_CZYpYPozk", replyMarkup: replyMarkup);
    }
    else if (msg.Text == "Солянка")
    {
        var replyMarkup = new ReplyKeyboardMarkup(true);
        var send = await bot.SendMessage(msg.Chat, "Ингредиенты: \r\nБульон - 2 л\r\nСосиски - 4 шт.\r\nВарёная колбаса - 200 г\r\nВетчина - 200 г\r\nСолёные огурцы - 4 шт.\r\nРепчатый лук - 1 шт.\r\nМаслины - 1 банка\r\nПетрушка - 1 пучок\r\nТоматная паста - 2 ст. л.\r\nЛимон - 1 шт.\r\nСметана - 1 упаковка\r\n https://www.youtube.com/watch?v=g0eoDOybiNM", replyMarkup: replyMarkup);
    }
}

// method that handle other types of updates received by the bot:
async Task OnUpdate(Update update)
{
    if (update is { CallbackQuery: { } query }) // non-null CallbackQuery
    {
        await bot.AnswerCallbackQuery(query.Id, $"You picked {query.Data}");
        await bot.SendMessage(query.Message!.Chat, $"User {query.From} clicked on {query.Data}");
    }
}

