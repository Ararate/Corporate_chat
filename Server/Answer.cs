using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Ответ сервера
    /// </summary>
    [ProtoContract]
    internal class Answer
    {
        [ProtoMember(1)]
        internal ConsoleColor color;
        [ProtoMember(2)]
        internal string whiteText;
        [ProtoMember(3)]
        internal string coloredText;
        public Answer()
        {

        }
        internal Answer(string whiteText, 
            string coloredText, ConsoleColor color)
        {
            this.color = color;
            this.whiteText = whiteText;
            this.coloredText = coloredText;
        }
        /// <summary>
        /// Перевести в массив байтов
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, this);
                string t = whiteText;
                return stream.GetBuffer();
            };
            
        }
    }
}
