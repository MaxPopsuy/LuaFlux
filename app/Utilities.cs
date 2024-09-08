using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LuaFlux.Commands;

namespace LuaFlux
{
    internal class Utilities
    {
        public static void LuaFluxScreen()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"██╗     ██╗   ██╗ █████╗ ███████╗██╗     ██╗   ██╗██╗  ██╗ ");
            Console.WriteLine($"██║     ██║   ██║██╔══██╗██╔════╝██║     ██║   ██║╚██╗██╔╝ ");
            Console.WriteLine($"██║     ██║   ██║███████║█████╗  ██║     ██║   ██║ ╚███╔╝  ");
            Console.WriteLine($"██║     ██║   ██║██╔══██║██╔══╝  ██║     ██║   ██║ ██╔██╗  v - {Common.LuaFluxVersion}");
            Console.WriteLine("███████╗╚██████╔╝██║  ██║██║     ███████╗╚██████╔╝██╔╝ ██╗ ");
            Console.WriteLine($"╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚══════╝ ╚═════╝ ╚═╝  ╚═╝ ");

            Console.WriteLine("Write h or help for list of commands, welcome to LuaFlux!");

            Console.ResetColor();
        }

        public static void LuaFluxWaitAnimation()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(">>");
            Thread.Sleep(50);
            Console.Write("\b");
            Console.Write(">>");
            Thread.Sleep(50);
            Console.Write("\b");
            Console.Write(">>");
            Thread.Sleep(50);
            Console.Write("\b");
            Console.Write(": ");
        }

        public static string LuaFluxGetCommandFromAlias(string command)
        {
            foreach (KeyValuePair<string, string[]> entry in _commandsAliases)
            {
                if (entry.Value.Contains(command))
                {
                    return entry.Key;
                }
            }
            return command;
        }

        public static void LuaFluxWrite(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }
}