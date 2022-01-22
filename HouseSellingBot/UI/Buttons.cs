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
        public static IReplyMarkup Districts()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
            new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Центральный", callbackData: "Центральный"),//id-1
                        InlineKeyboardButton.WithCallbackData(text: "Северный", callbackData: "Северный")//id-2
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Северо-Восточный", callbackData: "Северо-Восточный"),//id-3
                        InlineKeyboardButton.WithCallbackData(text: "Восточный", callbackData: "Восточный")//id-4
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Юго-Восточный", callbackData: "Юго-Восточный"),//id-5
                        InlineKeyboardButton.WithCallbackData(text: "Южный", callbackData: "Южный")//id-6
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Юго-Западный", callbackData: "Юго-Западный"),//id-7
                        InlineKeyboardButton.WithCallbackData(text: "Западный", callbackData: "Западный")//id-8
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Северо-Западный", callbackData: "Северо-Западный"),//id-9
                        InlineKeyboardButton.WithCallbackData(text: "Зеленоградский", callbackData: "Зеленоградский")//id-10
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Троицкий", callbackData: "Троицкий"),//id-11
                        InlineKeyboardButton.WithCallbackData(text: "Новомосковский", callbackData: "Новомосковский")//id-12
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
                        InlineKeyboardButton.WithCallbackData(text: "Покупка дома",
                        callbackData: "Продажа дома"),
                        InlineKeyboardButton.WithCallbackData(text: "Покупка квартиры",
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
                        InlineKeyboardButton.WithCallbackData(text: "Добавить фильтр", callbackData: "Добавить фильтр")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Вывести варианты", callbackData: "Вывести варианты")
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
                        InlineKeyboardButton.WithCallbackData(text: "Добавить фильтр", callbackData: "Добавить фильтр")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Вывести варианты", callbackData: "Вывести варианты")
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
                        InlineKeyboardButton.WithCallbackData(text: "Добавить фильтр", callbackData: "Добавить фильтр")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Вывести варианты", callbackData: "Вывести варианты")
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
                        InlineKeyboardButton.WithCallbackData(text: "Выберите район", callbackData: "РайонПокупка"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "Покупка"),
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
                        InlineKeyboardButton.WithCallbackData(text: "Выберите количество комнат", callbackData: "КомнатаПокупка"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Добавить фильтр", callbackData: "Добавить фильтр")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Вывести варианты", callbackData: "Вывести варианты")
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "РайонПокупка"),
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
                        InlineKeyboardButton.WithCallbackData(text: "Выберите желаемую стоимость", callbackData: "СтоимостьПокупка"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Добавить фильтр", callbackData: "Добавить фильтр")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Вывести варианты", callbackData: "Вывести варианты")
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "КомнатаПокупка"),
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
                        InlineKeyboardButton.WithCallbackData(text: "Выберите желаемый метраж", callbackData: "МетражПокупка"),
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Добавить фильтр", callbackData: "Добавить фильтр")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Вывести варианты", callbackData: "Вывести варианты")
                    },
                    new List<InlineKeyboardButton>
                    {
                        //TODO - Add way back
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "СтоимостьПокупка"),
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
        public static IReplyMarkup Filter()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Добавить фильтр", callbackData: "Добавить фильтр")
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Вывести варианты", callbackData: "Вывести варианты")
                }
            });
        }
    }
}


