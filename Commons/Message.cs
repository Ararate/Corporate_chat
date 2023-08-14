using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons
{
    /// <summary>
    /// Сообщение в формате {цветной текст}{белый текст}
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Цвет цветного текста
        /// </summary>
        public ConsoleColor Color { get; set; }
        /// <summary>
        /// Белый текст
        /// </summary>
        public string WhiteText { get; set; }
        /// <summary>
        /// Цветной текст
        /// </summary>
        public string ColoredText { get; set; }
        public Message()
        {

        }
        public Message(string coloredText, 
            string whiteText, ConsoleColor color)
        {
            Color = color;
            WhiteText = whiteText;
            ColoredText = coloredText;
        }
        public Message(string coloredText, ConsoleColor color)
        {
            Color = color;
            ColoredText = coloredText;
            WhiteText = "";
        }
    }
}
