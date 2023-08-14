using Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server 
{
    /// <summary>
    /// Сессия для отдельного пользователя
    /// </summary>
    internal class Session
    {
        /// <summary>
        /// Никнейм
        /// </summary>
        internal string Nickname { get; set; }
        /// <summary>
        /// IP адрес
        /// </summary>
        internal string IpAdress { get; set; }
        /// <summary>
        /// Сетевой поток
        /// </summary>
        internal NetworkStream Stream { get; set; }
        /// <summary>
        /// Цвет никнейма
        /// </summary>
        internal ConsoleColor NickColor { get; set; } = ConsoleColor.White;
        internal readonly TcpClient client;
        internal readonly ISender sender;
        private readonly List<Session> sessions;
        private readonly Dictionary<string, Action<Session, string[]>> _commands;
        internal Session(TcpClient client, List<Session> sessions, Dictionary<string, Action<Session, string[]>> commands) 
        {
            this.client = client;
            IpAdress = Convert.ToString(client.Client.RemoteEndPoint);
            Nickname = "Аноним";
            sender = new Sender(sessions, this);
            this.sessions = sessions;
            _commands = commands;
        }
        /// <summary>
        /// Начать сессию
        /// </summary>
        internal async void Start()
        {
            await Task.Run(async () => 
            {
                try
                {
                    Stream = client.GetStream();
                    _commands["/имя"].Invoke(this, Array.Empty<string>());
                    await sender.Notification(Nickname +
                            " подключился к чату", NickColor, UserGroup.Others);
                    while (true)
                    {
                        string message = WaitInput();
                        if (message[0] == '/')
                        {
                            if (_commands.ContainsKey(message))
                            {
                                string[] parameters = message.Split(' ').Skip(1).ToArray();
                                _commands[message].Invoke(this, parameters);
                            }
                            else
                                await sender.Notification
                                    ($"Неизвестная команда \"{message}\"", ConsoleColor.White, UserGroup.This);
                        }
                        else
                            await sender.SingleMessage
                                (new Message($"{Nickname}: ", message, NickColor), UserGroup.Others);
                    }
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("Удаленный хост принудительно " +
                        "разорвал существующее подключение"))
                    Console.WriteLine(e.Message + "\n" + e.GetType() + "\n" + e.StackTrace);
                }
                await Delete();
            });            
        }

        /// <summary>
        /// Ожидание сообщения от пользователя
        /// </summary>
        /// <returns>Полученное сообщение</returns>
        internal string WaitInput()
        {
            Stream = client.GetStream();
            byte[] bytes = new byte[512];
            int len = Stream.Read(bytes, 0, bytes.Length);
            string message = Encoding.UTF8
                .GetString(bytes, 0, len);
            Console.ForegroundColor = NickColor;
            Console.Write(Nickname + ": ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(message + "\n");
            return message;
        }
        /// <summary>
        /// Удаление текущей сессии
        /// </summary>
        /// <returns></returns>
        public async Task Delete()
        {
            Message msg = new(Nickname + " покинул чат\n", null, NickColor);
            await sender.SingleMessage(msg, UserGroup.Others);
            Console.WriteLine(Nickname + " покинул чат");
            sessions.Remove(this);
        }
    }
}
