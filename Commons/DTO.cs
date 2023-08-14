using ProtoBuf;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System;

namespace Commons
{
    /// <summary>
    /// Объект передачи данных с сервера
    /// </summary>
    public class DTO
    {
        /// <summary>
        /// Список сообщений
        /// </summary>
        public List<Message> MessageList { get; set; }

        public DTO()
        {
            
        }

        public DTO(byte[] jsonBytes)
        {
            string json = Encoding.Unicode.GetString(jsonBytes);
            var ans = JsonSerializer.Deserialize<DTO>(json);
            MessageList = ans.MessageList;
        }
    }
}
