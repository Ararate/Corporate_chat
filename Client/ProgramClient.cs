﻿using Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    /// <summary>
    /// Начало программы
    /// </summary>
    class ProgramClient
    {
        static TcpClient client;
        static NetworkStream stream = null;
        static Listener Listener;

        static internal void Main(string[] args)
        {
            Console.WriteLine("Введите адрес хоста");
            do
            {
                try
                {
                    client = new(Console.ReadLine(), 7000);
                }
                catch {
                    Console.WriteLine("Не удаётся подключиться, повторите попытку");
                }
            }
            while (client == null || !client.Connected);
            Console.WriteLine("Подключение установлено");
            Listener = new (client);
            Listener.Listen();
            try
            {
                while (true)
                {
                    SendMessage(Console.ReadLine());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// Отправить сообщение на сервер
        /// </summary>
        /// <param name="request" >Текст сообщения</param>
        static void SendMessage(string request)
        {
            byte[] bytes;
            stream = client.GetStream();
            bytes = Encoding.UTF8.GetBytes(request);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }
    }
}
