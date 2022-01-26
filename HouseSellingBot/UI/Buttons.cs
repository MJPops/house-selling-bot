using HouseSellingBot.Models;
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
        }
        public static async Task<IReplyMarkup> FiltersMenuForUserAsync(long chatId)
        {
            List<List<InlineKeyboardButton>> startButtons = new()
            {
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "По Цене",
                    callbackData: "ПоЦене"),
                    InlineKeyboardButton.WithCallbackData(text: "По Метражу",
                    callbackData: "ПоМетражу"),
                    InlineKeyboardButton.WithCallbackData(text: "По Комнатам",
                    callbackData: "ПоКомнатам")
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "По Району",
                    callbackData: "ПоРайону"),
                    InlineKeyboardButton.WithCallbackData(text: "По Метро",
                    callbackData: "ПоМетро")
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "По Типу Покупки",
                    callbackData: "ПоТипуПокупки"),
                    InlineKeyboardButton.WithCallbackData(text: "По Типу Дома",
                    callbackData: "ПоТипуДома")
                }
            };
            if (await UsersRepositore.UserIsRegisteredAsync(chatId))
            {
                startButtons.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Мои Фильтры",
                    callbackData: "МоиФильтры"),
                    InlineKeyboardButton.WithCallbackData(text: "Дома по Фильтрам",
                    callbackData: "ДомаПоФильтрам")
                });
                startButtons.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Очистить Фильтры",
                    callbackData: "ОчиститьФильтры"),
                    InlineKeyboardButton.WithCallbackData(text: "Назад",
                    callbackData: "/start")
                });
            }
            else
            {
                startButtons.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Назад",
                    callbackData: "/start")
                });
            }

            return new InlineKeyboardMarkup(startButtons);

        }
        public static IReplyMarkup BackToStart()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Меню", callbackData: "/start"),
                    }
            });
        }
        public static IReplyMarkup BackToFilters()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "К фильтрам", callbackData: "Фильтры"),
                    }
            });
        }
        public static IReplyMarkup PriceFilters()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Верхнее", callbackData: "ЦенаВерх"),
                        InlineKeyboardButton.WithCallbackData(text: "Нижнее", callbackData: "ЦенаНиз")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "Фильтры"),
                    }
            });
        }
        public static IReplyMarkup FootageFilters()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Верхнее", callbackData: "МетражВерх"),
                        InlineKeyboardButton.WithCallbackData(text: "Нижнее", callbackData: "МетражНиз")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "Фильтры"),
                    }
            });
        }
        public static IReplyMarkup FiltersList(IEnumerable<string> filters)
        {
            List<List<InlineKeyboardButton>> filtersList = new();

            foreach (string filter in filters)
            {
                filtersList.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: filter, callbackData: filter)
                });
            }
            filtersList.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "Фильтры")
            });

            return new InlineKeyboardMarkup(filtersList);
        }
        public static IReplyMarkup FiltersList(IEnumerable<int?> filters)
        {
            List<List<InlineKeyboardButton>> filtersList = new();

            foreach (var filter in filters)
            {
                filtersList.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: filter.ToString(), callbackData: filter.ToString())
                });
            }
            filtersList.Add(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithCallbackData(text: "Назад", callbackData: "Фильтры")
            });
            return new InlineKeyboardMarkup(filtersList);
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
        public static IReplyMarkup Link(string path)
        {
            return new InlineKeyboardMarkup(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithUrl("Ссылка", path),
            });
        }
        public static IReplyMarkup Link(string path, long chatId)
        {
            return new InlineKeyboardMarkup(new List<InlineKeyboardButton>
            {
                InlineKeyboardButton.WithUrl("Ссылка", path),
                InlineKeyboardButton.WithCallbackData(text: "Добавить в избранное", callbackData: "Избранное")
            });
        }
        public static IReplyMarkup RedactionMenu()
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Редактировать",
                    callbackData: "Редактировать"),
                    InlineKeyboardButton.WithCallbackData(text: "Добавить",
                    callbackData: "Добавить")
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Удалить Дом",
                    callbackData: "УдалитьДом"),
                }
            });
        }
        public static IReplyMarkup AdminsListAsync(IEnumerable<User> admins)
        {
            List<List<InlineKeyboardButton>> adminsListToDelete = new();

            foreach (var admin in admins)
            {
                adminsListToDelete.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: $"Удалить {admin.Name}", 
                    callbackData: $"Удалить{admin.ChatId}")
                });
            }

            return new InlineKeyboardMarkup(adminsListToDelete);
        }
    }
}


