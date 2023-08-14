using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Отправщик сообщений пользователю
    /// </summary>
    internal interface ISender
    {
        /// <summary>
        /// Набор сообщений
        /// </summary>
        public Task MessageList(List<Message> messagesList, UserGroup group);
        /// <summary>
        /// Одно сообщение
        /// </summary>
        public Task SingleMessage(Message message, UserGroup group);
        /// <summary>
        /// Простая белая строка
        /// </summary>
        public Task Notification(string message, ConsoleColor color, UserGroup group);
    }
}
