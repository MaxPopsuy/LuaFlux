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

            [JsonConverter(typeof(StringEnumConverter))]
            public Status TodoStatus { get; set; } = Status.Backlog;
            [JsonConverter(typeof(StringEnumConverter))]
            public Category TodoCategory { get; set; } = Category.Backlog;
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
            public enum Category
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
                ["view"] = ["v"]
            };


            public static Dictionary<string, Action<string, string>> _commands = new Dictionary<string, Action<string, string>>()
            {
                ["test"] = TestFunction,
                ["view"] = ViewTodosFunction,
            };
        }
    }
}