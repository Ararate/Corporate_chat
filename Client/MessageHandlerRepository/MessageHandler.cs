using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Client.MessageHandlerRepository
{
    internal class MessageHandler : IMessageHandler
    {
        public void Handle(byte[] bytes)
        {
            DTO dto = new(bytes);
            var messages = dto.MessageList;
            foreach (var message in messages)
            {
                Console.ForegroundColor = message.Color;
                Console.Write(message.ColoredText);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(message.WhiteText);
            }
        }
    }
}
