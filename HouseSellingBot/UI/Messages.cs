using HouseSellingBot.Models;
using HouseSellingBot.Repositories;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace HouseSellingBot.UI
{
    public class Messages
    {
        private static TelegramBotClient Client { get; set; }
        private static long chatId;

        [Obsolete]
        public Messages(TelegramBotClient client, long _chatId)
        {
            Client = client;
            chatId = _chatId;
        }

        public async Task SendStartMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Здравствуйте, рад вас видеть. \n" +
                "Я являюсь ботом, который поможет вам ознакомиться с домами, доступными к приобретению и аренде.\n" +
                "Выберите интересующий вас пункт меню.",
                replyMarkup: Buttons.Start());
        }
        public async Task SendAllHousesAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Вот все доступные на данных момент дома\n" +
                "Для того, чтобы в любой момент вернуться на стартовое меню, введите /start");
            foreach (var item in await HousesRepositore.GetAllHousesAsync())
            {
                await SendOneHouseAsync(item);
            }
        }


        public async Task SendFiltersMenuAsync()
        {
            if (await UsersRepositore.UserIsRegisteredAsync(chatId))
            {

            }
            await Client.SendTextMessageAsync(chatId,
                "Вот доступные фильтры\n" +
                "Напоминаем, что незарегистрированный клиент может использовать единовременно только 1 фильтр\n" +
                "Для того, чтобы зарегистрироваться введите \"Регистрация <Имя>\", где <Имя> заменить на ваше имя.",
                replyMarkup: await Buttons.FiltersMenuForUserAsync(chatId));
        }
        public async Task SendHousesForUserAsync(long chatId)
        {
            await Client.SendTextMessageAsync(chatId, "Дома, соответствующие вашим фильтрам:");
            foreach (var item in await UsersRepositore.GetHousesWhithCustomFiltersAsync(chatId))
            {
                await SendOneHouseAsync(item);
            }
        }
        public async Task SendHousesTypeMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId,
                "Выберите тип жилого помещения:",
                replyMarkup: await Buttons.HousesTypesMenuForUser(chatId));
        }
        public async Task SendHouseByTypeAsync(string type)
        {
            await Client.SendTextMessageAsync(chatId, $"Вот все доступные {type} на данный момент:");
            foreach (var item in await HousesRepositore.GetHousesByTypeAsync(type)) 
            {
                await SendOneHouseAsync(item);
            }
        }

        private static async Task SendOneHouseAsync(House house)
        {
            await Client.SendTextMessageAsync(chatId,
                    $"Описание: {house.Description}\n" +
                    $"Метраж: {house.Footage}\n" +
                    $"Число комнат: {house.RoomsNumber}\n" +
                    $"Тип покупки: {house.RentType}\n" +
                    $"Цена: {house.Price}\n");
        }
    }
}
