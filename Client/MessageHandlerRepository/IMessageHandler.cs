using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.MessageHandlerRepository
{
    /// <summary>
    /// Обработчик поступающих данных с сервера
    /// </summary>
    internal interface IMessageHandler
    {
        /// <summary>
        /// Обработать массив байтов, поступивших с сервера
        /// </summary>
        /// <param name="bytes"></param>
        public void Handle(byte[] bytes);
    }
}
