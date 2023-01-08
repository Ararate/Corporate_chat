using Client;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class ProgramClient
    {
        static byte[] bytesRead = new byte[512];
        static byte[] bytesWrite = new byte[256];
        static TcpClient client = new TcpClient();
        static NetworkStream stream = null;
        static Answer ans;

        static void Main(string[] args)
        {
            Console.WriteLine("Введите адрес сервера");
            while (!client.Connected)
            try
            {
                client = new(Console.ReadLine(), 7000);
            }
            catch
            {
                Console.WriteLine("Не удалось подключиться," +
                    " повторите попытку");
            }
            Console.WriteLine("Подключение установлено");
            try
            {
                Listen();
                SendMessage("");
                while (true)
                {
                    SendMessage(Console.ReadLine());
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            client.Close();
            Console.ReadKey();
        }
        /// <summary>
        /// Отправить сообщение на сервер
        /// </summary>
        /// <param name="request" >Текст сообщения</param>
        static void SendMessage(string request)
        {
            stream = client.GetStream();
            bytesWrite = Encoding.UTF8.GetBytes(request);
            stream.Write(bytesWrite, 0, bytesWrite.Length);
            stream.Flush();
        }
        /// <summary>
        /// Прослушивание сообщений от сервера
        /// </summary>
        static async void Listen()
        {
            await Task.Run(() =>
            {
                try
                {
                    while (true)
                    {
                        stream = client.GetStream();
                        stream.Read(bytesRead, 0, bytesRead.Length);
                        ans = new(bytesRead);

                        Console.ForegroundColor = ans.color;
                        Console.Write(ans.coloredText);
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(ans.whiteText);

                        SendMessage("");
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
                }
            });
        }
    }
}
