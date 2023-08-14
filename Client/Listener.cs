using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Commons;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Client.MessageHandlerRepository;
using Server;

namespace Client
{
    /// <summary>
    /// Прослушиватель входящих с сервера данных
    /// </summary>
    internal class Listener
    {
        private readonly TcpClient _client;
        public Listener(TcpClient client)
        {
            _client = client;
        }
        /// <summary>
        /// Начать прослушивание
        /// </summary>
        internal async void Listen()
        {
            await Task.Run(() =>
            {
                try
                {
                    while (_client.Connected)
                    {
                        NetworkStream stream = _client.GetStream();
                        byte[] size = new byte[2];
                        stream.Read(size);
                        byte[] bytes = new byte[size[1] + (size[0]*256)];
                        stream.Read(bytes);

                        IMessageHandler handler = new MessageHandler();
                        handler.Handle(bytes);
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("Удаленный хост принудительно " +
                        "разорвал существующее подключение"))
                    {
                        Console.WriteLine("Соединение с сервером потеряно");
                    }
                    else
                        Console.WriteLine(e.Message + "\n" + e.StackTrace);
                    ProgramClient.Main(null);
                }
            });
        }
    }
}
