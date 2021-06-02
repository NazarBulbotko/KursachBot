using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.Net.Http;
using Newtonsoft.Json;

namespace Test_1
{
    class Response_History
    {
        public float Rate { get; set; }
        public string Basic { get; set; }
        public string Date { get; set; }
    }
    class Request_History
    {
        public string Basic { get; set; }
        public string Add { get; set; }
        public string Date { get; set; }
    }
    class Response_Latest 
    {
        public string Basic { get; set; }
        public string Add { get; set; }
        public string Date { get; set; }
        public float Difference { get; set; }
        public float Percent_difference { get; set; }
        public float Rate { get; set; }
    }
    class Volatility
    {
        public string Date { get; set; }
        public string Min_Rate { get; set; }
        public float _Min_Rate { get; set; }
        public string Max_Rate { get; set; }
        public float _Max_Rate { get; set; }
        public string Middle { get; set; }
        public float _Middle { get; set; }
    }
    class Request_Calculation
    {
        public string Trend { get; set; }
        public string Basic { get; set; }
        public string Add { get; set; }
        public string Date_1 { get; set; }
        public string Date_2 { get; set; }
        public int Amount { get; set; }
        public int Multiplier { get; set; }
    }
    class Response_Calculation
    {
        public float Rate_1 { get; set; }
        public float Rate_2 { get; set; }
        public float Profit_Lose { get; set; }
    }
    class Request_Calculation_With
    {
        public string Trend { get; set; }
        public string Basic { get; set; }
        public string Add { get; set; }
        public string Date_1 { get; set; }
        public string Date_2 { get; set; }
        public int Amount { get; set; }
        public int Multiplier { get; set; }
        public int Take_Profit { get; set; }
        public int Stop_Loss { get; set; }
    }
    class Response_Calculation_With
    {
        public string Date { get; set; }
        public float Rate { get; set; }
        public float Rate_1 { get; set; }
        public float Rate_2 { get; set; }
        public float Profit_Lose { get; set; }
    }
    class Program
    {
        public static  int hist = 1;
        public static Request_History currencyRequest = new Request_History();
        public static Response_History currencyResponse = new Response_History();
        public static Request_History latestRequest = new Request_History();
        public static Response_Latest latestResponse = new Response_Latest();
        public static Request_Calculation calculationRequest = new Request_Calculation();
        public static Request_Calculation_With requestWith = new Request_Calculation_With();
        public static Response_Calculation_With responseWith = new Response_Calculation_With();
        static TelegramBotClient Bot;

        public static HttpClient client = new HttpClient();
        public static void Main(string[] args)
        {
            Bot = new TelegramBotClient("1878084651:AAGNdpTm-p0MCPeOuXfizd4Prr8RxEVbdKo");
            string apiAddress = "https://localhost:44325";
            client.BaseAddress = new Uri(apiAddress);
            var me = Bot.GetMeAsync().Result;
            Console.WriteLine(me.FirstName);
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotCallbackQueryReceived;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotCallbackQueryReceived(object sender, CallbackQueryEventArgs e)
        {
            string buttonText = e.CallbackQuery.Data;
            string name = $"{e.CallbackQuery.From.FirstName} {e.CallbackQuery.From.LastName}";
            Console.WriteLine($"{name} нажал кнопку: {buttonText}");
            await Bot.AnswerCallbackQueryAsync(e.CallbackQuery.Id, $"Вы нажали кнопку {buttonText}");
        }

        private static async void BotOnMessageReceived(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var message = e.Message;
            if (message == null || message.Type != MessageType.Text)
                return;
            string name = $"{message.From.FirstName} {message.From.LastName}";
            Console.WriteLine($"{name} відправив повідомлення: {message.Text}");
            switch (message.Text)
            {
                case "/start":
                    string text =
@"Список команд:
/start - запуск бота
/instruction - Інструкція користування ботом
/menu - Меню команд";
                    Bot.SendTextMessageAsync(message.From.Id, text);
                    break;
                case "/menu":
                    string menu =
@"Виберіть операцію:
/historical - Історичне значення валютної пари
/latest - порівняння з наявним курсом
/volatility- визначення характеристик волатильності елементів вибірки
/calculation - обрахувати прибуток/збиток при заданих умовах
/tp_sl - обрахунок прибутку/збитку при додаткових обмеженнях take profit і stop lose (у межах одного місяця)";
                    Bot.SendTextMessageAsync(message.From.Id, menu);
                    break;
                case "/instruction":
                    string inst =
                        @"Шаблони запитів:
/historical - BASE YYYY-MM-DD ADD;
/latest - BASE YYYY-MM-DD ADD;
/volatility - BASE YYYY-MM-DD ADD[0] ADD[1] ADD[2] ADD[3] ADD[4];
/calculation - TREND BASE ADD YYYY-MM-DD[0] YYYY-MM-DD[1] AMOUNT MULTI;
/tp_ls - TREND BASE ADD YYYY-MM-DD[0] YYYY-MM-DD[1] AMOUNT MULTI TP SL;
де BASE - основна валюта в парі;
ADD - додаткова валюта;
YYYY-MM-DD - дата (рік, місяць, день);
ADD[] - вибірка додаткових валют;
YYYY-MM-DD[] - вибірка дат;
TREND - тенденція на ринку (Bull або Bear);
AMOUNT - сума угоди;
MULTI - мультиплікатор;
TP - обмеження Take Profit;
SL - обмеження Stop Loss";
                    Bot.SendTextMessageAsync(message.From.Id, inst);
                    break;
                case "/historical":
                    await Bot.SendTextMessageAsync(message.From.Id, text:"Enter history:", replyMarkup: new ForceReplyMarkup() { Selective = true });
                    break;
                case "/latest":
                    await Bot.SendTextMessageAsync(message.From.Id, text: "Enter latest:", replyMarkup: new ForceReplyMarkup() { Selective = true });
                    break;
                case "/volatility":
                    await Bot.SendTextMessageAsync(message.From.Id, text: "Enter sample:", replyMarkup: new ForceReplyMarkup() { Selective = true });
                    break;
                case "/calculation":
                    await Bot.SendTextMessageAsync(message.From.Id, text: "Enter terms:", replyMarkup: new ForceReplyMarkup() { Selective = true });
                    break;
                case "/tp_sl":
                    await Bot.SendTextMessageAsync(message.From.Id, text: "Enter with add terms:", replyMarkup: new ForceReplyMarkup() { Selective = true });
                    break;
                default:
                    break;
            }
            if (e.Message.ReplyToMessage != null && e.Message.ReplyToMessage.Text.Contains("Enter latest:"))
            {
                string history = e.Message.Text;
                for (int i = 0; i < 18; i++)
                {
                    if (i < 3)
                    {
                        latestRequest.Basic += history[i];
                    }
                    else if (i > 3 && i < 14)
                    {
                        latestRequest.Date += history[i];
                    }
                    else if (i > 14)
                    {
                        latestRequest.Add += history[i];
                    }
                }
                var result = await client.GetAsync($"/CurrencyRevision/latest?Basic={latestRequest.Basic}&Date={latestRequest.Date}&Add={latestRequest.Add}");
                result.EnsureSuccessStatusCode();
                var content = result.Content.ReadAsStringAsync().Result;
                var currency = JsonConvert.DeserializeObject<Response_Latest>(content);
                Bot.SendTextMessageAsync(message.From.Id, $"Difference: {currency.Difference}\n Percent Differance: {currency.Percent_difference}%");
                latestRequest.Basic = "";
                latestRequest.Date = "";
                latestRequest.Add = "";
            }
            if (e.Message.ReplyToMessage != null && e.Message.ReplyToMessage.Text.Contains("Enter history:"))
            {
                //string history = "USD 2020-06-10 CHF";
                string history = e.Message.Text;
                for (int i = 0; i < 18; i++)
                {
                    if (i < 3)
                    {
                        currencyRequest.Basic += history[i];
                    }
                    else if (i > 3 && i < 14)
                    {
                        currencyRequest.Date += history[i];
                    }
                    else if (i > 14)
                    {
                        currencyRequest.Add += history[i];
                    }
                }
                var result = await client.GetAsync($"/CurrencyRevision/historical?Basic={currencyRequest.Basic}&Date={currencyRequest.Date}&Add={currencyRequest.Add}");
                result.EnsureSuccessStatusCode();
                var content = result.Content.ReadAsStringAsync().Result;
                var currency = JsonConvert.DeserializeObject<Response_History>(content);
                Bot.SendTextMessageAsync(message.From.Id, $"Rate: {currency.Rate}");
                currencyRequest.Basic = "";
                currencyRequest.Date = "";
                currencyRequest.Add = "";
            }
            if (e.Message.ReplyToMessage != null && e.Message.ReplyToMessage.Text.Contains("Enter sample:"))
            {
                string[] Add = { "", "", "", "", ""};
                string history = e.Message.Text;
                for (int i = 0; i < 34; i++)
                {
                    if (i < 3)
                    {
                        currencyRequest.Basic += history[i];
                    }
                    else if (i > 3 && i < 14)
                    {
                        currencyRequest.Date += history[i];
                    }
                    else if (i > 14 && i < 18)
                    {
                        Add[0] += history[i];
                    }
                    else if (i > 18 && i < 22)
                    {
                        Add[1] += history[i];
                    }
                    else if (i > 22 && i < 26)
                    {
                        Add[2] += history[i];
                    }
                    else if (i > 26 && i < 30)
                    {
                        Add[3] += history[i];
                    }
                    else if (i > 30 && i < 34)
                    {
                        Add[4] += history[i];
                    }
                }
                var result = await client.GetAsync($"/CurrencyRevision/Volatility?Basic={currencyRequest.Basic}&Date={currencyRequest.Date}&" +
                    $"Add_1={Add[0]}&Add_2={Add[1]}&Add_3={Add[2]}&Add_4={Add[3]}&Add_5={Add[4]}");
                result.EnsureSuccessStatusCode();
                var content = result.Content.ReadAsStringAsync().Result;
                var currency = JsonConvert.DeserializeObject<Volatility>(content);
                Bot.SendTextMessageAsync(message.From.Id, $"Min Pair: {currency.Min_Rate}\n Min Rate: {currency._Min_Rate}%\n " +
                    $"Max Pair: {currency.Max_Rate}\n Max Rate: {currency._Max_Rate}%\n Middle Pair: {currency.Middle}\n " +
                    $"Middle Rate: {currency._Middle}%");
                currencyRequest.Basic = "";
                currencyRequest.Date = "";
                Add[0] = Add[1] = Add[2] = Add[3] = Add[4] = "";
            }
            if (e.Message.ReplyToMessage != null && e.Message.ReplyToMessage.Text.Contains("Enter terms:"))
            {
                bool q = true;
                string amount = "";
                string multi = "";
                string history = e.Message.Text;
                for (int i = 0; i < history.Count(); i++)
                {
                    if (i < 4)
                    {
                        calculationRequest.Trend += history[i];
                    }
                    else if (i > 4 && i < 8)
                    {
                        calculationRequest.Basic += history[i];
                    }
                    else if (i > 8 && i < 12)
                    {
                        calculationRequest.Add += history[i];
                    }
                    else if (i > 12 && i < 23)
                    {
                        calculationRequest.Date_1 += history[i];
                    }
                    else if (i > 23 && i < 34)
                    {
                        calculationRequest.Date_2 += history[i];
                    }
                    else if (i > 34 && q == true)
                    {
                        amount += history[i];
                        if (history[i + 1] == ' ')
                        {
                            q = false;
                            i++;
                        }
                    }
                    else if (i > 34 && q == false)
                    {
                        multi += history[i];
                    }
                }
                calculationRequest.Amount = int.Parse(amount);
                calculationRequest.Multiplier = int.Parse(multi);
                var result = await client.GetAsync($"/CurrencyRevision/Calculation?Trend={calculationRequest.Trend}&Basic={calculationRequest.Basic}&Add={calculationRequest.Add}" +
                    $"&Date_1={calculationRequest.Date_1}&Date_2={calculationRequest.Date_2}&Amount={calculationRequest.Amount}" +
                    $"&Multiplier={calculationRequest.Multiplier}");
                result.EnsureSuccessStatusCode();
                var content = result.Content.ReadAsStringAsync().Result;
                var currency = JsonConvert.DeserializeObject<Response_Calculation>(content);
                Bot.SendTextMessageAsync(message.From.Id, $"Profit/Lose: {currency.Profit_Lose}{calculationRequest.Basic}");
                calculationRequest.Trend = calculationRequest.Basic = calculationRequest.Add = calculationRequest.Date_1 = calculationRequest.Date_2 = "";
            }
            if (e.Message.ReplyToMessage != null && e.Message.ReplyToMessage.Text.Contains("Enter with add terms:"))
            {
                int counter = 0;
                int count_ = 0;
                requestWith.Trend = requestWith.Basic = requestWith.Add = requestWith.Date_1 = requestWith.Date_2 = "";
                string amount, multiplier, tp, sl;
                amount = multiplier = tp = sl = "";
                string history = e.Message.Text;
                int size = history.Length;
                for (int i = 0; i <= 8; i++)
                {
                    switch (count_)
                    {
                        case 0:
                        while (history[counter] != ' ')
                        {
                            requestWith.Trend += history[counter];
                            counter++;
                        }
                        count_++;
                        counter++;
                            break;
                        case 1:
                            while (history[counter] != ' ')
                            {
                                requestWith.Basic += history[counter];
                                counter++;
                            }
                            count_++;
                            counter++;
                            break;
                        case 2:
                            while (history[counter] != ' ')
                            {
                                requestWith.Add += history[counter];
                                counter++;
                            }
                            count_++;
                            counter++;
                            break;
                        case 3:
                            while (history[counter] != ' ')
                            {
                                requestWith.Date_1 += history[counter];
                                counter++;
                            }
                            count_++;
                            counter++;
                            break;
                        case 4:
                            while (history[counter] != ' ')
                            {
                                requestWith.Date_2 += history[counter];
                                counter++;
                            }
                            count_++;
                            counter++;
                            break;
                        case 5:
                            while (history[counter] != ' ')
                            {
                                amount += history[counter];
                                counter++;
                            }
                            requestWith.Amount = int.Parse(amount);
                            count_++;
                            counter++;
                            break;
                        case 6:
                            while (history[counter] != ' ')
                            {
                                multiplier += history[counter];
                                counter++;
                            }
                            requestWith.Multiplier = int.Parse(multiplier);
                            count_++;
                            counter++;
                            break;
                        case 7:
                            while (history[counter] != ' ')
                            {
                                tp += history[counter];
                                counter++;
                            }
                            requestWith.Take_Profit = int.Parse(tp);
                            count_++;
                            counter++;
                            break;
                        case 8:
                            while (counter != size)
                            {
                                sl += history[counter];
                                counter++;
                            }
                            requestWith.Stop_Loss = int.Parse(sl);
                            count_++;
                            counter++;
                            break;
                    }
                }
                var result = await client.GetAsync($"/CurrencyRevision/Calculation_with_TP/SL?Trend={requestWith.Trend}&Basic={requestWith.Basic}" +
                    $"&Add={requestWith.Add}&Date_1={requestWith.Date_1}&Date_2={requestWith.Date_2}&Amount={requestWith.Amount}" +
                    $"&Multiplier={requestWith.Multiplier}&Take_Profit={requestWith.Take_Profit}&Stop_Loss={requestWith.Stop_Loss}");
                result.EnsureSuccessStatusCode();
                var content = result.Content.ReadAsStringAsync().Result;
                var currency = JsonConvert.DeserializeObject<Response_Calculation_With>(content);
                Bot.SendTextMessageAsync(message.From.Id, $"Profit/Lose: {currency.Profit_Lose} Date: {currency.Date}");
                requestWith.Trend = requestWith.Basic = requestWith.Add = requestWith.Date_1 = requestWith.Date_2 = "";
                requestWith.Amount = requestWith.Multiplier = requestWith.Stop_Loss = requestWith.Take_Profit = 0;
                amount = multiplier = tp = sl = "";
                counter = count_ = 0; 
            }
        }
    }
}