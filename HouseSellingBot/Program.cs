using HouseSellingBot.Repositories;
using HouseSellingBot.UI;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace HouseSellingBot
{
    class Program
    {
        private static string token { get; set; } = "5009457163:AAEz5eg_AAz26uVh9rLmdvdq7pxfCCzYBzo";
        private static TelegramBotClient client;

        [Obsolete]
        static void Main()
        {
            try
            {
                client = new TelegramBotClient(token);
                client.StartReceiving();
                client.OnMessage += OnMessageHandler;
                client.OnCallbackQuery += OnCallbackQweryHandlerAsync;
                Console.ReadLine();
                client.StopReceiving();
            }
            catch
            {
                Console.WriteLine("ERROR");
                Console.ReadLine();
            }
        }

        [Obsolete]
        private static async void OnCallbackQweryHandlerAsync(object sender, CallbackQueryEventArgs e)
        {

        }

        [Obsolete]
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            Messages Message = new(client, e);
            var inputMessage = e.Message;
            Console.WriteLine(inputMessage.Text); //TODO - delete

            if (inputMessage.Text == "/start")
            {
                foreach (var item in await AllHouseRepositore.GetAllHousesAsync())
                {
                    Console.WriteLine(item.Description);
                }
            }
        }
    }
}
