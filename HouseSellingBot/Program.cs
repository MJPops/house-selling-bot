using HouseSellingBot.Models;
using HouseSellingBot.PersonalExceptions;
using HouseSellingBot.Repositories;
using HouseSellingBot.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace HouseSellingBot
{
    class Program
    {
        private static string Token { get; set; } = "5009457163:AAEz5eg_AAz26uVh9rLmdvdq7pxfCCzYBzo";
        private static TelegramBotClient client;
        private static List<(long chatId, string filterName)> UsersFilters = new();

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
            string callbackMessage = e.CallbackQuery.Data;
            long chatId = e.CallbackQuery.Message.Chat.Id;
            int messageId = e.CallbackQuery.Message.MessageId;
            Messages Message = new(client, chatId);
            Console.WriteLine(callbackMessage); //TODO - delete

            if (callbackMessage == "/start")
            {
                await Message.EditStartMenuAsync(messageId);
            }
            else if (callbackMessage == "ВсеДома")
            {
                await Message.SendAllHousesAsync();
            }
            else if (callbackMessage == "Фильтры")
            {
                await Message.SendFiltersMenuAsync(messageId);
            }

            else if (callbackMessage == "ПоТипуДома")
            {
                await Message.SendTypeListAsync(messageId);
            }
            else if (callbackMessage == "Квартиры")
            {
                await Message.SendHousesByTypeAsync("Квартиры", messageId);
            }
            else if (callbackMessage == "Частные Дома")
            {
                await Message.SendHousesByTypeAsync("Частные Дома", messageId);
            }
            else if (callbackMessage == "ПоРайону")
            {
                await Task.Run(() => CleanUnregisterUserFilter(chatId));
                await Message.SendDistrictsListAsync(messageId);
                UsersFilters.Add((chatId, "Район"));
            }
            else if (callbackMessage == "ПоКомнатам")
            {
                await Task.Run(() => CleanUnregisterUserFilter(chatId));
                await Message.SendRoomsNumberListAsync(messageId);
                UsersFilters.Add((chatId, "Комнаты"));
            }
            else if (callbackMessage == "ПоТипуПокупки")
            {
                await Task.Run(() => CleanUnregisterUserFilter(chatId));
                await Message.SendRentTypeListAsync(messageId);
                UsersFilters.Add((chatId, "ТипПокупки"));
            }
            else if (callbackMessage == "ОчиститьФильтры")
            {
                await UsersRepositore.ClearUserFiltersAsync(chatId);
            }
            else if (UsersFilters.Any())
            {
                List<(long, string)> FiltersToRemove = new();
                foreach (var filterData in UsersFilters)
                {
                    if (await UsersRepositore.UserIsRegisteredAsync(filterData.chatId))
                    {
                        if (filterData.filterName == "Район")
                        {
                            var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                            user.HouseDistrict = callbackMessage;
                            await UsersRepositore.UpdateUserAsync(user);
                            await Message.SendHousesForUserAsync(chatId, messageId);
                        }
                        else if (filterData.filterName == "Комнаты")
                        {
                            try
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.HouseRoomsNumbe = Convert.ToInt32(callbackMessage);
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendHousesForUserAsync(chatId, messageId);
                            }
                            catch (FormatException)
                            {
                                await Message.SendNotFoundMessageAsync(messageId);
                            }
                        }
                        else if (filterData.filterName == "ТипПокупки")
                        {
                            try
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.HouseRentType = callbackMessage;
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendHousesForUserAsync(chatId, messageId);
                            }
                            catch (FormatException)
                            {
                                await Message.SendNotFoundMessageAsync(messageId);
                            }
                        }
                    }
                    else if (filterData.chatId == chatId)
                    {
                        if (filterData.filterName == "Район")
                        {
                            await Message.SendHousesByDistrictAsync(callbackMessage, messageId);
                        }
                        else if (filterData.filterName == "Комнаты")
                        {
                            try
                            {
                                await Message.SendHousesByRoomsNumberAsync(Convert.ToInt32(callbackMessage),
                                    messageId);
                            }
                            catch (FormatException)
                            {
                                await Message.SendNotFoundMessageAsync(messageId);
                            }
                        }
                        else if (filterData.filterName == "ТипПокупки")
                        {
                            try
                            {
                                await Message.SendHousesByRentTypeAsync(callbackMessage, messageId);
                            }
                            catch (FormatException)
                            {
                                await Message.SendNotFoundMessageAsync(messageId);
                            }
                        }

                        FiltersToRemove.Add(filterData);
                    }
                }
                foreach (var filter in FiltersToRemove)
                {
                    UsersFilters.Remove(filter);
                }
            }
        }

        private static void CleanUnregisterUserFilter(long chatId)
        {
            List<(long chatId, string Filter)> filtersToRemove = new();
            foreach (var filter in UsersFilters)
            {
                if (filter.chatId == chatId)
                {
                    filtersToRemove.Add(filter);
                }
            }
            foreach (var filter in filtersToRemove)
            {
                UsersFilters.Remove(filter);
            }
        }

        [Obsolete]
        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            string inputMessage = e.Message.Text;
            long chatId = e.Message.Chat.Id;
            Messages Message = new(client, chatId);
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
            else
            {
                try
                {
                    if (inputMessage.Substring(0, 12) == "Регистрация ")
                    {
                        try
                        {
                            await UsersRepositore.AddUserAsync(new User
                            {
                                ChatId = chatId,
                                Name = inputMessage.Substring(12)
                            });
                        }
                        catch (AlreadyContainException)
                        {
                            await Message.SendAlreadyRegisterAsync();
                        }
                    }
                }
                catch (ArgumentOutOfRangeException) { }//It's OK
            }
        }
    }
}
