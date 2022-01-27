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
        private static long ChatId;
        private static TelegramBotClient Client { get; set; }

        [Obsolete]
        public Messages(TelegramBotClient client, long chatId)
        {
            Client = client;
            ChatId = chatId;
        }

        public async Task SendStartMenuAsync()
        {
            if (await UsersRepositore.UserIsRegisteredAsync(ChatId))
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(ChatId);
                await Client.SendTextMessageAsync(ChatId, $"Здравствуйте {user.Name}, рад вас видеть.",
                replyMarkup: await Buttons.StartAsync(ChatId));
            }
            else
            {
                await Client.SendTextMessageAsync(ChatId, "Здравствуйте, рад вас видеть. \n" +
                    "Я являюсь ботом, который поможет вам ознакомиться с домами, доступными к приобретению и аренде.\n\n" +
                    "Выберите интересующий вас пункт меню.",
                    replyMarkup: await Buttons.StartAsync(ChatId));
            }
        }
        public async Task EditStartMenuAsync(int messageId)
        {
            if (await UsersRepositore.UserIsRegisteredAsync(ChatId))
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(ChatId);
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    $"Здравствуйте {user.Name}, рад вас видеть.",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                await Buttons.StartAsync(ChatId));
            }
            else
            {
                await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Здравствуйте, рад вас видеть. \n" +
                "Я являюсь ботом, который поможет вам ознакомиться с домами, доступными к приобретению и аренде.\n\n" +
                "Выберите интересующий вас пункт меню.",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                await Buttons.StartAsync(ChatId));
            }
        }
        public async Task SendAllHousesAsync()
        {
            try
            {
                await Client.SendTextMessageAsync(ChatId, "Вот все доступные на данных момент дома:");
                await SendHousesListAsync(await HousesRepositore.GetAllHousesAsync());
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync();
            }
        }
        public async Task SendFiltersMenuAsync(int messageId)
        {
            if (await UsersRepositore.UserIsRegisteredAsync(ChatId))
            {
                await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Вот доступные фильтры",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                await Buttons.FiltersMenuForUserAsync(ChatId));
            }
            else
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Вот доступные фильтры\n" +
                    "Напоминаем, что незарегистрированный клиент может использовать единовременно только 1 фильтр\n" +
                    "Для того, чтобы зарегистрироваться введите \"Регистрация <Имя>\", где <Имя> заменить на ваше имя.",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                    await Buttons.FiltersMenuForUserAsync(ChatId));
            }
        }
        public async Task SendUsersFiltersAsync(long chatId, int messageId)
        {
            try
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);
                await Client.EditMessageTextAsync(chatId,
                    messageId,
                    $"Вот выши фильтры:\n" +
                    $"Тип дома: {user.HouseType}\n" +
                    $"Метро: {user.HouseMetro}\n" +
                    $"Тип покупки: {user.HouseRentType}\n" +
                    $"Район: {user.HouseDistrict}\n" +
                    $"Число комнат: {user.HouseRoomsNumbe}\n" +
                    $"Цена: {user.LowerPrice ?? 00} - {user.HightPrice ?? 00}\n" +
                    $"Метраж: {user.LowerFootage ?? 00} - {user.HightFootage ?? 00}",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToFilters());
            }
            catch (NotFoundException)
            {
                await Client.EditMessageTextAsync(chatId,
                    messageId,
                    "Вы не зарегистрированны",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToFilters());
            }
        }

        public async Task SendHousesForUserAsync(long ChatId)
        {
            try
            {
                await Client.SendTextMessageAsync(ChatId, "Дома, соответствующие вашим фильтрам:");
                await SendHousesListAsync(await UsersRepositore.GetHousesWhithCustomFiltersAsync(ChatId));
            }
            catch (NoHomesWithTheseFeaturesException)
            {
                await SendNotFoundMessageAsync();
            }
        }
        public async Task SendFavoriteHousesAsync()
        {
            var user = await UsersRepositore.GetUserByChatIdAsync(ChatId);
            await Client.SendTextMessageAsync(ChatId, "Ваши дома, которые были добавлены в избранное:");
            await SendHousesListAsync(user.FavoriteHouses);
        }
        public async Task SendHousesForUserAsync(long ChatId, int messageId)
        {
            try
            {
                await Client.EditMessageTextAsync(ChatId, messageId, "Дома, соответствующие вашим фильтрам:");
                await SendHousesListAsync(await UsersRepositore.GetHousesWhithCustomFiltersAsync(ChatId));
            }
            catch (NoHomesWithTheseFeaturesException)
            {
                await SendNotFoundMessageAsync(messageId);
            }
        }
        public async Task SendHousesByTypeAsync(string type, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, messageId, $"Вот все доступные {type} на данный момент:");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHousesByTypeAsync(type));
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync(messageId);
            }
        }
        public async Task SendHousesMetroAsync(string metro, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, messageId, $"Вот станции метро, рядом с которыми сейчас доступны квартиры:");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHousesByMetroAsync(metro));
            }
            catch
            {
                await SendNotFoundMessageAsync(messageId);
            }
        }
        public async Task SendHousesByRentTypeAsync(string type, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, messageId, $"Вот все доступные помещения на данный момент:");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHousesByRentTypeAsync(type));
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync(messageId);
            }
        }
        public async Task SendHousesByDistrictAsync(string type, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                $"Вот все доступные помещение на данный момент в {type}");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHouseByDistrictAsync(type));
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync(messageId);
            }
        }
        public async Task SendHousesByRoomsNumberAsync(int roomsNumber, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, messageId, $"Вот все доступные {roomsNumber}-х комнатные" +
                $" помещение на данный момент:");
            try
            {
                await SendHousesListAsync(await HousesRepositore.GetHousesByRoomsNumberAsync(roomsNumber));
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync(messageId);
            }
        }
        public async Task SendHousesWhithHigerPriceAsync(float price)
        {
            await Client.SendTextMessageAsync(ChatId, $"Вот все доступные помещения с ценой выше " +
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
        public async Task SendHousesWhithHigerFootageAsync(float footage)
        {
            await Client.SendTextMessageAsync(ChatId, $"Вот все доступные помещения с метражом больше " +
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
        public async Task SendHousesWhithLowerPriceAsync(float price)
        {
            await Client.SendTextMessageAsync(ChatId, $"Вот все доступные помещения с ценой ниже" +
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
        public async Task SendHousesWhithLowerFootageAsync(float footage)
        {
            await Client.SendTextMessageAsync(ChatId, $"Вот все доступные помещения с метражом ниже" +
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

        public async Task SendTypeListAsync(int messageId)
        {
            try
            {
                var types = await GetAllTypesAsync();
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Вот все доступные варианты:",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                Buttons.FiltersList(types));
            }
            catch (NotFoundException)
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Типы жилплощади пока не добавлены",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToFilters());
            }
        }
        public async Task SendRentTypeListAsync(int messageId)
        {
            try
            {
                var rentTypes = await GetAllRentTypesAsync();
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Вот все доступные варианты:",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                Buttons.FiltersList(rentTypes));
            }
            catch (NotFoundException)
            {
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Типы продажи пока не добавлены",
                    replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToFilters());
            }
        }
        public async Task SendDistrictsListAsync(int messageId)
        {
            try
            {
                var districts = await GetAllDistrictsAsync();
                await Client.EditMessageTextAsync(ChatId, messageId, "Вот все доступные районы:",
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
        public async Task SendMetroListAsync(int messageId)
        {
            try
            {
                var metro = await GetAllMetroAsync();
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Вот все доступные станции метро:",
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
        public async Task SendRoomsNumberListAsync(int messageId)
        {
            try
            {
                var rooms = await GetAllRoomsNumberAsync();
                await Client.EditMessageTextAsync(ChatId,
                    messageId,
                    "Вот все доступные варианты квартир по количеству комнат:",
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
        public async Task SendPriceFilterMenuAsync(int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Выберите вид ограничения цены",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.PriceFilters());
        }
        public async Task SendFootageFilterMenuAsync(int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Выберите вид ограничения метража",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.FootageFilters());
        }

        public async Task SendAlreadyRegisterAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Вы уже зарегистрированны.\n" +
                "Если хотите сменить имя, введите \"Удалить меня\" и перерегистрируйтесь.");
        }
        public async Task SendNotFoundMessageAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Дома с такими параметрами не обнаружены.",
                replyMarkup: Buttons.BackToStart());
        }
        public async Task SendNotFoundMessageAsync(int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Дома с такими параметрами не обнаружены.",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.BackToStart());
        }
        public async Task SubmitInputRequest(string request, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                $"Введите {request}");
        }
        public async Task SendNotRegistredAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Вы не зарегистрированны");
        }
        public async Task SendCodeNotWorkAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Код не подходит");
        }

        public async Task SendNotAdminAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Вы не имеете прав администатора, зарегистрируйтесь," +
                " а потом введите \"Запросить права регистратора\"");
        }
        public async Task SendAlreadyAdminAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Вы уже имеете права администратора");
        }
        public async Task SendYouAdminAsync(long directorChatId)
        {
            await Client.SendTextMessageAsync(directorChatId,
                "Админ зарегистрирован");
            await Client.SendTextMessageAsync(ChatId,
                "Вы получили роль администратора");
        }
        public async Task SendAdminRegistrationCodeAsync(int code, string userName)
        {
            await Client.SendTextMessageAsync(await UsersRepositore.GetDirectorChatIdAsync(),
                $"Пользователь {userName}, просит предоставить доступ администратора.\n" +
                $"Для предоставления доступа администратора сообщите ему код: {code}");
            await Client.SendTextMessageAsync(ChatId, "Введите код. Код направлен директору");
        }
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
        public async Task SendAdminsRedactionMenuAsync(int messageId)
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
        public async Task SendHousesListAsync(IEnumerable<House> houses)
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


        private static async Task SendOneHouseAsync(House house)
        {
            string text = $"Описание: {house.Description}\n" +
                $"Метро: {house.Metro}\n" +
                $"Район: {house.District}\n" +
                $"Метраж: {house.Footage}\n" +
                $"Число комнат: {house.RoomsNumber}\n" +
                $"Тип покупки: {house.RentType}\n" +
                $"Цена: {house.Price}₽\n" +
                $"Тип дома: {house.Type}";
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
        private static async Task SendOneHouseForAdminAsync(House house)
        {
            string text = $"Описание: {house.Description}\n" +
                $"Метро: {house.Metro}\n" +
                $"Район: {house.District}\n" +
                $"Метраж: {house.Footage}\n" +
                $"Число комнат: {house.RoomsNumber}\n" +
                $"Тип покупки: {house.RentType}\n" +
                $"Цена: {house.Price}₽\n" +
                $"Тип дома: {house.Type}";
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
        private static async Task SendBackToStart()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Вернуться на стартовое меню",
                replyMarkup: Buttons.BackToStart());
        }
        private static async Task<IEnumerable<string>> GetAllTypesAsync()
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
        private static async Task<IEnumerable<string>> GetAllRentTypesAsync()
        {
            var allRentTypes = from house in await HousesRepositore.GetAllHousesAsync()
                               where house.RentType != null
                               select house.RentType;
            allRentTypes = allRentTypes.Distinct();
            if (allRentTypes == null)
            {
                throw new NotFoundException();
            }
            return allRentTypes;
        }
        private static async Task<IEnumerable<string>> GetAllDistrictsAsync()
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
        private static async Task<IEnumerable<string>> GetAllMetroAsync()
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
        private static async Task<IEnumerable<int?>> GetAllRoomsNumberAsync()
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
