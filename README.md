# Многопользовательский анонимный чат
Программа позволяет обмениваться сообщениями внутри сети посредством сервера, используя сетевую модель tcp/ip. <br>
При запуске ChatServer выдаёт айпи адреса по которым клиент может подключиться к нему. Для соединения можно использовать роутер, wifi, vpn, logmein hamacci и т.д.<br>
К чату может присоединяться неограниченное число пользователей. 

#  Код
По сути воссоздан паттерн MVC. Можно добавлять контроллеры, которые для работы должны быть отмечены аттрибутом controller и наследоваться от базового класса controller. 
Методы в контроллерах должны быть отмечены аттрибутом command(Команда).
Они могут ожидать ввод пользователя и отправлять в ответ сообщения. <br>
Пример сложения двух чисел:
```cs
  namespace Server
  {
      [Controller]
      internal class MainController : Controller
      {
          [Command("/сумма")]
          public bool ChangeName(string[] parameters)
          {
            int a, b;
            if (!int.TryParse(parameters[0], out a) || !int.TryParse(parameters[1], out b))
            {
                User.sender.Notification("Неправильный ввод", ConsoleColor.White, UserGroup.This);
                return false;
            }
            int res = a + b;
            User.sender.Notification(res.ToString(), ConsoleColor.White, UserGroup.This);
            User.sender.Notification($"{User.Nickname} сложил {a} и {b} и получил {res}", ConsoleColor.White, UserGroup.All);
            return true;
          }
      }
  }
