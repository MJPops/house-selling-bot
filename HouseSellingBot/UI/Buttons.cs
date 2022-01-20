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
        public static IReplyMarkup InRentDistrict()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Выберите район", callbackData: "РайонАренда"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "Аренда"),
                    }
            });
            ;
        }
        public static IReplyMarkup InRentRooms()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Выберите количество комнат", callbackData: "КомнатаАренда"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "РайонАренда"),
                    }
            });
            ;
        }
        public static IReplyMarkup InRentPrice()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Выберите желаемую стоимость", callbackData: "СтоимостьАренда"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "КомнатаАренда"),
                    }
            });
            ;
        }
        public static IReplyMarkup InRentFootage()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Выберите желаемый мтераж", callbackData: "МетражАренда"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "СтоимостьАренда"),
                    }
            });
            ;
        }
        public static IReplyMarkup InSaleDistrict()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Выберите район", callbackData: "РайонПродажа"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "Продажа"),
                    }
            });
            ;
        }
        public static IReplyMarkup InSaleRooms()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Выберите количество комнат", callbackData: "КомнатаПродажа"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "РайонПродажа"),
                    }
            });
            ;
        }
        public static IReplyMarkup InSalePrice()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Выберите желаемую стоимость", callbackData: "СтоимостьПродажа"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "КомнатаПродажа"),
                    }
            });
            ;
        }
        public static IReplyMarkup InSaleFootage()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Выберите желаемый метраж", callbackData: "МетражПродажа"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "СтоимостьПродажа"),
                    }
            });
            ;
        }
        public static IReplyMarkup Contacts()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "", callbackData: "О нас")
                }
            });
        }
    }
}


