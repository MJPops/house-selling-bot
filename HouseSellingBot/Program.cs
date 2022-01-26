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
        private static List<(long chatId, int code)> RegistrationData = new();

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
            if (callbackMessage == "МоиИзбранные")
            {
                await Message.SendFavoriteHousesAsync();
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
                await client.SendTextMessageAsync(chatId, "Ваши фильтры удалены",
                    replyMarkup: Buttons.BackToFilters());
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
                                await Message.SendUsersFiltersAsync(chatId, messageId);
                            }
                            else if (filterData.filterName == "Комнаты")
                            {
                                try
                                {
                                    var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                    user.HouseRoomsNumbe = Convert.ToInt32(callbackMessage);
                                    await UsersRepositore.UpdateUserAsync(user);
                                    await Message.SendUsersFiltersAsync(chatId, messageId);
                                }
                                catch (FormatException)
                                {
                                    await Message.SendNotFoundMessageAsync(messageId);
                                }
                            }
                            else if (filterData.filterName == "ТипПокупки")
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.HouseRentType = callbackMessage;
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendUsersFiltersAsync(chatId, messageId);
                            }
                            else if (filterData.filterName == "ПоТипуДома")
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.HouseType = callbackMessage;
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendUsersFiltersAsync(chatId, messageId);
                            }
                            else if (filterData.filterName == "ПоМетро")
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.HouseMetro = callbackMessage;
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendUsersFiltersAsync(chatId, messageId);
                            }
                        }
                        catch (NotFoundException)
                        {
                            await Message.SendNotFoundMessageAsync(messageId);
                        }

                        FiltersToRemove.Add(filterData);
                    }
                    else if (filterData.chatId == chatId)
                    {
                        try
                        {
                            if (filterData.filterName == "Район")
                            {
                                await Message.SendHousesByDistrictAsync(callbackMessage, messageId);
                            }
                            else if (filterData.filterName == "Комнаты")
                            {
                                await Message.SendHousesByRoomsNumberAsync(Convert.ToInt32(callbackMessage),
                                    messageId);
                            }
                            else if (filterData.filterName == "ТипПокупки")
                            {
                                await Message.SendHousesByRentTypeAsync(callbackMessage, messageId);
                            }
                            else if (filterData.filterName == "ПоТипуДома")
                            {
                                await Message.SendHousesByTypeAsync(callbackMessage, messageId);
                            }
                            else if (filterData.filterName == "ПоМетро")
                            {
                                await Message.SendHousesMetroAsync(callbackMessage, messageId);
                            }
                        }
                        catch (FormatException)
                        {
                            await Message.SendNotFoundMessageAsync(messageId);
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
                    if (callbackMessage.Substring(0, 7) == "Удалить")
                    {
                        await UsersRepositore.
                            RemoveUserByChatIdAsync(Convert.ToInt32(callbackMessage.Substring(8)));
                        await Message.SendAdminsRedactionMenuAsync(messageId);
                    }
                    else if (callbackMessage.Substring(0, 9) == "Избранное")
                    {
                        await UsersRepositore.AddFavoriteHouseToUserAsync(chatId,
                            Convert.ToInt32(callbackMessage.Substring(9)));
                    }
                }
                catch { }//It's OK
            }
        }

        private static void CleanUserFilter(long chatId)
        {
            List<(long chatId, string filterName)> filtersToRemove = new();
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
            int messageId = e.Message.MessageId;
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
                    PicturePath = "https://telegra.ph/file/915e952e35f0cb24be7c4.jpg",
                    WebPath = "",
                    District = "ЖК HeadLiner",
                    Metro = "Шелепиха",
                    Footage = 41,
                    Price = 17600000,
                    RentType = "Покупка",
                    RoomsNumber = 1,
                    Type = "Панельный"
                });
                await HousesRepositore.AddHouseAsync(new House()
                {
                    PicturePath = "https://telegra.ph/file/8f82c09d39585f1464d4a.jpg",
                    WebPath = "https://telegra.ph/3-kom-kv-v-ZHK-HeadLiner-33-300-000r-11-19",
                    District = "ЖК HeadLiner",
                    Metro = "Шелепиха",
                    Footage = 70,
                    Price = 33300000,
                    RentType = "Продажа",
                    RoomsNumber = 3,
                    Type = "Панельный"
                });
                await HousesRepositore.AddHouseAsync(new House()
                {
                    PicturePath = "https://telegra.ph/file/8dc56f710a9678174992f.jpg",
                    WebPath = "https://telegra.ph/4-komnatnaya-kvartira-v-ZHK-Dom-na-Tishinke-05-18",
                    District = "ЖК Дом на Тишинке",
                    Metro = "Войковская",
                    Footage = 110,
                    Price = 73000000,
                    RentType = "Продажа",
                    RoomsNumber = 4,
                    Type = "Панельный"
                });
                await Message.SendStartMenuAsync();
            }
            else if (inputMessage == "Удалить меня")
            {
                await UsersRepositore.RemoveUserByChatIdAsync(chatId);
                await Message.SendStartMenuAsync();
            }
            else if (inputMessage == "Меню редактирования админов")
            {
                await Message.SendAdminsRedactionMenuAsync();
            }
            else if (inputMessage == "Запросить права администратора")
            {
                if (await UsersRepositore.UserIsAdminAsync(chatId) ||
                    await UsersRepositore.UserIsDirectorAsync(chatId))
                {
                    await Message.SendAlreadyAdminAsync();
                }
                else
                {
                    try
                    {
                        await Task.Run(() => CleanRegistrationDataFilter(chatId));
                        var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                        Random random = new();
                        int code = random.Next(1000, 9999);
                        await Message.SendAdminRegistrationCodeAsync(code, user.Name);
                        RegistrationData.Add((chatId, code));
                    }
                    catch
                    {
                        await Message.SendNotRegistredAsync();
                    }
                }
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
                                await Message.SendUsersFiltersAsync(chatId, messageId);
                            }
                            else if (filterData.filterName == "ЦенаНиз")
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.LowerPrice = Convert.ToInt32(inputMessage);
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendUsersFiltersAsync(chatId, messageId);
                            }
                            else if (filterData.filterName == "МетражВерх")
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.HightFootage = Convert.ToInt32(inputMessage);
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendUsersFiltersAsync(chatId, messageId);
                            }
                            else if (filterData.filterName == "МетражНиз")
                            {
                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                user.LowerFootage = Convert.ToInt32(inputMessage);
                                await UsersRepositore.UpdateUserAsync(user);
                                await Message.SendUsersFiltersAsync(chatId, messageId);
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
            else if (RegistrationData.Any())
            {
                List<(long, int)> RegistrationDataToRemove = new();
                foreach (var registrationData in RegistrationData)
                {
                    try
                    {
                        if (registrationData.chatId == chatId
                            && registrationData.code == Convert.ToInt32(inputMessage))
                        {
                            var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                            user.Role = "admin";
                            await UsersRepositore.UpdateUserAsync(user);
                            await Message.SendYouAdminAsync(await UsersRepositore.GetDirectorChatIdAsync());

                            RegistrationDataToRemove.Add(registrationData);
                        }
                        else
                        {
                            await Message.SendCodeNotWorkAsync();
                        }
                    }
                    catch (FormatException)
                    {
                        await Message.SendCodeNotWorkAsync();
                    }
                }
                foreach (var filter in RegistrationDataToRemove)
                {
                    RegistrationData.Remove(filter);
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
        private static void CleanRegistrationDataFilter(long chatId)
        {
            List<(long chatId, int code)> registrationDataToRemove = new();
            foreach (var registrationData in RegistrationData)
            {
                if (registrationData.chatId == chatId)
                {
                    registrationDataToRemove.Add(registrationData);
                }
            }
            foreach (var registrationData in registrationDataToRemove)
            {
                RegistrationData.Remove(registrationData);
            }
        }
    }
}
