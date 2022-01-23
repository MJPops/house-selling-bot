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
        private static TelegramBotClient Client { get; set; }
        private static long ChatId;

        [Obsolete]
        public Messages(TelegramBotClient client, long chatId)
        {
            Client = client;
            ChatId = chatId;
        }

        public async Task EditStartMenuAsync(int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, 
                messageId,
                "Здравствуйте, рад вас видеть. \n" +
                "Я являюсь ботом, который поможет вам ознакомиться с домами, доступными к приобретению и аренде.\n" +
                "Выберите интересующий вас пункт меню.",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.Start());
        }
        public async Task SendStartMenuAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Здравствуйте, рад вас видеть. \n" +
                "Я являюсь ботом, который поможет вам ознакомиться с домами, доступными к приобретению и аренде.\n" +
                "Выберите интересующий вас пункт меню.",
                replyMarkup: Buttons.Start());
        }
        public async Task SendAllHousesAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Вот все доступные на данных момент дома\n" +
                "Для того, чтобы в любой момент вернуться на стартовое меню, введите /start");
            foreach (var item in await HousesRepositore.GetAllHousesAsync())
            {
                await SendOneHouseAsync(item);
            }
        }
        public async Task SendFiltersMenuAsync(int messageId)
        {
            await Client.EditMessageTextAsync(ChatId,
                messageId,
                "Вот доступные фильтры\n" +
                "Напоминаем, что незарегистрированный клиент может использовать единовременно только 1 фильтр\n" +
                "Для того, чтобы зарегистрироваться введите \"Регистрация <Имя>\", где <Имя> заменить на ваше имя.",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                await Buttons.FiltersMenuForUserAsync(ChatId));
        }
        public async Task SendHousesForUserAsync(long ChatId, int messageId)
        {
            try
            {
                await Client.EditMessageTextAsync(ChatId, messageId, "Дома, соответствующие вашим фильтрам:");
                foreach (var item in await UsersRepositore.GetHousesWhithCustomFiltersAsync(ChatId))
                {
                    await SendOneHouseAsync(item);
                }
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
                foreach (var item in await HousesRepositore.GetHousesByTypeAsync(type))
                {
                    await SendOneHouseAsync(item);
                }
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync(messageId);
            }
        }
        public async Task SendHousesByRentTypeAsync(string type, int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, messageId, $"Вот все доступные {type} на данный момент:");
            try
            {
                foreach (var item in await HousesRepositore.GetHousesByRentTypeAsync(type))
                {
                    await SendOneHouseAsync(item);
                }
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
                $"Вот все доступные помещение на данный момент в этом районе");
            try
            {
                foreach (var item in await HousesRepositore.GetHouseByDistrictAsync(type))
                {
                    await SendOneHouseAsync(item);
                }
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
                foreach (var item in await HousesRepositore.GetHousesByRoomsNumberAsync(roomsNumber))
                {
                    await SendOneHouseAsync(item);
                }
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync(messageId);
            }
        }
        public async Task SendDistrictsListAsync(int messageId)
        {
            try
            {
                var districts = await GetAllDistrictsAsync();
                await Client.EditMessageTextAsync(ChatId, messageId, "Вот все доступные районы:",
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)
                Buttons.GetFiltersList(districts));
            }
            catch (NotFoundException)
            {
                await Client.EditMessageTextAsync(ChatId, messageId, "Районы пока не добавлены");
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
                replyMarkup: (Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup)Buttons.GetFiltersList(rooms));
            }
            catch (NotFoundException)
            {
                await Client.EditMessageTextAsync(ChatId, messageId, "Количество комнат пока не добавлено");
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
                Buttons.GetFiltersList(types));
            }
            catch (NotFoundException)
            {
                await Client.SendTextMessageAsync(ChatId, "Типы жилплощади пока не добавлены");
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
                Buttons.GetFiltersList(rentTypes));
            }
            catch (NotFoundException)
            {
                await Client.SendTextMessageAsync(ChatId, "Типы продажи пока не добавлены");
            }
        }
        public async Task SendNotFoundMessageAsync(int messageId)
        {
            await Client.EditMessageTextAsync(ChatId, messageId, "Дома с такими параметрами не обнаруженно.");
        }
        public async Task SendAlreadyRegisterAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Вы уже зарегистрированны.\n" +
                "Если хотите сменить имя, введите \"Удалить меня\" и перерегистрируйтесь.");
        }

        private static async Task<IEnumerable<string>> GetAllDistrictsAsync()
        {
            var allDistricts = from house in await HousesRepositore.GetAllHousesAsync() select house.District;
            allDistricts = allDistricts.Distinct();
            if (allDistricts == null)
            {
                throw new NotFoundException();
            }
            return allDistricts;
        }
        private static async Task<IEnumerable<string>> GetAllTypesAsync()
        {
            var allTypes = from house in await HousesRepositore.GetAllHousesAsync() select house.Type;
            allTypes = allTypes.Distinct();
            if (allTypes == null)
            {
                throw new NotFoundException();
            }
            return allTypes;
        }
        private static async Task<IEnumerable<string>> GetAllRentTypesAsync()
        {
            var allRentTypes = from house in await HousesRepositore.GetAllHousesAsync() select house.RentType;
            allRentTypes = allRentTypes.Distinct();
            if (allRentTypes == null)
            {
                throw new NotFoundException();
            }
            return allRentTypes;
        }
        private static async Task<IEnumerable<int?>> GetAllRoomsNumberAsync()
        {
            var allRoomsNumber = from house in await HousesRepositore.GetAllHousesAsync() select house.RoomsNumber;
            allRoomsNumber = allRoomsNumber.OrderBy(n => n).Distinct();
            if (allRoomsNumber == null)
            {
                throw new NotFoundException();
            }
            return allRoomsNumber;
        }
        private static async Task SendOneHouseAsync(House house)
        {
            await Client.SendTextMessageAsync(ChatId,
                    $"Описание: {house.Description}\n" +
                    $"Метраж: {house.Footage}\n" +
                    $"Число комнат: {house.RoomsNumber}\n" +
                    $"Тип покупки: {house.RentType}\n" +
                    $"Цена: {house.Price}\n");
        }
    }
}
