using Spectre.Console;
using static LuaFlux.Utilities;
using static LuaFlux.Common.ENums;
using static LuaFlux.Common;

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
            var categories = Enum.GetValues(typeof(Common.ENums.Status)).Cast<Common.ENums.Status>();

            var table = new Table().Border(TableBorder.Double).Title("{TODO}: {TEST BOARD}");
            foreach (var category in categories)
            {
                table.AddColumn(category.ToString());
            }

            int maxTodos = todos.Items
                .GroupBy(t => t.TodoStatus)
                .Max(g => g.Count());

            for (int i = 0; i < maxTodos; i++)
            {
                var row = new List<Panel>();

                foreach (var category in categories)
                {
                    var todo = todos.Items
                        .Where(t => t.TodoStatus == category)
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
        internal static void CreateTodoFunction(string _, string __)
        {
            var title = AnsiConsole.Ask<string>("[purple]Enter the [fuchsia]title[/] of the todo: [/]");
            var description = AnsiConsole.Ask<string>("[purple]Enter the [fuchsia]description[/] of the todo: [/]");

            var status = AnsiConsole.Prompt(
                new SelectionPrompt<ENums.Status>()
                    .Title("[purple]Select the [fuchsia]status[/] of the todo[/]")
                    .AddChoices(Enum.GetValues(typeof(ENums.Status)).Cast<ENums.Status>()));

            var todos = LoadTodos();

            todos.Items.Add(new LuaFluxTodoItem
            {
                Id = GenerateTodoId(todos.Items),
                Title = title,
                Description = description,
                TodoStatus = status,
            });

            SaveToFile(Utilities.TodosFilePath, todos);

            AnsiConsole.MarkupLine("[fuchsia]Todo added successfully![/]");
        }
        public static void EditTodoFunction(string _, string __)
        {
            var todos = LoadTodos();

            var displayItems = todos.Items.Select(t => new TodoDisplayItem
            {
                Id = t.Id,
                Title = t.Title
            }).ToList();

            var selectedTodo = AnsiConsole.Prompt(
                new SelectionPrompt<TodoDisplayItem>()
                    .Title("[purple]Select a todo to edit:[/]")
                    .AddChoices(displayItems)
            );

            var todo = todos.Items.FirstOrDefault(t => t.Id == selectedTodo.Id);

            if (todo != null)
            {
                var newTitle = AnsiConsole.Ask<string>(
                    $"[purple]Enter the new [fuchsia]title[/] for todo, or type '&skip' to keep it ([italic]{todo.Title}[/]): [/]"
                );

                if (string.IsNullOrWhiteSpace(newTitle) || newTitle.Equals("&skip", StringComparison.OrdinalIgnoreCase))
                {
                    newTitle = todo.Title;
                }

                var newDescription = AnsiConsole.Ask<string>(
                    $"[purple]Enter the new [fuchsia]description[/] for todo, or type '&skip' to keep it ([italic]{todo.Description}[/]): [/]"
                );

                if (string.IsNullOrWhiteSpace(newDescription) || newDescription.Equals("&skip", StringComparison.OrdinalIgnoreCase))
                {
                    newDescription = todo.Description;
                }

                var newStatus = AnsiConsole.Prompt(
                    new SelectionPrompt<ENums.Status>()
                        .Title($"[purple]Select the new [fuchsia]status[/] for the todo ([italic]{todo.TodoStatus}[/]), or type '&skip' to keep it:[/]")
                        .AddChoices(Enum.GetValues(typeof(ENums.Status)).Cast<ENums.Status>())
                );

                if (!newStatus.Equals(todo.TodoStatus))
                {
                    todo.TodoStatus = newStatus;
                }

                todo.Title = newTitle;
                todo.Description = newDescription;

                SaveToFile(Common.TodosFilePath, todos);
                AnsiConsole.MarkupLine("[green]Todo updated successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Todo not found![/]");
            }
        }
    }

}
