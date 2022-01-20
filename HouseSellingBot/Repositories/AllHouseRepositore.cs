using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace HouseSellingBot.Repositories
{
    public class AllHouseRepositore
    {
        private TelegramBotClient Client { get; set; }
        public AllHouseRepositore(TelegramBotClient client)
        {
            Client = client;
        }


    }
}
