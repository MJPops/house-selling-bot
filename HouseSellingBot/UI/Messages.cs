using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace HouseSellingBot.UI
{
    /// <summary>
    /// A class that encapsulates messages towards the user.
    /// </summary>
    public class Messages
    {
        private static TelegramBotClient Client { get; set; }
        private static Telegram.Bot.Types.Message message;

        /// <summary>
        /// A class that encapsulates messages towards the user.
        /// </summary>
        [Obsolete]
        public Messages(TelegramBotClient client, MessageEventArgs e)
        {
            Client = client;
            message = e.Message;
        }

        /// <summary>
        /// Sends a start menu with a set of buttons.
        /// </summary>
        public async Task SendStartMenuAsync()
        {
            await Client.SendTextMessageAsync(message.Chat.Id,
                "Надмите кнопку",
                replyMarkup: Buttons.Start());
        }
    }
}
