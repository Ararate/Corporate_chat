using Commons;
using Server.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Основной контроллер
    /// </summary>
    [Controller]
    internal class MainController : Controller
    {
        /// <summary>
        /// Процедура изменения имени
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [Command("/имя")]
        public bool ChangeName(string[] parameters)
        {
            User.sender.Notification("Введите имя", ConsoleColor.White, UserGroup.This);
            string tempNickname = User.WaitInput();
            bool rightAnswer = false;
            do
            {
                if (Users.FirstOrDefault(x => x.Nickname == tempNickname) != null)
                    User.sender.Notification("Имя занято",
                        ConsoleColor.White, UserGroup.This);
                else
                    rightAnswer = true;
            }
            while (!rightAnswer);
            User.Nickname = tempNickname;
            List<Message> messages = new()
            {
                new Message("Ок", ConsoleColor.White),
                new Message("Выберите цвет ника", ConsoleColor.White)
            };
            var colorList = Enum.GetNames
                (typeof(ConsoleColor));
            int i = 0;
            foreach (string color in colorList)
            {
                messages.Add(new Message($"{i++}) {color}", null,
                    (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color)));
            }
            messages.RemoveAt(0);
            User.sender.MessageList(messages, UserGroup.This);
            int colorNumber;
            rightAnswer = false;
            do
            {
                string colorNumberStr = User.WaitInput();
                if (!int.TryParse(colorNumberStr, out colorNumber) ||
                    colorNumber < 1 || colorNumber > 15)
                    User.sender.Notification("Нет", ConsoleColor.White, UserGroup.This);
                else
                    rightAnswer = true;
            }
            while (!rightAnswer);
            User.NickColor = (ConsoleColor)Enum.ToObject(typeof(ConsoleColor), colorNumber);
            var messages1 = new List<Message>()
            {
                new Message("", $"Добро пожаловать, {User.Nickname}", ConsoleColor.White),
                new Message("", "Чтобы сменить имя пиши /имя", User.NickColor),
            };
            User.sender.MessageList(messages1, UserGroup.This);
            return true;
        }
    }
}
