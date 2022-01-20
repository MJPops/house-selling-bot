using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

namespace HouseSellingBot.UI
{
    public class Buttons
    {
        public static IReplyMarkup Start()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Аренда", callbackData: "Аренда"),
                        InlineKeyboardButton.WithCallbackData(text: "Покупка", callbackData: "Покупка")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "О нас", callbackData: "О нас"),
                    }
            });
            ;
        }
        public static IReplyMarkup Rent()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Аренда дома", 
                        callbackData: "Аренда дома"),
                        InlineKeyboardButton.WithCallbackData(text: "Аренда квартиры",
                        callbackData: "Аренда квартиры")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "/start"),
                    }
            });
            ;
        }
        public static IReplyMarkup Sale()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Продажа дома",
                        callbackData: "Продажа дома"),
                        InlineKeyboardButton.WithCallbackData(text: "Продажа квартиры",
                        callbackData: "Продажа квартиры")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "/start"),
                    }
            });
            ;
        }
        public static IReplyMarkup InRent()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Район", callbackData: "Район"),
                        InlineKeyboardButton.WithCallbackData(text: "Количество комнат", 
                        callbackData: "Количество комнат")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Этаж", callbackData: "Этаж"),
                        InlineKeyboardButton.WithCallbackData(text: "Цена", callbackData: "Цена")
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: ""),
                    }
            });
            ;
        }
    }
}


