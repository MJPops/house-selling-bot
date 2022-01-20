using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace HouseSellingBot.UI
{
    public class Message
    {
        private TelegramBotClient Client { get; set; }
        public Message(TelegramBotClient client)
        {
            Client = client;
        }
    }
}
