using Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Server
{
    /// <summary>
    /// Начало программы
    /// </summary>
    class ProgramServer
    {
        /// <summary>
        /// Команды и выполняемые процедуры
        /// </summary>
        private static readonly Dictionary<string, Action<Session, string[]>> commands = new();
        /// <summary>
        /// Список сессий
        /// </summary>
        private static readonly List<Session> sessions = new();
        /// <summary>
        /// Main
        /// </summary>
        static void Main()
        {
            TcpListener socket = new(IPAddress.Any, 7000);
            socket.Start();
                        
            foreach(var a in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                Console.WriteLine(a.ToString());
            }
            MapCommands();
            while (true)
            {
                TcpClient client = socket.AcceptTcpClient();
                Console.WriteLine("Новое подключение: " 
                    + client.Client.RemoteEndPoint);
                sessions.Add(new Session(client, sessions, commands));
                sessions[^1].Start();
            }
        }
        /// <summary>
        /// Маппинг команд из контроллеров
        /// </summary>
        private static void MapCommands()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(ControllerAttribute), true).Length <= 0)
                    continue;
                MethodInfo[] methods = type
                .GetMethods()
                .Where(x => x.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                .ToArray();
                foreach (MethodInfo method in methods)
                {
                    commands.Add(((CommandAttribute)method
                        .GetCustomAttribute(typeof(CommandAttribute))).Command, (session, parameters) =>
                    {
                        try
                        {
                            Controller obj = (Controller)Activator.CreateInstance(type, null);

                            type.GetProperty("User", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, session);
                            type.GetProperty("Users", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(obj, sessions);
                            method.Invoke(obj, new object[] { parameters });
                        }
                        catch
                        {
                        }
                    });
                }
            }
        }
    }
}
