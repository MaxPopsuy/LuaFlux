using Spectre.Console;
using static LuaFlux.Utilities;
using static LuaFlux.Common.ENums;

namespace LuaFlux
{
    internal class Functions
    {
        public static void TestFunction(string _, string __)
        {
            Utilities.LuaFluxWrite(ConsoleColor.Magenta, "Welcome to LuaFlux!");
        }
        public static void ViewTodosFunction(string _, string __)
        {
            Todos todos = LoadTodos();
            var categories = Enum.GetValues(typeof(Category)).Cast<Category>();

            var table = new Table().Border(TableBorder.Double).Title("{TODO}: {TEST BOARD}");
            foreach (var category in categories)
            {
                table.AddColumn(category.ToString());
            }

            int maxTodos = todos.Items
                .GroupBy(t => t.TodoCategory)
                .Max(g => g.Count());

            for (int i = 0; i < maxTodos; i++)
            {
                var row = new List<Panel>();

                foreach (var category in categories)
                {
                    var todo = todos.Items
                        .Where(t => t.TodoCategory == category)
                        .Skip(i)
                        .FirstOrDefault();

                    var content = todo != null
                        ? $@"[bold]ID:[/] {todo.Id}
[bold]Title:[/] {todo.Title}
[bold]Description:[/] {todo.Description}
[bold]Status:[/] {todo.TodoStatus}"
                        : string.Empty;

                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        var panel = new Panel(content)
                            .Border(BoxBorder.Ascii);
                        row.Add(panel);
                    }
                    else
                    {
                        row.Add(new Panel(" ") { Border = BoxBorder.None });
                    }
                }

                table.AddRow(row.ToArray());
            }

            AnsiConsole.Write(table);
        }
    }
}
