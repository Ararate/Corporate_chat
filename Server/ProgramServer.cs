using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    class ProgramServer
    {
        public static List<ClientSession> SessionList = new();
        static TcpClient client;
        static Task Main(string[] args)
        {
            TcpListener socket = new(IPAddress.Any, 7000);
            socket.Start();
            Console.WriteLine("Сервер запущен по адресу "+ 
                Dns.GetHostEntry(Dns.GetHostName()).AddressList[0]);
            while (true)
            {
                client = socket.AcceptTcpClient();
                Console.WriteLine("Новое подключение: " 
                    + client.Client.RemoteEndPoint);
                SessionList.Add(new ClientSession(client));
                SessionList[^1].StartSession();
            }


        }
    }
}
