using HouseSellingBot.Models;
using HouseSellingBot.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace HouseSellingBot.UI
{
    public class Buttons
    {

        public static async Task<IReplyMarkup> StartAsync(long chatId)
        {
            List<List<InlineKeyboardButton>> returnsButtons = new()
            {
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Все дома", callbackData: "ВсеДома"),
                    InlineKeyboardButton.WithCallbackData(text: "Фильтры", callbackData: "Фильтры")
                }
            };
            if (await UsersRepositore.UserIsRegisteredAsync(chatId))
            {
                if (await UsersRepositore.UserIsAdminAsync(chatId)
                    || await UsersRepositore.UserIsDirectorAsync(chatId))
                {
                    returnsButtons.Add(new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Добавить дом",
                            callbackData: "ДобавитьДом")
                    });
                }
                else
                {
                    returnsButtons.Add(new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Мои избранные",
                            callbackData: "МоиИзбранные")
                    });
                }
            }
            returnsButtons.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "О нас", callbackData: "О нас")
                });

            return new InlineKeyboardMarkup(returnsButtons);
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
                    InlineKeyboardButton.WithCallbackData(text: "Очистить Фильтры",
                    callbackData: "ОчиститьФильтры")
                });
                startButtons.Add(new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Мои Фильтры",
                    callbackData: "МоиФильтры"),
                    InlineKeyboardButton.WithCallbackData(text: "Дома по Фильтрам",
                    callbackData: "ДомаПоФильтрам")
                });
                startButtons.Add(new List<InlineKeyboardButton>
                {
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
        public static IReplyMarkup RedactionMenuForHouses(int houseId)
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Редактировать",
                    callbackData: $"РедактированиеДома{houseId}"),
                    InlineKeyboardButton.WithCallbackData(text: "Удалить", callbackData: $"УдалитьДом{houseId}")
                }
            });
        }
        public static IReplyMarkup RedactionMenuForHouses(string path, int houseId)
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithUrl("Ссылка", path)
                },
                new List<InlineKeyboardButton>
                {
                    InlineKeyboardButton.WithCallbackData(text: "Редактировать",
                    callbackData: $"РедактированиеДома{houseId}"),
                    InlineKeyboardButton.WithCallbackData(text: "Удалить", callbackData: $"УдалитьДом{houseId}")
                }
            });
        }
        public static IReplyMarkup InRedaction(int houseId)
        {
            return new InlineKeyboardMarkup(new List<List<InlineKeyboardButton>>
            {
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Изображение",
                        callbackData: $"Изображение{houseId}"),
                        InlineKeyboardButton.WithCallbackData(text: "Ссылка", callbackData: $"Ссылка{houseId}")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Описание", callbackData: $"Описание{houseId}"),
                        InlineKeyboardButton.WithCallbackData(text: "Район", callbackData: $"Район{houseId}")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Тип дома", callbackData: $"ТипДома{houseId}"),
                        InlineKeyboardButton.WithCallbackData(text: "Тип покупки",
                        callbackData: $"ТипПокупки{houseId}")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Метро", callbackData: $"Метро{houseId}"),
                        InlineKeyboardButton.WithCallbackData(text: "Комнаты", callbackData: $"Комнаты{houseId}")
                    },
                    new List<InlineKeyboardButton>
                    {
                        InlineKeyboardButton.WithCallbackData(text: "Цена", callbackData: $"Цена{houseId}"),
                        InlineKeyboardButton.WithCallbackData(text: "Метраж", callbackData: $"Метраж{houseId}")
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
                        InlineKeyboardButton.WithCallbackData(text: "Нижнее", callbackData: "ЦенаНиз"),
                        InlineKeyboardButton.WithCallbackData(text: "Верхнее", callbackData: "ЦенаВерх")
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
                        InlineKeyboardButton.WithCallbackData(text: "Нижнее", callbackData: "МетражНиз"),
                        InlineKeyboardButton.WithCallbackData(text: "Верхнее", callbackData: "МетражВерх")
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
        public static async Task<IReplyMarkup> LinkAndAddToFavoritAsync(string path, long chatId, int houseId)
        {
            List<InlineKeyboardButton> returnsButtons = new()
            {
                InlineKeyboardButton.WithUrl("Ссылка", path),
            };

            if (await UsersRepositore.UserIsRegisteredAsync(chatId))
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);

                if (user.FavoriteHouses.Where(h => h.Id == houseId).Any())
                {
                    returnsButtons.Add(InlineKeyboardButton.WithCallbackData(text: "Удалить из избранного",
                        callbackData: $"УдалитьИзИзбранного{houseId}"));
                }
                else
                {
                    returnsButtons.Add(InlineKeyboardButton.WithCallbackData(text: "Добавить в избранное",
                    callbackData: $"Избранное{houseId}"));
                }
            }
            return new InlineKeyboardMarkup(returnsButtons);
        }
        public static async Task<IReplyMarkup> AddToFavorit(long chatId, int houseId)
        {
            List<InlineKeyboardButton> returnsButtons = new();
            if (await UsersRepositore.UserIsRegisteredAsync(chatId))
            {
                var user = await UsersRepositore.GetUserByChatIdAsync(chatId);

                if (user.FavoriteHouses.Where(h => h.Id == houseId).Any())
                {
                    returnsButtons.Add(InlineKeyboardButton.WithCallbackData(text: "Удалить из избранного",
                        callbackData: $"УдалитьИзИзбранного{houseId}"));
                }
                else
                {
                    returnsButtons.Add(InlineKeyboardButton.WithCallbackData(text: "Добавить в избранное",
                    callbackData: $"Избранное{houseId}"));
                }
            }
            else
            {
                return null;
            }
            return new InlineKeyboardMarkup(returnsButtons);
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
                    callbackData: $"УдалитьАдмина{admin.ChatId}")
                });
            }

            return new InlineKeyboardMarkup(adminsListToDelete);
        }
    }
}


