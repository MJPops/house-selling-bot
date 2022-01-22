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
                        InlineKeyboardButton.WithCallbackData(text: "Все дома", callbackData: "ВсеДома"),
                        InlineKeyboardButton.WithCallbackData(text: "Фильтры", callbackData: "Фильтры")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "О нас", callbackData: "О нас"),
                    }
            });
            ;
        }
        public static IReplyMarkup FiltersMenu()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "По Цене",
                        callbackData: "ПоЦене"),
                        InlineKeyboardButton.WithCallbackData(text: "По Метражу",
                        callbackData: "ПоМетражу")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "По Району",
                        callbackData: "ПоРайону"),
                        InlineKeyboardButton.WithCallbackData(text: "По Комнатам",
                        callbackData: "ПоКомнатам")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "По Типу Дома",
                        callbackData: "ПоТипуДома")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "По Типу Покупки",
                        callbackData: "ПоТипуПокупки")
                    }
            });
            ;
        }
        public static IReplyMarkup HousesTypesMenu()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Частные Дома", callbackData: "ЧастныеДома"),
                        InlineKeyboardButton.WithCallbackData(text: "Квартиры", callbackData: "Квартиры")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "О нас", callbackData: "О нас"),
                    }
            });
            ;
        }
    }
}


