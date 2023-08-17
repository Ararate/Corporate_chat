using Commons;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Стандартный отправщик сообщений
    /// </summary>
    internal class Sender : ISender
    {
        private readonly List<Session> _sessions;
        private readonly Session _currentSession;
        public Sender(List<Session> sessions, Session session)
        {
            _sessions = sessions;
            _currentSession = session;
        }
        public async Task MessageList(List<Message> messagesList, UserGroup group)
        {
            DTO dto = new()
            {
                MessageList = messagesList,
            };
            Group(group, dto);
        }

        public async Task SingleMessage(Message message, UserGroup group)
        {
            DTO dto = new()
            {
                MessageList = new List<Message>() { message },
            };
            Group(group, dto);
        }

        public async Task Notification(string message, ConsoleColor color, UserGroup group)
        {

            DTO dto = new()
            {
                MessageList = new List<Message>() { new Message(message, color) }
            };
            Group(group, dto);
        }

        private async void Group(UserGroup group, DTO dto)
        {
            if (group == UserGroup.All)
            {
                foreach (var session in _sessions)
                    await SendAsync(session.Stream, dto);
                return;
            }
            if (group == UserGroup.Others)
            {
                foreach (var session in _sessions
                    .Where(i => i != _currentSession))
                    await SendAsync(session.Stream, dto);
                return;
            }
            if (group == UserGroup.This)
            {
                await SendAsync(_currentSession.Stream, dto);
                return;
            }
        }

        /// <summary>
        /// Сериализация в json и отправка
        /// </summary>
        private static async Task SendAsync(NetworkStream stream, DTO dto)
        {
            string json = JsonSerializer.Serialize(dto);
            byte[] bytes = Encoding.Unicode.GetBytes(json);
            byte[] size = new byte[2] {
                (byte)(bytes.Length >> 8),
                (byte)bytes.Length
            };
            await stream.WriteAsync(size);
            await stream.WriteAsync(bytes);
            await stream.FlushAsync();
        }
    }
}
