using HouseSellingBot.Models;
using HouseSellingBot.Repositories;
using HouseSellingBot.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace HouseSellingBot
{
    class Program
    {
        private static string Token { get; set; } = "5009457163:AAEz5eg_AAz26uVh9rLmdvdq7pxfCCzYBzo";
        private static TelegramBotClient client;
        private static List<(long, string)> UnregisteredUsersFilters = new();

        [Obsolete]
        static void Main()
        {
            try
            {
                client = new TelegramBotClient(Token);
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
            long chatId = e.CallbackQuery.Message.Chat.Id;
            Messages Message = new(client, e.CallbackQuery.Message.Chat.Id);
            Console.WriteLine(callbackMessage); //TODO - delete

            if (callbackMessage == "ВсеДома")
            {
                await Message.SendAllHousesAsync();


            }
            else if (callbackMessage == "Фильтры")
            {
                await Message.SendFiltersMenuAsync();
            }
            else if (callbackMessage == "ПоТипуДома")
            {
                await Message.SendHousesTypeMenuAsync();
            }
            else if (callbackMessage == "Квартиры")
            {
                await Message.SendHousesByTypeAsync("Квартиры");
            }
            else if (callbackMessage == "ЧастныеДома")
            {
                await Message.SendHousesByTypeAsync("Частные Дома");
            }
            else if (callbackMessage == "ПоРайону")
            {
                await Message.SendDistrictsListAsync();
                UnregisteredUsersFilters.Add((chatId, "Район"));
            }
            else if (callbackMessage == "ПоКомнатам")
            {
                await Message.SendRoomsNumberListAsync();
                UnregisteredUsersFilters.Add((chatId, "Комнаты"));
            }
            else if (callbackMessage == "ПоТипуПокупки")
            {
                await Message.SendRentTypeListAsync();
            }
            else if (callbackMessage == "КвартирыРег")
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                user.HouseType = "Квартиры";
                await UsersRepositore.UpdateUserAsync(user);
                await Message.SendHousesForUserAsync(chatId);
            }
            else if (callbackMessage == "ЧастныеДомаРег")
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                user.HouseType = "Частные Дома";
                await UsersRepositore.UpdateUserAsync(user);
                await Message.SendHousesForUserAsync(chatId);
            }
            else if (callbackMessage == "ОчиститьФильтры")
            {
                await UsersRepositore.ClearUserFiltersAsync(chatId);
            }
        }

        [Obsolete]
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            var inputMessage = e.Message.Text;
            long chatId = e.Message.Chat.Id;
            Messages Message = new(client, e.Message.Chat.Id);
            Console.WriteLine(inputMessage); //TODO - delete

            if (inputMessage == "/start")
            {
                await Message.SendStartMenuAsync();
            }
            else if (inputMessage == "Заполнить бд")
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
            else if (inputMessage == "Удалить меня")
            {
                await UsersRepositore.RemoveUserByChatIdAsync(chatId);
                await Message.SendStartMenuAsync();
            }
            else if (UnregisteredUsersFilters.Any())
            {
                List<(long, string)> FiltersToRemove = new();
                foreach ((long, string) filterData in UnregisteredUsersFilters)
                {
                    if (filterData.Item1 == chatId)
                    {
                        if (filterData.Item2 == "Район")
                        {
                            await Message.SendHousesByDistrictAsync(inputMessage);
                        }
                        else if (filterData.Item2 == "Комнаты")
                        {
                            try
                            {
                                await Message.SendHousesByRoomsNumberAsync(Convert.ToInt32(inputMessage));
                            }
                            catch (FormatException)
                            {
                                await Message.SendNotFoundMessageAsync();
                            }
                        }
                        FiltersToRemove.Add(filterData);
                    }
                }
                foreach (var filter in FiltersToRemove)
                {
                    UnregisteredUsersFilters.Remove(filter);
                }
            }
            else
            {
                try
                {
                    if (inputMessage.Substring(0, 12) == "Регистрация ")
                    {
                        await UsersRepositore.AddUserAsync(new User
                        {
                            ChatId = chatId,
                            Name = inputMessage.Substring(12)
                        });
                    }
                }
                catch (ArgumentOutOfRangeException) { }//It's OK
            }
        }
    }
}
