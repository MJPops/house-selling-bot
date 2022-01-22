using HouseSellingBot.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public static async Task<IReplyMarkup> FiltersMenuForUserAsync(long chatId)
        {
            if (await UsersRepositore.UserIsRegisteredAsync(chatId))
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
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Очистить Фильтры",
                        callbackData: "ОчиститьФильтры")
                    }
                });
                ;
            }
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
        public static async Task<IReplyMarkup> HousesTypesMenuForUser(long chatId)
        {
            if (await UsersRepositore.UserIsRegisteredAsync(chatId))
            {
                return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Частные Дома",
                        callbackData: "ЧастныеДомаРег"),
                        InlineKeyboardButton.WithCallbackData(text: "Квартиры",
                        callbackData: "КвартирыРег")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "О нас", callbackData: "О насРег"),
                    }
                });
                ;
            }
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


