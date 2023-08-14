using System.Collections.Generic;

namespace Server.Controllers
{
    /// <summary>
    /// Базовый контроллер
    /// </summary>
    internal abstract class Controller
    {
        /// <summary>
        /// Сессия пользователя, вызвавшего команду
        /// </summary>
        protected Session User { get; set; }
        /// <summary>
        /// Список всех сессий
        /// </summary>
        protected List<Session> Users { get; set; }
    }
}
