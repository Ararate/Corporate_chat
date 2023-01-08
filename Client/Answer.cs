using ProtoBuf;
using ProtoBuf.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
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
        private Answer()
        {
            
        }
        /// <summary>
        /// Получить экземпляр из массива байтов
        /// </summary>
        /// <param name="bytes"></param>
        internal Answer(byte[] bytes)
        {
            using var stream = new MemoryStream();
            stream.Write(bytes.Where(i => i != 0).ToArray(),
                0, bytes.Where(i => i != 0).ToArray().Length);
            stream.Seek(0, SeekOrigin.Begin);

            var ans = Serializer.Deserialize<Answer>(stream);
            whiteText = ans.whiteText;
            color = ans.color;
            coloredText = ans.coloredText;
        }
    }
}
