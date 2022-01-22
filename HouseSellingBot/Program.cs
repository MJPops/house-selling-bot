using HouseSellingBot.Models;
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
            var callbackMessage = e.CallbackQuery.Data;
            Messages Message = new(client, e.CallbackQuery.Message.Chat.Id);

            if (callbackMessage == "ВсеДома")
            {
                await Message.SendAllHousesAsync();
            }
            else if(callbackMessage =="Фильтры")
            {
                await Message.SendFiltersMenuAsync();
            }
            else if (callbackMessage == "ПоТипуДома")
            {
                await Message.SendHousesTypeMenuAsync();
            }
            else if (callbackMessage == "Квартиры")
            {
                await Message.SendHouseByTypeAsync("Квартиры");
            }
            else if (callbackMessage == "ЧастныеДома")
            {
                await Message.SendHouseByTypeAsync("Частные Дома");
            }
        }

        [Obsolete]
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var inputMessage = e.Message;
            Messages Message = new(client, inputMessage.Chat.Id);
            Console.WriteLine(inputMessage.Text); //TODO - delete

            if (inputMessage.Text == "/start")
            {
                await Message.SendStartMenuAsync();
            }
            else if (inputMessage.Text == "Заполнить бд")
            {
                await HousesRepositore.AddHouseAsync(new House()
                {
                    District = "Центральная",
                    Footage = 60,
                    Price = 6000000,
                    RentType = "Аренда",
                    RoomsNumber = 2,
                    Type = "Квартиры"
                });
                await HousesRepositore.AddHouseAsync(new House()
                {
                    District = "Северный",
                    Footage = 36,
                    Price = 4000000,
                    RentType = "Продажа",
                    RoomsNumber = 1,
                    Type = "Квартиры"
                });
                await HousesRepositore.AddHouseAsync(new House()
                {
                    District = "Центральная",
                    Footage = 79,
                    Price = 6000000,
                    RentType = "Продажа",
                    RoomsNumber = 3,
                    Type = "Частные Дома"
                });
                await UsersRepositore.AddUserAsync(new User()
                {
                    HightFootage = 70,
                });
                await Message.SendStartMenuAsync();
            }
        }
    }
}
