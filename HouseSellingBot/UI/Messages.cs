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
        public async Task SendFiltersMenuAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Вот доступные фильтры\n" +
                "Напоминаем, что незарегистрированный клиент может использовать единовременно только 1 фильтр\n" +
                "Для того, чтобы зарегистрироваться введите \"Регистрация <Имя>\", где <Имя> заменить на ваше имя.",
                replyMarkup: await Buttons.FiltersMenuForUserAsync(ChatId));
        }
        public async Task SendHousesForUserAsync(long ChatId)
        {
            await Client.SendTextMessageAsync(ChatId, "Дома, соответствующие вашим фильтрам:");
            foreach (var item in await UsersRepositore.GetHousesWhithCustomFiltersAsync(ChatId))
            {
                await SendOneHouseAsync(item);
            }
        }
        public async Task SendHousesTypeMenuAsync()
        {
            await Client.SendTextMessageAsync(ChatId,
                "Выберите тип жилого помещения:",
                replyMarkup: await Buttons.HousesTypesMenuForUser(ChatId));
        }
        public async Task SendHousesByTypeAsync(string type)
        {
            await Client.SendTextMessageAsync(ChatId, $"Вот все доступные {type} на данный момент:");
            try
            {
                foreach (var item in await HousesRepositore.GetHousesByTypeAsync(type))
                {
                    await SendOneHouseAsync(item);
                }
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync();
            }
        }
        public async Task SendHousesByDistrictAsync(string type)
        {
            await Client.SendTextMessageAsync(ChatId, $"Вот все доступные помещение на данный момент в этом районе");
            try
            {
                foreach (var item in await HousesRepositore.GetHouseByDistrictAsync(type))
                {
                    await SendOneHouseAsync(item);
                }
            }
            catch (NotFoundException)
            {
                await SendNotFoundMessageAsync();
            }
        }
        public async Task SendHousesByRoomsNumberAsync(int roomsNumber)
        {
            await Client.SendTextMessageAsync(ChatId, $"Вот все доступные {roomsNumber}-х комнатные" +
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
                await SendNotFoundMessageAsync();
            }
        }
        public async Task SendDistrictsListAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Вот все доступные районы:");
            try
            {
                int number = 0;
                foreach (var district in await GetAllDistrictsAsync())
                {
                    number++;
                    await Client.SendTextMessageAsync(ChatId, $"{number}. {district}");
                }
            }
            catch (NotFoundException)
            {
                await Client.SendTextMessageAsync(ChatId, "Районы пока не добавлены");
            }
        }
        public async Task SendRoomsNumberListAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Вот все доступные варианты квартир по количеству комнат:");
            try
            {
                int number = 0;
                foreach (var roomsnumber in await GetAllRoomsNumberAsync())
                {
                    number++;
                    await Client.SendTextMessageAsync(ChatId, $"{number}. Количество комнат: {roomsnumber}");
                }
            }
            catch (NotFoundException)
            {
                await Client.SendTextMessageAsync(ChatId, "Количество комнат пока не добавлено");
            }
        }
        public async Task SendRentTypeListAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Вот все доступные варианты:");
            try
            {
                int number = 0;
                foreach (var renttype in await GetAllRentTypesAsync())
                {
                    number++;
                    await Client.SendTextMessageAsync(ChatId, $"{number}. {renttype}");
                }
            }
            catch (NotFoundException)
            {
                await Client.SendTextMessageAsync(ChatId, "Типы продажи пока не добавлены");
            }
        }
        public async Task SendNotFoundMessageAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Дома с такими параметрами не обнаруженно.");
        }
        public async Task SentAlreadyRegisterAsync()
        {
            await Client.SendTextMessageAsync(ChatId, "Вы уже зарегистрированны.\n" +
                "Если хотите сменить имя, введите \"Удали меня\" и перерегистрируйтесь.");
        }

        private async Task<IEnumerable<string>> GetAllDistrictsAsync()
        {
            var allDistricts = from house in await HousesRepositore.GetAllHousesAsync() select house.District;
            allDistricts = allDistricts.Distinct();
            if (allDistricts == null)
            {
                throw new NotFoundException();
            }
            return allDistricts;
        }
        private async Task<IEnumerable<string>> GetAllRentTypesAsync()
        {
            var allRentTypes = from house in await HousesRepositore.GetAllHousesAsync() select house.RentType;
            allRentTypes = allRentTypes.Distinct();
            if (allRentTypes == null)
            {
                throw new NotFoundException();
            }
            return allRentTypes;
        }
        private async Task<IEnumerable<int?>> GetAllRoomsNumberAsync()
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
