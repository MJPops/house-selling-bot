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
        private static string Token { get; set; } = "";//TODO - insert correct token
        private static TelegramBotClient client;
        private static List<(long chatId, int houseId, string attribute)> RedactionData = new();
        private static List<(long chatId, string filterName)> UsersFilters = new();
        private static List<(long chatId, int code)> AdminRegistrationData = new();
        private static List<long> RegistrationUsersChatId = new();

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
            Console.WriteLine(callbackMessage);

            HousesRepositore.dBContext = new AppDBContext();
            UsersRepositore.dBContext = new AppDBContext();

            try
            {
                if (callbackMessage == "/start")
                {
                    await Message.EditIntoStartMenuAsync(messageId);
                }
                else if (callbackMessage == "send/start")
                {
                    await Message.SendStartMenuAsync();
                }
                else if (callbackMessage == "МоиИзбранные")
                {
                    await Message.SendFavoriteHousesAsync();
                }
                else if (callbackMessage == "ВсеДома")
                {
                    await Message.EditIntoAllHousesAsync(messageId);
                }
                else if (callbackMessage == "Фильтры")
                {
                    await Message.EditIntoFiltersMenuAsync(messageId);
                }
                else if (callbackMessage == "МоиФильтры")
                {
                    await Message.EditIntoUsersFiltersAsync(chatId, messageId);
                }
                else if (callbackMessage == "О нас")
                {
                    await Message.EditIntoAboutUsAsync(messageId);
                }
                else if (callbackMessage == "ДомаПоФильтрам")
                {
                    try
                    {
                        await Message.EditIntoHousesForUserAsync(messageId);
                    }
                    catch (NotFoundException)
                    {
                        await Message.EditIntoNotFoundMessageAsync(messageId);
                    }
                }

                else if (callbackMessage == "ПоТипуДома")
                {
                    await Task.Run(() => CleanUserFilter(chatId));
                    await Message.EditIntoTypeListAsync(messageId);
                    UsersFilters.Add((chatId, "ПоТипуДома"));
                }
                else if (callbackMessage == "ПоМетро")
                {
                    await Task.Run(() => CleanUserFilter(chatId));
                    await Message.EditIntoMetroListAsync(messageId);
                    UsersFilters.Add((chatId, "ПоМетро"));
                }
                else if (callbackMessage == "ПоРайону")
                {
                    await Task.Run(() => CleanUserFilter(chatId));
                    await Message.EditIntoDistrictsListAsync(messageId);
                    UsersFilters.Add((chatId, "Район"));
                }
                else if (callbackMessage == "ПоКомнатам")
                {
                    await Task.Run(() => CleanUserFilter(chatId));
                    await Message.EditIntoRoomsNumberListAsync(messageId);
                    UsersFilters.Add((chatId, "Комнаты"));
                }
                else if (callbackMessage == "ПоЦене")
                {
                    await Message.EditIntoPriceFilterMenuAsync(messageId);
                }
                else if (callbackMessage == "ЦенаВерх")
                {
                    await Task.Run(() => CleanUserFilter(chatId));
                    await Message.EditIntoSubmitInputRequest("цены", messageId);
                    UsersFilters.Add((chatId, "ЦенаВерх"));
                }
                else if (callbackMessage == "ЦенаНиз")
                {
                    await Task.Run(() => CleanUserFilter(chatId));
                    await Message.EditIntoSubmitInputRequest("цены", messageId);
                    UsersFilters.Add((chatId, "ЦенаНиз"));
                }
                else if (callbackMessage == "ПоМетражу")
                {
                    await Message.EditIntoFootageFilterMenuAsync(messageId);
                }
                else if (callbackMessage == "МетражВерх")
                {
                    await Task.Run(() => CleanUserFilter(chatId));
                    await Message.EditIntoSubmitInputRequest("метража", messageId);
                    UsersFilters.Add((chatId, "МетражВерх"));
                }
                else if (callbackMessage == "МетражНиз")
                {
                    await Task.Run(() => CleanUserFilter(chatId));
                    await Message.EditIntoSubmitInputRequest("метража", messageId);
                    UsersFilters.Add((chatId, "МетражНиз"));
                }
                else if (callbackMessage == "ОчиститьФильтры")
                {
                    await UsersRepositore.ClearUserFiltersAsync(chatId);
                    await Message.EditIntoUsersFiltersAsync(chatId, messageId);
                }

                else if (callbackMessage == "ДобавитьДом")
                {
                    try
                    {
                        House newHouse = new() { Description = "Новый дом" };
                        await HousesRepositore.AddHouseAsync(newHouse);
                        await Message.SendNewHouseDesignAsync(newHouse);
                    }
                    catch { }
                }

                else
                {
                    try
                    {
                        if (callbackMessage.Substring(0, 4) == "Цена")
                        {
                            await Task.Run(() => CleanRedactionDataFilter(chatId));
                            await Message.EditIntoSubmitInputRequest("цену", messageId);
                            RedactionData.Add((chatId, Convert.ToInt32(callbackMessage.Substring(4)), "Цена"));
                        }
                        else if (callbackMessage.Substring(0, 5) == "Район")
                        {
                            await Task.Run(() => CleanRedactionDataFilter(chatId));
                            await Message.EditIntoSubmitInputRequest("район", messageId);
                            RedactionData.Add((chatId, Convert.ToInt32(callbackMessage.Substring(5)), "Район"));
                        }
                        else if (callbackMessage.Substring(0, 5) == "Метро")
                        {
                            await Task.Run(() => CleanRedactionDataFilter(chatId));
                            await Message.EditIntoSubmitInputRequest("Метро", messageId);
                            RedactionData.Add((chatId, Convert.ToInt32(callbackMessage.Substring(5)), "Метро"));
                        }
                        else if (callbackMessage.Substring(0, 5) == "Номер")
                        {
                            await Task.Run(() => CleanRedactionDataFilter(chatId));
                            await Message.EditIntoSubmitInputRequest("Номер", messageId);
                            RedactionData.Add((chatId, Convert.ToInt32(callbackMessage.Substring(5)), "Номер"));
                        }
                        else if (callbackMessage.Substring(0, 6) == "Ссылка")
                        {
                            await Task.Run(() => CleanRedactionDataFilter(chatId));
                            await Message.EditIntoSubmitInputRequest("ссылку на сайт", messageId);
                            RedactionData.Add((chatId, Convert.ToInt32(callbackMessage.Substring(6)), "Ссылка"));
                        }
                        else if (callbackMessage.Substring(0, 6) == "Метраж")
                        {
                            await Task.Run(() => CleanRedactionDataFilter(chatId));
                            await Message.EditIntoSubmitInputRequest("метраж", messageId);
                            RedactionData.Add((chatId, Convert.ToInt32(callbackMessage.Substring(6)), "Метраж"));
                        }
                        else if (callbackMessage.Substring(0, 7) == "ТипДома")
                        {
                            await Task.Run(() => CleanRedactionDataFilter(chatId));
                            await Message.EditIntoSubmitInputRequest("тип дома", messageId);
                            RedactionData.Add((chatId, Convert.ToInt32(callbackMessage.Substring(7)), "ТипДома"));
                        }
                        else if (callbackMessage.Substring(0, 7) == "Комнаты")
                        {
                            await Task.Run(() => CleanRedactionDataFilter(chatId));
                            await Message.EditIntoSubmitInputRequest("количество комнат", messageId);
                            RedactionData.Add((chatId, Convert.ToInt32(callbackMessage.Substring(7)), "Комнаты"));
                        }
                        else if (callbackMessage.Substring(0, 8) == "Описание")
                        {
                            await Task.Run(() => CleanRedactionDataFilter(chatId));
                            await Message.EditIntoSubmitInputRequest("описание к дому", messageId);
                            RedactionData.Add((chatId, Convert.ToInt32(callbackMessage.Substring(8)), "Описание"));
                        }
                        else if (callbackMessage.Substring(0, 9) == "Избранное")
                        {
                            await UsersRepositore.AddFavoriteHouseToUserAsync(chatId,
                                Convert.ToInt32(callbackMessage.Substring(9)));
                            await Message.SendNotificationFavoriteIsAddAsync();
                        }
                        else if (callbackMessage.Substring(0, 10) == "УдалитьДом")
                        {
                            await HousesRepositore.RemoveHouseAsync(Convert.ToInt32(callbackMessage.Substring(10)));
                            await Message.SendHouseIsDeleted();
                        }
                        else if (callbackMessage.Substring(0, 11) == "Изображение")
                        {
                            await Task.Run(() => CleanRedactionDataFilter(chatId));
                            await Message.EditIntoSubmitInputRequest("ссылку на изображение", messageId);
                            RedactionData.Add((chatId, Convert.ToInt32(callbackMessage.Substring(11)), "Изображение"));
                        }
                        else if (callbackMessage.Substring(0, 13) == "УдалитьАдмина")
                        {
                            await UsersRepositore.
                                RemoveUserByChatIdAsync(Convert.ToInt32(callbackMessage.Substring(13)));
                            await Message.EditIntoAdminsRedactionMenuAsync(messageId);
                        }
                        else if (callbackMessage.Substring(0, 18) == "РедактированиеДома")
                        {
                            await Message.SendHouseRedactionMenuAsync(Convert.ToInt32(callbackMessage.Substring(18)));
                        }
                        else if (callbackMessage.Substring(0, 19) == "УдалитьИзИзбранного")
                        {
                            await UsersRepositore.RemoveFromFavoritHousesAsync(chatId,
                                Convert.ToInt32(callbackMessage.Substring(19)));
                            await Message.SendNotificationFavoriteIsRemoveAsync();
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
                                            await Message.EditIntoUsersFiltersAsync(chatId, messageId);
                                        }
                                        else if (filterData.filterName == "Комнаты")
                                        {
                                            try
                                            {
                                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                                user.HouseRoomsNumbe = Convert.ToInt32(callbackMessage);
                                                await UsersRepositore.UpdateUserAsync(user);
                                                await Message.EditIntoUsersFiltersAsync(chatId, messageId);
                                            }
                                            catch (FormatException)
                                            {
                                                await Message.EditIntoNotFoundMessageAsync(messageId);
                                            }
                                        }
                                        else if (filterData.filterName == "ПоТипуДома")
                                        {
                                            var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                            user.HouseType = callbackMessage;
                                            await UsersRepositore.UpdateUserAsync(user);
                                            await Message.EditIntoUsersFiltersAsync(chatId, messageId);
                                        }
                                        else if (filterData.filterName == "ПоМетро")
                                        {
                                            var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                            user.HouseMetro = callbackMessage;
                                            await UsersRepositore.UpdateUserAsync(user);
                                            await Message.EditIntoUsersFiltersAsync(chatId, messageId);
                                        }
                                    }
                                    catch (NotFoundException)
                                    {
                                        await Message.EditIntoNotFoundMessageAsync(messageId);
                                    }

                                    FiltersToRemove.Add(filterData);
                                }
                                else if (filterData.chatId == chatId)
                                {
                                    try
                                    {
                                        if (filterData.filterName == "Район")
                                        {
                                            await Message.EditIntoHousesByDistrictAsync(callbackMessage, messageId);
                                        }
                                        else if (filterData.filterName == "Комнаты")
                                        {
                                            await Message.EditIntoHousesByRoomsNumberAsync(Convert.ToInt32(callbackMessage),
                                                messageId);
                                        }
                                        else if (filterData.filterName == "ПоТипуДома")
                                        {
                                            await Message.EditIntoHousesByTypeAsync(callbackMessage, messageId);
                                        }
                                        else if (filterData.filterName == "ПоМетро")
                                        {
                                            await Message.EditIntoHousesMetroAsync(callbackMessage, messageId);
                                        }
                                    }
                                    catch (FormatException)
                                    {
                                        await Message.EditIntoNotFoundMessageAsync(messageId);
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
                    catch
                    {
                        if (UsersFilters.Any())
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
                                            await Message.EditIntoUsersFiltersAsync(chatId, messageId);
                                        }
                                        else if (filterData.filterName == "Комнаты")
                                        {
                                            try
                                            {
                                                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                                user.HouseRoomsNumbe = Convert.ToInt32(callbackMessage);
                                                await UsersRepositore.UpdateUserAsync(user);
                                                await Message.EditIntoUsersFiltersAsync(chatId, messageId);
                                            }
                                            catch (FormatException)
                                            {
                                                await Message.EditIntoNotFoundMessageAsync(messageId);
                                            }
                                        }
                                        else if (filterData.filterName == "ПоТипуДома")
                                        {
                                            var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                            user.HouseType = callbackMessage;
                                            await UsersRepositore.UpdateUserAsync(user);
                                            await Message.EditIntoUsersFiltersAsync(chatId, messageId);
                                        }
                                        else if (filterData.filterName == "ПоМетро")
                                        {
                                            var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                            user.HouseMetro = callbackMessage;
                                            await UsersRepositore.UpdateUserAsync(user);
                                            await Message.EditIntoUsersFiltersAsync(chatId, messageId);
                                        }
                                    }
                                    catch (NotFoundException)
                                    {
                                        await Message.EditIntoNotFoundMessageAsync(messageId);
                                    }

                                    FiltersToRemove.Add(filterData);
                                }
                                else if (filterData.chatId == chatId)
                                {
                                    try
                                    {
                                        if (filterData.filterName == "Район")
                                        {
                                            await Message.EditIntoHousesByDistrictAsync(callbackMessage, messageId);
                                        }
                                        else if (filterData.filterName == "Комнаты")
                                        {
                                            await Message.EditIntoHousesByRoomsNumberAsync(Convert.ToInt32(callbackMessage),
                                                messageId);
                                        }
                                        else if (filterData.filterName == "ПоТипуДома")
                                        {
                                            await Message.EditIntoHousesByTypeAsync(callbackMessage, messageId);
                                        }
                                        else if (filterData.filterName == "ПоМетро")
                                        {
                                            await Message.EditIntoHousesMetroAsync(callbackMessage, messageId);
                                        }
                                    }
                                    catch (FormatException)
                                    {
                                        await Message.EditIntoNotFoundMessageAsync(messageId);
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
                }
            }
            catch { }
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
            Console.WriteLine(inputMessage);

            HousesRepositore.dBContext = new AppDBContext();
            UsersRepositore.dBContext = new AppDBContext();

            try
            {
                if (inputMessage == "/start")
                {
                    await Message.SendStartMenuAsync();
                }
                else if (inputMessage == "Добавить директора")
                {
                    try
                    {
                        UsersRepositore.GetDirectorChatId();
                    }
                    catch
                    {
                        try
                        {
                            await UsersRepositore.AddUserAsync(new User
                            {
                                ChatId = 828959058,
                                Name = "Сергей",
                                Role = "director"
                            });
                        }
                        catch
                        {
                            var user = await UsersRepositore.GetUserByChatIdAsync(828959058);
                            user.Role = "director";
                            await UsersRepositore.UpdateUserAsync(user);
                        }
                    }
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
                            await Message.SendToAdminRegistrationCodeAsync(code, user.Name);
                            AdminRegistrationData.Add((chatId, code));
                        }
                        catch
                        {
                            await Message.SendNotRegistredAsync();
                        }
                    }
                }
                else if (RedactionData.Any()
                    && (await UsersRepositore.UserIsAdminAsync(chatId)
                    || await UsersRepositore.UserIsDirectorAsync(chatId)))
                {
                    List<(long, int, string)> DataToRemove = new();
                    foreach (var data in RedactionData)
                    {
                        if (data.chatId == chatId)
                        {
                            try
                            {
                                if (data.attribute == "Изображение")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.PicturePath = inputMessage;
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                                else if (data.attribute == "Ссылка")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.WebPath = inputMessage;
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                                else if (data.attribute == "Описание")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.Description = inputMessage;
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                                else if (data.attribute == "Район")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.District = inputMessage;
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                                else if (data.attribute == "ТипДома")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.Type = inputMessage;
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                                else if (data.attribute == "Метро")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.Metro = inputMessage;
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                                else if (data.attribute == "Метро")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.Metro = inputMessage;
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                                else if (data.attribute == "Комнаты")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.RoomsNumber = Convert.ToInt32(inputMessage);
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                                else if (data.attribute == "Цена")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.Price = Convert.ToInt32(inputMessage); ;
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                                else if (data.attribute == "Метраж")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.Footage
                                        = Convert.ToInt32(inputMessage);
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                                else if (data.attribute == "Номер")
                                {
                                    var house = await HousesRepositore.GetHouseByIdAsync(data.houseId);
                                    house.AdminPhone = inputMessage;
                                    await Message.SendNewHouseDesignAsync(house);
                                    await HousesRepositore.UpdateHouseAsync(house);
                                }
                            }
                            catch (FormatException)
                            {
                                await Message.SendNotFoundMessageAsync();
                            }

                            DataToRemove.Add(data);
                        }
                    }
                    foreach (var data in DataToRemove)
                    {
                        RedactionData.Remove(data);
                    }
                }
                else if (RegistrationUsersChatId.Any() && inputMessage.Substring(0, 1) == "8")
                {
                    if (RegistrationUsersChatId.Contains(chatId))
                    {
                        RegistrationUsersChatId.Remove(chatId);
                        await UsersRepositore.AddUserAsync(new User
                        {
                            ChatId = chatId,
                            Phone = inputMessage.Substring(0, 11),
                            Name = inputMessage.Substring(12)
                        });
                        await Message.SendRegistrationСompletedAsync();
                        await Message.SendStartMenuAsync();
                    }
                }
                else if (AdminRegistrationData.Any() && inputMessage.Substring(0, 3) == "Код")
                {
                    List<(long, int)> RegistrationDataToRemove = new();
                    foreach (var registrationData in AdminRegistrationData)
                    {
                        try
                        {
                            if (registrationData.chatId == chatId
                                && registrationData.code == Convert.ToInt32(inputMessage.Substring(3)))
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
                        AdminRegistrationData.Remove(filter);
                    }
                }
                else if (inputMessage.Substring(0, 11) == "Регистрация")
                {
                    try
                    {
                        RegistrationUsersChatId.Remove(chatId);
                        await Message.SendPhoneAndNameRequestAsync();
                        RegistrationUsersChatId.Add(chatId);
                    }
                    catch (AlreadyContainException)
                    {
                        await Message.SendAlreadyRegisterAsync();
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
                                    await Message.SendUsersFiltersAsync(chatId);
                                }
                                else if (filterData.filterName == "ЦенаНиз")
                                {
                                    var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                    user.LowerPrice = Convert.ToInt32(inputMessage);
                                    await UsersRepositore.UpdateUserAsync(user);
                                    await Message.SendUsersFiltersAsync(chatId);
                                }
                                else if (filterData.filterName == "МетражВерх")
                                {
                                    var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                    user.HightFootage = Convert.ToInt32(inputMessage);
                                    await UsersRepositore.UpdateUserAsync(user);
                                    await Message.SendUsersFiltersAsync(chatId);
                                }
                                else if (filterData.filterName == "МетражНиз")
                                {
                                    var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                    user.LowerFootage = Convert.ToInt32(inputMessage);
                                    await UsersRepositore.UpdateUserAsync(user);
                                    await Message.SendUsersFiltersAsync(chatId);
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
                                    await Message.SendHousesWhthLowerPriceAsync(Convert.ToInt32(inputMessage));
                                }
                                else if (filterData.filterName == "ЦенаНиз")
                                {
                                    await Message.SendHousesWhthHigerPriceAsync(Convert.ToInt32(inputMessage));
                                }
                                else if (filterData.filterName == "МетражВерх")
                                {
                                    await Message.SendHousesWhthLowerFootageAsync(Convert.ToInt32(inputMessage));
                                }
                                else if (filterData.filterName == "МетражНиз")
                                {
                                    await Message.SendHousesWhthHigerFootageAsync(Convert.ToInt32(inputMessage));
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
            }
            catch
            {
                if (UsersFilters.Any())
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
                                    await Message.SendUsersFiltersAsync(chatId);
                                }
                                else if (filterData.filterName == "ЦенаНиз")
                                {
                                    var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                    user.LowerPrice = Convert.ToInt32(inputMessage);
                                    await UsersRepositore.UpdateUserAsync(user);
                                    await Message.SendUsersFiltersAsync(chatId);
                                }
                                else if (filterData.filterName == "МетражВерх")
                                {
                                    var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                    user.HightFootage = Convert.ToInt32(inputMessage);
                                    await UsersRepositore.UpdateUserAsync(user);
                                    await Message.SendUsersFiltersAsync(chatId);
                                }
                                else if (filterData.filterName == "МетражНиз")
                                {
                                    var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                                    user.LowerFootage = Convert.ToInt32(inputMessage);
                                    await UsersRepositore.UpdateUserAsync(user);
                                    await Message.SendUsersFiltersAsync(chatId);
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
                                    await Message.SendHousesWhthLowerPriceAsync(Convert.ToInt32(inputMessage));
                                }
                                else if (filterData.filterName == "ЦенаНиз")
                                {
                                    await Message.SendHousesWhthHigerPriceAsync(Convert.ToInt32(inputMessage));
                                }
                                else if (filterData.filterName == "МетражВерх")
                                {
                                    await Message.SendHousesWhthLowerFootageAsync(Convert.ToInt32(inputMessage));
                                }
                                else if (filterData.filterName == "МетражНиз")
                                {
                                    await Message.SendHousesWhthHigerFootageAsync(Convert.ToInt32(inputMessage));
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
            }
        }
        private static void CleanRegistrationDataFilter(long chatId)
        {
            List<(long chatId, int code)> registrationDataToRemove = new();
            foreach (var registrationData in AdminRegistrationData)
            {
                if (registrationData.chatId == chatId)
                {
                    registrationDataToRemove.Add(registrationData);
                }
            }
            foreach (var registrationData in registrationDataToRemove)
            {
                AdminRegistrationData.Remove(registrationData);
            }
        }
        private static void CleanRedactionDataFilter(long chatId)
        {
            List<(long, int, string)> redactionDataToRemove = new();
            foreach (var data in RedactionData)
            {
                if (data.chatId == chatId)
                {
                    redactionDataToRemove.Add(data);
                }
            }
            foreach (var registrationData in redactionDataToRemove)
            {
                RedactionData.Remove(registrationData);
            }
        }
    }
}
