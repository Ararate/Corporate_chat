using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server 
{
    /// <summary>
    /// Сессия для отдельного пользователя
    /// </summary>
    internal class ClientSession : IDisposable
    {
        internal string nickname;
        internal string ipAdress;
        internal ConsoleColor nickColor = ConsoleColor.White;
        internal byte[] bytesRead = new byte[512];
        internal byte[] bytesWrite = new byte[512];
        internal readonly TcpClient client;
        internal NetworkStream stream;
        internal int len = 0;
        internal string userMsg = "";
        internal ClientSession(TcpClient client) 
        {
            this.client = client;
            ipAdress = Convert.ToString(client.Client.RemoteEndPoint);
            nickname = ipAdress;
        }
        /// <summary>
        /// Начать сессию
        /// </summary>
        internal async void StartSession()
        {
            
            await Task.Run(() => 
            {
                try
                {
                    stream = client.GetStream();
                    ChangeName();
                    foreach (ClientSession client in ProgramServer.
                        SessionList.Where(i => i.ipAdress != ipAdress))
                    {
                        client.SendServerMessage(nickname + 
                            " подключился к чату\n", null, nickColor);
                    }
                    while (true)
                    {
                        if( WaitMessage(true) == "/имя")
                            ChangeName();
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("Удаленный хост принудительно " +
                        "разорвал существующее подключение"))
                    {
                        Console.WriteLine(nickname + " покинул чат");
                        foreach (ClientSession client in ProgramServer.
                        SessionList.Where(i => i.ipAdress != ipAdress))
                        {
                            client.SendServerMessage
                            (nickname + " покинул чат\n", null, nickColor);
                        }
                    }
                    else
                    {
                        Console.WriteLine(e.Message + "\n" + e.StackTrace);
                    }
                }
                Dispose();
            });            
        }
        /// <summary>
        /// Отправить сообщение пользователю
        /// </summary>
        /// <param name="colorText"></param>
        /// <param name="whiteText"></param>
        /// <param name="msgColor"></param>
        void SendServerMessage(string colorText, 
            string whiteText, ConsoleColor msgColor)
        {
            Answer answer = new(whiteText, colorText, msgColor);
            bytesWrite = answer.ToByteArray();
            stream.Write(bytesWrite, 0, bytesWrite.Length);
            stream.Flush();
            stream = client.GetStream();
        }
        /// <summary>
        /// Процедура смены имени
        /// </summary>
        void ChangeName()
        {
            SendServerMessage("Как вас зовут?\n",null, ConsoleColor.White);
        repeatNickName:
            string tempNick = WaitMessage(false);
            foreach (var user in ProgramServer.SessionList)
            {
                if (user.nickname == tempNick)
                {
                    SendServerMessage("Такой человек уже есть\n",
                        null, ConsoleColor.White);
                    goto repeatNickName;
                }
            }
            nickname = tempNick;
            SendServerMessage("Ок\nВыберите цвет ника \n",
                null, ConsoleColor.White);
            List<string> colorList = Enum.GetNames
                (typeof(ConsoleColor)).ToList();
            int i = 0;
            colorList.RemoveAt(0);
            Thread.Sleep(200);
            foreach (string col in colorList)
            {
                SendServerMessage($"{++i}) {col}\n", null, 
                    (ConsoleColor)Enum.Parse(typeof(ConsoleColor), col));
                Thread.Sleep(200);
            }
            ColorChoose:
            int colorNumber;
            string colorNumberStr = WaitMessage(false);
            if (!int.TryParse(colorNumberStr, out colorNumber) ||
            colorNumber < 1 || colorNumber > 15)
            {
                SendServerMessage("Нет\n", null, ConsoleColor.Red);
                goto ColorChoose;
            }
            nickColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor),
                Enum.GetName(typeof(ConsoleColor), colorNumber));
            SendServerMessage("Добро пожаловать, ", null, ConsoleColor.White);
            SendServerMessage(nickname+"\n", null, nickColor);
            Thread.Sleep(150);
            SendServerMessage("Чтобы сменить имя пиши /имя\n",
                null, ConsoleColor.White);
        }
        /// <summary>
        /// Ожидание сообщения от пользователя
        /// </summary>
        /// <param name="sendToOthers"></param>
        /// <returns>Сообщение пользователя</returns>
        string WaitMessage(bool sendToOthers)
        {
            stream = client.GetStream();
            len = stream.Read(bytesRead, 0, bytesRead.Length);
            userMsg = Encoding.UTF8
            .GetString(bytesRead, 0, len);
            Console.ForegroundColor = nickColor;
            Console.Write(nickname + ": ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(userMsg + "\n");
            if (sendToOthers)
            {
                foreach(ClientSession client in ProgramServer.
                    SessionList.Where(i => i.ipAdress != ipAdress))
                {
                    client.SendServerMessage
                        (nickname+": ", userMsg+"\n", nickColor);
                }
            }
            return userMsg;
        }

        public void Dispose()
        {
            ProgramServer.SessionList.Remove(this);
        }
    }
}
