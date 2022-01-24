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
            else if (callbackMessage == "МоиФильтры")
            {
                await Message.SendUsersFiltersAsync(chatId, messageId);
            }
            else if (callbackMessage == "ДомаПоФильтрам")
            {
                try
                {
                    await Message.SendHousesForUserAsync(chatId, messageId);
                }
                catch (NotFoundException)
                {
                    await Message.SendNotFoundMessageAsync(messageId);
                }
            }

            else if (callbackMessage == "ПоТипуДома")
            {
                await Task.Run(() => CleanUserFilter(chatId));
                await Message.SendTypeListAsync(messageId);
                UsersFilters.Add((chatId, "ПоТипуДома"));
            }
            else if (callbackMessage == "ПоМетро")
            {
                await Task.Run(() => CleanUserFilter(chatId));
                await Message.SendMetroListAsync(messageId);
                UsersFilters.Add((chatId, "ПоМетро"));
            }
            else if (callbackMessage == "ПоРайону")
            {
                await Task.Run(() => CleanUserFilter(chatId));
                await Message.SendDistrictsListAsync(messageId);
                UsersFilters.Add((chatId, "Район"));
            }
            else if (callbackMessage == "ПоКомнатам")
            {
                await Task.Run(() => CleanUserFilter(chatId));
                await Message.SendRoomsNumberListAsync(messageId);
                UsersFilters.Add((chatId, "Комнаты"));
            }
            else if (callbackMessage == "ПоТипуПокупки")
            {
                await Task.Run(() => CleanUserFilter(chatId));
                await Message.SendRentTypeListAsync(messageId);
                UsersFilters.Add((chatId, "ТипПокупки"));
            }
            else if (callbackMessage == "ПоЦене")
            {
                await Message.SendPriceFilterMenuAsync(messageId);
            }
            else if (callbackMessage == "ЦенаВерх")
            {
                await Task.Run(() => CleanUserFilter(chatId));
                await Message.SubmitInputRequest("цены", messageId);
                UsersFilters.Add((chatId, "ЦенаВерх"));
            }
            else if (callbackMessage == "ЦенаНиз")
            {
                await Task.Run(() => CleanUserFilter(chatId));
                await Message.SubmitInputRequest("цены", messageId);
                UsersFilters.Add((chatId, "ЦенаНиз"));
            }
            else if (callbackMessage == "ПоМетражу")
            {
                await Message.SendFootageFilterMenuAsync(messageId);
            }
            else if (callbackMessage == "МетражВерх")
            {
                await Task.Run(() => CleanUserFilter(chatId));
                await Message.SubmitInputRequest("метража", messageId);
                UsersFilters.Add((chatId, "МетражВерх"));
            }
            else if (callbackMessage == "МетражНиз")
            {
                await Task.Run(() => CleanUserFilter(chatId));
                await Message.SubmitInputRequest("метража", messageId);
                UsersFilters.Add((chatId, "МетражНиз"));
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
                        try
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
                            else if (filterData.filterName == "ПоТипуДома")
                            {
                                try
                                {
                                    var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                    user.HouseType = callbackMessage;
                                    await UsersRepositore.UpdateUserAsync(user);
                                    await Message.SendHousesForUserAsync(chatId, messageId);
                                }
                                catch (FormatException)
                                {
                                    await Message.SendNotFoundMessageAsync(messageId);
                                }
                            }
                        }
                        catch (NotFoundException)
                        {
                            await Message.SendNotFoundMessageAsync(messageId);
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
                        else if (filterData.filterName == "ПоТипуДома")
                        {
                            try
                            {
                                await Message.SendHousesByTypeAsync(callbackMessage, messageId);
                            }
                            catch (FormatException)
                            {
                                await Message.SendNotFoundMessageAsync(messageId);
                            }
                        }
                        else if (filterData.filterName == "ПоМетро")
                        {
                            try
                            {
                                await Message.SendHousesMetroAsync(callbackMessage, messageId);
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

        private static void CleanUserFilter(long chatId)
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
                    Metro = "Войковская",
                    Footage = 60,
                    Price = 6000000,
                    RentType = "Аренда",
                    RoomsNumber = 2,
                    Type = "Кирпичный"
                });
                await HousesRepositore.AddHouseAsync(new House()
                {
                    District = "Северный",
                    Metro = "Водный стадион",
                    Footage = 36,
                    Price = 4000000,
                    RentType = "Продажа",
                    RoomsNumber = 1,
                    Type = "Монолитный"
                });
                await HousesRepositore.AddHouseAsync(new House()
                {
                    District = "Центральная",
                    Metro = "Речной вокзал",
                    Footage = 79,
                    Price = 6000000,
                    RentType = "Продажа",
                    RoomsNumber = 3,
                    Type = "Панельный"
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
            else if (UsersFilters.Any())
            {
                List<(long, string)> FiltersToRemove = new();
                foreach (var filterData in UsersFilters)
                {
                    if (await UsersRepositore.UserIsRegisteredAsync(filterData.chatId))
                    {
                        try
                        {
                            if (filterData.filterName == "ЦенаВерх")
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.HightPrice = Convert.ToInt32(inputMessage);
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendHousesForUserAsync(chatId);
                            }
                            else if (filterData.filterName == "ЦенаНиз")
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.LowerPrice = Convert.ToInt32(inputMessage);
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendHousesForUserAsync(chatId);
                            }
                            else if (filterData.filterName == "МетражВерх")
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.HightFootage = Convert.ToInt32(inputMessage);
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendHousesForUserAsync(chatId);
                            }
                            else if (filterData.filterName == "МетражНиз")
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.LowerFootage = Convert.ToInt32(inputMessage);
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendHousesForUserAsync(chatId);
                            }
                        }
                        catch 
                        {
                            await Message.SendNotFoundMessageAsync();
                        }

                        FiltersToRemove.Add(filterData);
                    }
                    else if (filterData.chatId == chatId)
                    {
                        try
                        {
                            if (filterData.filterName == "ЦенаВерх")
                            {
                                await Message.SendHousesWhithLowerPriceAsync(Convert.ToInt32(inputMessage));
                            }
                            else if (filterData.filterName == "ЦенаНиз")
                            {
                                await Message.SendHousesWhithHigerPriceAsync(Convert.ToInt32(inputMessage));
                            }
                            else if (filterData.filterName == "МетражВерх")
                            {
                                await Message.SendHousesWhithLowerFootageAsync(Convert.ToInt32(inputMessage));
                            }
                            else if (filterData.filterName == "МетражНиз")
                            {
                                await Message.SendHousesWhithHigerFootageAsync(Convert.ToInt32(inputMessage));
                            }
                        }
                        catch (FormatException)
                        {
                            await Message.SendNotFoundMessageAsync();
                        }

                        FiltersToRemove.Add(filterData);
                    }
                }
                foreach (var filter in FiltersToRemove)
                {
                    UsersFilters.Remove(filter);
                }
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
                            await Message.SendStartMenuAsync();
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
