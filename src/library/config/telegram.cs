using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace OdinSdk.BaseLib.Configuration
{
    /// <summary>
    ///
    /// </summary>
    public class CTelegramMessage
    {
        /// <summary>
        ///
        /// </summary>
        public bool isBroadcasting
        {
            get;
            set;
        }

        /// <summary>
        /// chatting uniqueue id
        /// </summary>
        public long chatid
        {
            get;
            set;
        }

        /// <summary>
        /// grouping
        /// </summary>
        public string category
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string nickName
        {
            get;
            set;
        }

        /// <summary>
        ///
        /// </summary>
        public string message
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CTelegramUser
    {
        /// <summary>
        /// grouping
        /// </summary>
        public string category
        {
            get;
            set;
        }

        /// <summary>
        /// chatting uniqueue id
        /// </summary>
        public long chatid
        {
            get;
            set;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class CTelegram
    {
        /// <summary>
        ///
        /// </summary>
        public CTelegram(string token)
        {
            Token = token;
            ChatIds = new List<CTelegramUser>();
        }

        /// <summary>
        ///
        /// </summary>
        public CTelegram(string token, List<CTelegramUser> chatIds)
        {
            Token = token;
            ChatIds = chatIds;
        }

        /// <summary>
        /// telegram-token
        /// </summary>
        private string Token
        {
            get; set;
        }

        /// <summary>
        /// telegram-chatid
        /// </summary>
        private List<CTelegramUser> ChatIds
        {
            get; set;
        }

        private TelegramBotClient __telegram_api = null;

        private TelegramBotClient TelegramApi
        {
            get
            {
                if (__telegram_api == null)
                    __telegram_api = new TelegramBotClient(Token);
                return __telegram_api;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public async Task SendMessage(CTelegramMessage message)
        {
            foreach (var _chatid in ChatIds)
            {
                if (message.isBroadcasting == true || message.category == _chatid.category || message.chatid == _chatid.chatid)
                    await TelegramApi.SendTextMessageAsync(_chatid.chatid, message.message);
            }
        }
    }
}