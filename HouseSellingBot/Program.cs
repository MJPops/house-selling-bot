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
            var message = e.CallbackQuery.Message;
            Messages Message = new(client, message.Chat.Id);
            try
            {
                if (e.CallbackQuery.Data == "О нас")
                {
                    await Message.SendInfoMenuAsync();
                }
                else if (e.CallbackQuery.Data == "Добавить фильтр")
                {
                    await Message.SendFilterMenuAsync();
                }
                else if (e.CallbackQuery.Data == "Аренда")
                {
                    await Message.SendRentMenuAsync();
                }
                else if (e.CallbackQuery.Data == "Покупка")
                {
                    await Message.SendSaleMenuAsync();
                }
                else if (e.CallbackQuery.Data == "РайонАренда")
                {
                    await Message.SendRentDistrictMenuAsync();
                }
                else if (e.CallbackQuery.Data == "КомнатаАренда")
                {
                    await Message.SendRentRoomsMenuAsync();
                }
                else if (e.CallbackQuery.Data == "СтоимостьАренда")
                {
                    await Message.SendRentPriceMenuAsync();
                }
                else if (e.CallbackQuery.Data == "МетражАренда")
                {
                    await Message.SendRentFootageMenuAsync();
                }
                else if (e.CallbackQuery.Data == "РайонПокупка")
                {
                    await Message.SendSaleDictrictMenuAsync();
                }
                else if (e.CallbackQuery.Data == "КомнатаПокупка")
                {
                    await Message.SendSaleRoomsMenuAsync();
                }
                else if (e.CallbackQuery.Data == "СтоимостьПокупка")
                {
                    await Message.SendSalePriceMenuAsync();
                }
                else if (e.CallbackQuery.Data == "МетражПокупка")
                {
                    await Message.SendSaleFootageMenuAsync();
                }
                else if (e.CallbackQuery.Data == "Аренда дома")
                {
                    await Message.SendDistrictMenuAsync();
                }
                else if (e.CallbackQuery.Data == "Аренда квартиры")
                {
                    await Message.SendDistrictMenuAsync();
                }
                else if (e.CallbackQuery.Data == "Покупка дома")
                {
                    await Message.SendDistrictMenuAsync();
                }
                else if (e.CallbackQuery.Data == "Покупка квартиры")
                {
                    await Message.SendDistrictMenuAsync();
                }
            }
            catch 
            {
                await client.SendTextMessageAsync(message.Chat.Id, "Ошибка");
            }
        }

        [Obsolete]
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var inputMessage = e.Message;
            Messages Message = new(client, inputMessage.Chat.Id);
            Console.WriteLine(inputMessage.Text); //TODO - delete
            try
            {
                if (inputMessage.Text == "/start")
                {
                    await Message.SendStartMenuAsync();
                }
                await UserRepositore.GetHousesWhithCustomFilters('1');
                {
                    foreach (House house in await AllHouseRepositore.GetAllHousesAsync())
                    {
                        await AllHouseRepositore.GetHouseWithHigherPrice('1');
                    }
                }
            }
            catch
            {
                await client.SendTextMessageAsync(inputMessage.Chat.Id, "Ошибка");
            }
        }
    }
}
