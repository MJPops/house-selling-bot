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
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.Start());
        }
        public async Task SendContactsMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.Contacts());
        }
        public async Task SendRentMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.Rent());
        }
        public async Task SendSaleMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.Sale());
        }
        public async Task SendRentDistrictMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.InRentDistrict());
        }
        public async Task SendRentRoomsMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.InRentRooms());
        }
        public async Task SendRentPriceMenuAsync()
        { 
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.InRentPrice());
        }
        public async Task SendRentFootageMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.InRentFootage());
        }
        public async Task SendSaleDictrictMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.InSaleDistrict());
        }
        public async Task SendSaleRoomsMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.InSaleRooms());
        }
        public async Task SendSalePriceMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.InSalePrice());
        }
        public async Task SendSaleFootageMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.InSaleFootage());
        }
        public async Task SendFilterMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите действие", replyMarkup: Buttons.Filter());
        }
        public async Task SendInfoMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите дейтсвие", replyMarkup: Buttons.Contacts());
        }
        public async Task SendDistrictMenuAsync()
        {
            await Client.SendTextMessageAsync(chatId, "Выберите район", replyMarkup: Buttons.Districts());
        }
    }
}
