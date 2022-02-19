using HouseSellingBot.Models;
using HouseSellingBot.PersonalExceptions;
using HouseSellingBot.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;

namespace HouseSellingBot.UI
{
    public class Messages
    {
        private long ChatId { get; set; }
        private TelegramBotClient Client { get; set; }

        [Obsolete]
        public Messages(TelegramBotClient client, long chatId)
        {
            Client = client;
            ChatId = chatId;
        }

        #region Menus
        public async Task SendStartMenuAsync()
        {
            if (await UsersRepositore.UserIsRegisteredAsync(ChatId))
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(ChatId);
                await Client.SendTextMessageAsync(ChatId, $"Здравствуйте, {user.Name}, рад Вас видеть.",
                replyMarkup: await Buttons.StartAsync(ChatId));
            }
            else
            {
                await Client.SendTextMessageAsync(ChatId, "Привет, я бот канала Элитной недвижимости Chase real estate services, и я помогу Вам найти квартиру мечты!\n" +
                                              "Для начала нужно зарегистрироваться, иначе не получится пользоваться полным функционалом!\n" +
                                              "Чтобы зарегистрироваться отправьте сообщение\n" +
                                              "\"Регистрация\".",
                    replyMarkup: await Buttons.StartAsync(ChatId));
            }
        }
        public async Task SendUsersFiltersAsync()
        {
            try
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(ChatId);
                await Client.SendTextMessageAsync(ChatId,
                    $"Вот выши фильтры:\n" +
                    $"Тип дома: {user.HouseType}\n" +
                    $"Метро: {user.HouseMetro}\n" +
                    $"Район: {user.HouseDistrict}\n" +
                    $"Число комнат: {user.HouseRoomsNumbe}\n" +
                    $"Цена: {user.LowerPrice ?? 00} - {user.HightPrice ?? 00}\n" +
                    $"Метраж: {user.LowerFootage ?? 00} - {user.HightFootage ?? 00}\n",
                    replyMarkup: Buttons.BackToFilters());
            }
            catch (NotFoundException)
            {
                await Client.SendTextMessageAsync(ChatId,
                    "Вы не зарегистрированны",
                    replyMarkup: Buttons.BackToFilters());
            }
        }
        public async Task EditIntoAboutUsAsync(int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, messageId,
                "Привет, я бот канала Элитной недвижимости Chase real estate services," +
                "и я помогу тебе найти квартиру мечты!\n" +
                                              "Будут вопросы, звони:");
            await Client.SendContactAsync(ChatId,
                phoneNumber: "+79856986633", //TODO - insert telephon
                firstName: "Сергей",
                lastName: "Малахов",
                replyMarkup: Buttons.StartAndLink());
        }
        public async Task EditIntoStartMenuAsync(int messageId)
        {
            if (await UsersRepositore.UserIsRegisteredAsync(ChatId))
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(ChatId);
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    $"Здравствуйте, {user.Name}, рад Вас видеть.",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                await Buttons.StartAsync(ChatId));
            }
            else
            {
                await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Привет, я бот канала Элитной недвижимости Chase real estate services, и я помогу Вам найти квартиру мечты!\n" +
                                              "Для начала нужно зарегистрироваться, иначе не получится пользоваться полным функционалом!\n" +
                                              "Чтобы зарегистрироваться отправьте сообщение\n" +
                                              "\"Регистрация\".",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                await Buttons.StartAsync(ChatId));
            }
        }
        public async Task EditIntoFiltersMenuAsync(int messageId)
        {
            if (await UsersRepositore.UserIsRegisteredAsync(ChatId))
            {
                await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Ниже указаны все доступные фильтры.",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                await Buttons.FiltersMenuForUserAsync(ChatId));
            }
            else
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Ниже указаны все доступные фильтры.",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                    await Buttons.FiltersMenuForUserAsync(ChatId));
            }
        }
        public async Task EditIntoUsersFiltersAsync(int messageId)
        {
            try
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(ChatId);
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    $"Вот ваши фильтры:\n" +
                    $"Тип дома: {user.HouseType}\n" +
                    $"Метро: {user.HouseMetro}\n" +
                    $"Район: {user.HouseDistrict}\n" +
                    $"Число комнат: {user.HouseRoomsNumbe}\n" +
                    $"Цена: {user.LowerPrice ?? 00} - {user.HightPrice ?? 00}\n" +
                    $"Метраж: {user.LowerFootage ?? 00} - {user.HightFootage ?? 00}\n",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToFilters());
            }
            catch (NotFoundException)
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Вы не зарегистрированны",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToFilters());
            }
        }
        #endregion

        #region Departures of houses without additional parameters
        public async Task SendFavoriteHousesAsync()
        {
            var user = await UsersRepositore.GetUserByChatIdAsync(ChatId);
            if (user.FavoriteHouses.Count == 0)
            {
                await Client.SendTextMessageAsync(ChatId, "Ваш список избранного пуст", replyMarkup: Buttons.BackToStart());
            }
            else
            {
                await Client.SendTextMessageAsync(ChatId, "Ваши дома, которые были добавлены в избранное:");
                await SendHousesListAsync(user.FavoriteHouses);
            }
        }
        public async Task EditIntoAllHousesAsync(int messageId)
        {
            try
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Все, что доступно сейчас на данный момент:");
                await SendHousesListAsync(await HousesRepositore.GetAllHousesAsync());
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync();
            }
        }
        public async Task EditIntoHousesForUserAsync(int messageId)
        {
            try
            {
                await Client.EditMessageTextAsync(ChatId, messageId, "Дома, соответствующие вашим фильтрам:");
                await SendHousesListAsync(await UsersRepositore.GetHousesWhthCustomFiltersAsync(ChatId));
            }
            catch (NoHomesWithTheseFeaturesException)
            {
                await EditIntoNotFoundMessageAsync(messageId);
            }
        }
        #endregion

        #region Departure of houses by filters
        public async Task SendHousesWhthHigerPriceAsync(float price)
        {
            await Client.SendTextMessageAsync(ChatId, $"Все доступные помещения с ценой выше " +
                $"{price}");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHouseWithHigherPrice(price));
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync();
            }
        }
        public async Task SendHousesWhthHigerFootageAsync(float footage)
        {
            await Client.SendTextMessageAsync(ChatId, $"Все доступные помещения с метражом больше " +
                $"{footage}");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHouseWithHigherFootage(footage));
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync();
            }
        }
        public async Task SendHousesWhthLowerPriceAsync(float price)
        {
            await Client.SendTextMessageAsync(ChatId, $"Все доступные помещения с ценой ниже " +
                $"{price}");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHouseWithLowerPrice(price));
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync();
            }
        }
        public async Task SendHousesWhthLowerFootageAsync(float footage)
        {
            await Client.SendTextMessageAsync(ChatId, $"Все доступные помещения с метражом ниже " +
                $"{footage}");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHouseWithLowerFootage(footage));
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync();
            }
        }
        public async Task EditIntoHousesByTypeAsync(string type, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, messageId, $"Вот все доступные {type} на данный момент:");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHousesByTypeAsync(type));
            }
            catch (NotFoundException)
            {
                await EditIntoNotFoundMessageAsync(messageId);
            }
        }
        public async Task EditIntoHousesMetroAsync(string metro, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, messageId, $"Станции метро, рядом с которыми сейчас доступны квартиры:");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHousesByMetroAsync(metro));
            }
            catch
            {
                await EditIntoNotFoundMessageAsync(messageId);
            }
        }
        public async Task EditIntoHousesByDistrictAsync(string type, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                $"Доступные помещение на данный момент в {type}");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHouseByDistrictAsync(type));
            }
            catch (NotFoundException)
            {
                await EditIntoNotFoundMessageAsync(messageId);
            }
        }
        public async Task EditIntoHousesByRoomsNumberAsync(int roomsNumber, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, messageId, $"Доступные {roomsNumber}-х комнатные" +
                $" помещения на данный момент:");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHousesByRoomsNumberAsync(roomsNumber));
            }
            catch (NotFoundException)
            {
                await EditIntoNotFoundMessageAsync(messageId);
            }
        }
        #endregion

        #region Filter selection sheets
        public async Task EditIntoTypeListAsync(int messageId)
        {
            try
            {
                var types = await GetAllTypesAsync();
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Все доступные варианты:",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                Buttons.FiltersList(types));
            }
            catch (NotFoundException)
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Типы жил/площади пока не добавлены",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToFilters());
            }
        }
        public async Task EditIntoDistrictsListAsync(int messageId)
        {
            try
            {
                var districts = await GetAllDistrictsAsync();
                await Client.EditMessageTextAsync(ChatId, messageId, "Все доступные районы:",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                Buttons.FiltersList(districts));
            }
            catch (NotFoundException)
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Районы пока не добавлены",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToFilters());
            }
        }
        public async Task EditIntoMetroListAsync(int messageId)
        {
            try
            {
                var metro = await GetAllMetroAsync();
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Все доступные станции метро:",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.FiltersList(metro));
            }
            catch (NotFoundException)
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Станции метро пока не добавлены",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToFilters());
            }
        }
        public async Task EditIntoRoomsNumberListAsync(int messageId)
        {
            try
            {
                var rooms = await GetAllRoomsNumberAsync();
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Все доступные варианты квартир по количеству комнат:",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.FiltersList(rooms));
            }
            catch (NotFoundException)
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Количество комнат пока не добавлено",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToFilters());
            }
        }
        public async Task EditIntoPriceFilterMenuAsync(int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Выберите вид ограничения цены",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.PriceFilters());
        }
        public async Task EditIntoFootageFilterMenuAsync(int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Выберите вид ограничения метража",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.FootageFilters());
        }
        #endregion

        #region Warnings and Exceptions
        public async Task SendNotFoundMessageAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Дома с такими параметрами не обнаружены.",
                replyMarkup: Buttons.BackToStart());
        }
        public async Task SendHouseIsDeleted()
        {
            await Client.SendTextMessageAsync(ChatId, "Удалено");
        }
        public async Task SendNotRegistredAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Вы не зарегистрированны");
        }
        public async Task SendAlreadyRegisterAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Вы уже зарегистрированны.\n" +
                "Если хотите сменить имя, введите \"Удалить меня\" и перерегистрируйтесь.");
        }
        public async Task SendAlreadyAdminAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Вы уже имеете права администратора");
        }
        public async Task SendCodeNotWorkAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Код не подходит");
        }
        public async Task SendNotificationFavoriteIsAddAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Квартира добавлена в список избранного.");
        }
        public async Task SendNotificationFavoriteIsRemoveAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Квартира удалена из списка избранного.");
        }
        public async Task EditIntoNotFoundMessageAsync(int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Дома с такими параметрами не обнаружены.",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToStart());
        }
        public async Task EditIntoSubmitInputRequest(string request, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                $"Введите {request}");
        }
        #endregion

        #region Registration messages
        public async Task SendPhoneAndNameRequestAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Введите ваш номер телефона и имя через пробел\n\n" +
                "Пример: 89991119911 Иван");
        }
        public async Task SendRegistrationСompletedAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Регистрация успешно завершена!");
        }
        #endregion

        #region Administrative Information
        public async Task SendAdminsRedactionMenuAsync()
        {
            try
            {
                await Client.SendTextMessageAsync(ChatId,
                    "Нажмите на кнопку для того, чтобы удалить соответствующего администратора",
                    replyMarkup: Buttons.AdminsListAsync(await UsersRepositore.GetAllAdminAsync()));
            }
            catch (NotFoundException)
            {
                await Client.SendTextMessageAsync(ChatId,
                    "Администраторы не добавлены");
            }
        }
        public async Task SendToAdminRegistrationCodeAsync(int code, string userName)
        {
            await Client.SendTextMessageAsync(await UsersRepositore.GetDirectorChatIdAsync(),
                $"Пользователь {userName}, просит предоставить доступ администратора.\n" +
                $"Для предоставления доступа администратора сообщите ему код: Код{code}");
            await Client.SendTextMessageAsync(ChatId, "Введите код в следующем формате: Код1111. Код направлен директору");
        }
        public async Task SendYouAdminAsync(long directorChatId)
        {
            await Client.SendTextMessageAsync(directorChatId,
                "Админ зарегистрирован");
            await Client.SendTextMessageAsync(ChatId,
                "Вы получили роль администратора");
        }
        public async Task SendHouseRedactionMenuAsync(int houseId)
        {
            await Client.SendTextMessageAsync(ChatId,
                "Выберите изменяемый параметр",
                replyMarkup: Buttons.InRedactionMenu(houseId));
        }
        public async Task SendNewHouseDesignAsync(House house)
        {
            string text = $"Описание: {house.Description}\n" +
                $"Метро: {house.Metro}\n" +
                $"Район: {house.District}\n" +
                $"Метраж: {house.Footage}\n" +
                $"Число комнат: {house.RoomsNumber}\n" +
                $"Цена: {house.Price}₽\n" +
                $"Тип дома: {house.Type}\n" +
                $"Номер телефона: {house.AdminPhone}";
            try
            {
                await Client.SendPhotoAsync(ChatId,
                    house.PicturePath,
                    caption: text,
                    replyMarkup: Buttons.RedactionMenuForHouses(house.WebPath, house.Id));
            }
            catch
            {
                try
                {
                    await Client.SendPhotoAsync(ChatId,
                        house.PicturePath,
                        caption: text,
                        replyMarkup: Buttons.RedactionMenuForHouses(house.Id));
                }
                catch
                {
                    try
                    {
                        await Client.SendTextMessageAsync(ChatId,
                            text,
                            replyMarkup: Buttons.RedactionMenuForHouses(house.WebPath, house.Id));
                    }
                    catch
                    {
                        await Client.SendTextMessageAsync(ChatId,
                            text,
                            replyMarkup: Buttons.RedactionMenuForHouses(house.Id));
                    }
                }
            }
            await SendBackToStart();
        }
        public async Task EditIntoAdminsRedactionMenuAsync(int messageId)
        {
            try
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Нажмите на кнопку для того, чтобы удалить соответствующего администратора",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                    Buttons.AdminsListAsync(await UsersRepositore.GetAllAdminAsync()));
            }
            catch (NotFoundException)
            {
                await Client.SendTextMessageAsync(ChatId,
                    "Администраторы не добавлены");
            }
        }
        #endregion


        private async Task SendBackToStart()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Вернуться на стартовое меню",
                replyMarkup: Buttons.BackToStart());
        }
        private async Task SendOneHouseAsync(House house)
        {
            string text = $"{house.Description ?? "Скоро тут будет описание😄"}\n\n" +
                $"По пунктам о квартире:\n" +
                $"◽ Данная квартиры распологается в {house.District ?? "районе который мы укажем чуть позже"}" +
                $", недалеко от метро {house.Metro ?? "которое мы постараемся указать как можно скорее"}\n" +
                $"◽ Площадь квартиры составляет {house.Footage}м², а число комнат - {house.RoomsNumber}\n" +
                $"◽ Цена квартиры составляет {house.Price}₽\n\n" +
                $"📱 По всем вопросам вы можете обратьться по телефону: {house.AdminPhone ?? "+79856986633"}";
            try
            {
                await Client.SendPhotoAsync(ChatId,
                    house.PicturePath,
                    caption: text,
                    replyMarkup: await Buttons.LinkAndAddToFavoritAsync(house.WebPath, ChatId, house.Id));
            }
            catch
            {
                try
                {
                    await Client.SendPhotoAsync(ChatId,
                        house.PicturePath,
                        caption: text,
                        replyMarkup: await Buttons.AddToFavorit(ChatId, house.Id));
                }
                catch
                {
                    try
                    {
                        await Client.SendTextMessageAsync(ChatId, text,
                            replyMarkup: await Buttons.LinkAndAddToFavoritAsync(house.WebPath, ChatId, house.Id));
                    }
                    catch
                    {
                        await Client.SendTextMessageAsync(ChatId,
                            text,
                            replyMarkup: await Buttons.AddToFavorit(ChatId, house.Id));
                    }
                }
            }
        }
        private async Task SendOneHouseForAdminAsync(House house)
        {
            string text = $"Описание: {house.Description}\n" +
                $"Метро: {house.Metro}\n" +
                $"Район: {house.District}\n" +
                $"Метраж: {house.Footage}\n" +
                $"Число комнат: {house.RoomsNumber}\n" +
                $"Цена: {house.Price}₽\n" +
                $"Тип дома: {house.Type}\n" +
                $"Номер телефона: {house.AdminPhone}";
            try
            {
                await Client.SendPhotoAsync(ChatId,
                    house.PicturePath,
                    caption: text,
                    replyMarkup: Buttons.RedactionMenuForHouses(house.WebPath, house.Id));
            }
            catch
            {
                try
                {
                    await Client.SendPhotoAsync(ChatId,
                        house.PicturePath,
                        caption: text,
                        replyMarkup: Buttons.RedactionMenuForHouses(house.Id));
                }
                catch
                {
                    try
                    {
                        await Client.SendTextMessageAsync(ChatId,
                            text,
                            replyMarkup: Buttons.RedactionMenuForHouses(house.WebPath, house.Id));
                    }
                    catch
                    {
                        await Client.SendTextMessageAsync(ChatId,
                            text,
                            replyMarkup: Buttons.RedactionMenuForHouses(house.Id));
                    }
                }
            }
        }
        private async Task SendHousesListAsync(IEnumerable<House> houses)
        {
            try
            {
                if (await UsersRepositore.UserIsAdminAsync(ChatId) || await UsersRepositore.UserIsDirectorAsync(ChatId))
                {
                    foreach (var item in houses)
                    {
                        await SendOneHouseForAdminAsync(item);
                    }
                }
                else
                {
                    foreach (var item in houses)
                    {
                        await SendOneHouseAsync(item);
                    }
                }
            }
            catch (NotFoundException)
            {
                foreach (var item in houses)
                {
                    await SendOneHouseAsync(item);
                }
            }
            await SendBackToStart();
        }

        private async Task<IEnumerable<string>> GetAllTypesAsync()
        {
            var allTypes = from house in await HousesRepositore.GetAllHousesAsync()
                           where house.Type != null
                           select house.Type;
            allTypes = allTypes.Distinct();
            if (allTypes == null)
            {
                throw new NotFoundException();
            }
            return allTypes;
        }
        private async Task<IEnumerable<string>> GetAllDistrictsAsync()
        {
            var allDistricts = from house in await HousesRepositore.GetAllHousesAsync()
                               where house.District != null
                               select house.District;
            allDistricts = allDistricts.Distinct();
            if (allDistricts == null)
            {
                throw new NotFoundException();
            }
            return allDistricts;
        }
        private async Task<IEnumerable<string>> GetAllMetroAsync()
        {
            var allMetro = from house in await HousesRepositore.GetAllHousesAsync()
                           where house.Metro != null
                           select house.Metro;
            allMetro = allMetro.Distinct();
            if (allMetro == null)
            {
                throw new NotFoundException();
            }
            return allMetro;
        }
        private async Task<IEnumerable<int?>> GetAllRoomsNumberAsync()
        {
            var allRoomsNumber = from house in await HousesRepositore.GetAllHousesAsync()
                                 where house.RoomsNumber != null
                                 select house.RoomsNumber;
            allRoomsNumber = allRoomsNumber.OrderBy(n => n).Distinct();
            if (allRoomsNumber == null)
            {
                throw new NotFoundException();
            }
            return allRoomsNumber;
        }
    }
}
