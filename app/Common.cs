using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static LuaFlux.Common.ENums;
using static LuaFlux.Functions;


namespace LuaFlux
{
    internal class Common
    {
        internal static string TodosFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "LuaFlux", "todos.json");
        internal static string SettingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "LuaFlux", "settings.json");

        internal static string LuaFluxAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version.ToString();
        internal static string LuaFluxVersion = LuaFluxAssemblyVersion.Substring(0, LuaFluxAssemblyVersion.Length - 2);

        internal List<LuaFluxTodoItem> Todos { get; set; } = new List<LuaFluxTodoItem>();

        public class LuaFluxTodoItem
        {
            public int Id { get; set; } = 0;
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";


            public Status TodoStatus { get; set; } = Status.Backlog;
        }
        public class TodoDisplayItem
        {
            public int Id { get; set; }
            public string Title { get; set; }

            public override string ToString() => $"{Id}: {Title}";
        }

        public class ENums
        {
            public enum Status
            {
                Backlog,
                Todo,
                Doing,
                Done,
                Frozen
            }            
        }

        internal class Commands
        {
            public static Dictionary<string, string[]> _commandsDsc = new() // string[] = ["parameters", "description"]
            {
                ["help"] = ["", "shows every command and short description of them"],
                ["clear"] = ["", "clears console."],
                ["view"] = ["", "show list of all todos"],
                ["create"] = ["", "create new todo"],
                ["edit"] = ["", "edit existing todo"],
                ["delete"] = ["", "removes existing todo"],
            };

            public static Dictionary<string, string[]> _commandsAliases = new()
            {
                ["help"] = ["h", "g"],
                ["clear"] = ["clr", "cls"],
                ["test"] = ["t"],
                ["view"] = ["v"],
                ["create"] = ["c"],
            };


            public static Dictionary<string, Action<string, string>> _commands = new Dictionary<string, Action<string, string>>()
            {
                //["help"] = HelpFunction,
                ["test"] = TestFunction,
                ["view"] = ViewTodosFunction,
                ["create"] = CreateTodoFunction,
                ["edit"] = EditTodoFunction,
                //["delete"] = ,
            };
        }
    }
}