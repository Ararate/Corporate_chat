using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// Обозначает команду
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    internal class CommandAttribute : Attribute
    {
        /// <summary>
        /// Строка, которой вызывается команда
        /// </summary>
        internal string Command { get; }
        public CommandAttribute(string command)
        {
            Command = command;
        }
    }
}
