using System;
using System.Collections.Generic;
using System.IO;
using static LuaFlux.Common.Commands;
using static LuaFlux.Common;
using Newtonsoft.Json;

namespace LuaFlux
{
    internal class Utilities
    {
        public static readonly string TodosFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "LuaFlux", "todos.json");

        public static void LuaFluxScreen()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine($"██╗     ██╗   ██╗ █████╗ ███████╗██╗     ██╗   ██╗██╗  ██╗ ");
            Console.WriteLine($"██║     ██║   ██║██╔══██╗██╔════╝██║     ██║   ██║╚██╗██╔╝ ");
            Console.WriteLine($"██║     ██║   ██║███████║█████╗  ██║     ██║   ██║ ╚███╔╝  ");
            Console.WriteLine($"██║     ██║   ██║██╔══██║██╔══╝  ██║     ██║   ██║ ██╔██╗  v - {Common.LuaFluxVersion}");
            Console.WriteLine($"███████╗╚██████╔╝██║  ██║██║     ███████╗╚██████╔╝██╔╝ ██╗ ");
            Console.WriteLine($"╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚══════╝ ╚═════╝ ╚═╝  ╚═╝ ");

            Console.WriteLine("Write h or help for list of commands, welcome to LuaFlux!");

            Console.ResetColor();
        }

        public static void LuaFluxWaitAnimation()
        {
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write(">>");
            System.Threading.Thread.Sleep(50);
            Console.Write("\b");
            Console.Write(">>");
            System.Threading.Thread.Sleep(50);
            Console.Write("\b");
            Console.Write(">>");
            System.Threading.Thread.Sleep(50);
            Console.Write("\b");
            Console.Write(": ");
            Console.ResetColor();
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

        public static void LuaFluxInitializeTodos()
        {
            if (!Directory.Exists(Path.GetDirectoryName(TodosFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(TodosFilePath));
            }

            if (!File.Exists(TodosFilePath))
            {
                var defaultTodos = CreateDefault<Todos>();
                SaveToFile(TodosFilePath, defaultTodos);
            }
        }

        public static T CreateDefault<T>() where T : new()
        {
            var instance = new T();
            foreach (var property in typeof(T).GetProperties())
            {
                if (property.CanWrite)
                {
                    var defaultValue = property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null;
                    property.SetValue(instance, defaultValue);
                }
            }
            return instance;
        }

        public static Todos LoadTodos()
        {
            return LoadFromFile<Todos>(TodosFilePath);
        }

        public static T LoadFromFile<T>(string filePath) where T : new()
        {
            if (!File.Exists(filePath))
            {
                LuaFluxInitializeTodos();
            }

            try
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<T>(json) ?? CreateDefault<T>();
            }
            catch (JsonException ex)
            {
                LuaFluxWrite(ConsoleColor.Red, $"Failed to deserialize file. Error: {ex.Message}");
                return CreateDefault<T>();
            }
        }

        public static void SaveToFile<T>(string filePath, T obj)
        {
            string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }

    internal class Todos
    {
        public List<LuaFluxTodoItem> Items { get; set; } = new List<LuaFluxTodoItem>();

        public void AddTodo(string title, string description)
        {
            Items.Add(new LuaFluxTodoItem { Title = title, Description = description });
        }

        public void RemoveTodoAt(int index)
        {
            if (index >= 0 && index < Items.Count)
            {
                Items.RemoveAt(index);
            }
        }
    }
}
