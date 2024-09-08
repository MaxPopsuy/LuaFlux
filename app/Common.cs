using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static LuaFlux.Functions;

namespace LuaFlux
{
    internal class Common
    {
        internal static string LuaFluxAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
        internal static string LuaFluxVersion = LuaFluxAssemblyVersion.Substring(0, LuaFluxAssemblyVersion.Length - 2);
    }
    internal class Commands
    {
        public static Dictionary<string, string[]> _commandsDsc = new() // string[] = ["arguments", "description"]
        {
            ["help"] = ["", "shows every command and short description of them"],
            ["clear"] = ["", "clears console. (aliases: clr, cls)"],
        };

        public static Dictionary<string, string[]> _commandsAliases = new()
        {
            ["help"] = ["h", "help", "g"],
            ["clear"] = ["clr", "cls", "clear"],
            ["test"] = ["t"],
        };


        public static Dictionary<string, Action<string, string>> _commands = new Dictionary<string, Action<string, string>>()
        {
            ["test"] = TestFunction,
        };
    }
}
