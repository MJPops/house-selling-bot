﻿using HouseSellingBot.Models;
using HouseSellingBot.Repositories;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace HouseSellingBot.UI
{
    /// <summary>
    /// A class that encapsulates messages towards the user.
    /// </summary>
    public class Messages
    {
        private static TelegramBotClient Client { get; set; }
        private static long chatId;

        /// <summary>
        /// A class that encapsulates messages towards the user.
        /// </summary>
        [Obsolete]
        public Messages(TelegramBotClient client, long _chatId)
        {
            Client = client;
            chatId = _chatId;
        }

        /// <summary>
        /// Sends a start menu with a set of buttons.
        /// </summary>
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
            await Client.SendTextMessageAsync(chatId,
                "Вот доступные фильтры\n" +
                "Напоминаем, что незарегистрированный клиент может использовать единовременно только 1 фильтр\n" +
                "Для того, чтобы зарегистрироваться введите \"Регистрация <Имя>\", где <Имя> заменить на ваше имя.",
                replyMarkup: Buttons.FiltersMenu());
        }
        public async Task SendHousesTypeMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId,
                "Выберите тип жилого помещения:",
                replyMarkup: Buttons.HousesTypesMenu());
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
