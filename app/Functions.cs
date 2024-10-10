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
            var categories = Enum.GetValues(typeof(ENums.Status)).Cast<ENums.Status>();

            var table = new Table().Border(TableBorder.Double).Title("{TODO}: {TEST BOARD}").BorderColor(Color.Purple);
            foreach (var category in categories)
            {
                string colorizedCategory = category switch
                {
                    Common.ENums.Status.Backlog => "[yellow]Backlog[/]",
                    Common.ENums.Status.Todo => "[blue]Todo[/]",
                    Common.ENums.Status.Doing => "[green]Doing[/]",
                    Common.ENums.Status.Done => "[magenta]Done[/]",
                    Common.ENums.Status.Frozen => "[cyan1]Frozen[/]",
                    _ => category.ToString()
                };

                table.AddColumn(colorizedCategory);
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
                        ? $@"[bold fuchsia]Title:[/] [white]{todo.Title}[/]
[bold fuchsia]Description:[/] [white]{todo.Description}[/]
[bold fuchsia]Status:[/] [white]{todo.TodoStatus}[/]"
                        : string.Empty;

                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        var panel = new Panel(content)
                            .Border(BoxBorder.Ascii).BorderColor(Color.Cyan1).Header($"[bold cyan]ID: [white]{todo.Id}[/][/]", Justify.Center);
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
                    $"[purple]Enter the new [fuchsia]title[/] for the todo, or type '-' to keep the current one ([italic white]{todo.Title}[/]): [/]"
                );

                if (!newTitle.Equals("-", StringComparison.OrdinalIgnoreCase))
                {
                    todo.Title = newTitle;
                }

                var newDescription = AnsiConsole.Ask<string>(
                    $"[purple]Enter the new [fuchsia]description[/] for the todo, or type '-' to keep the current one ([italic white]{todo.Description}[/]): [/]"
                );

                if (!newDescription.Equals("-", StringComparison.OrdinalIgnoreCase))
                {
                    todo.Description = newDescription;
                }

                var newStatus = AnsiConsole.Prompt(
                    new SelectionPrompt<ENums.Status>()
                        .Title($"[purple]Select the new [fuchsia]status[/] for the todo ([italic white]{todo.TodoStatus}[/]):[/]")
                        .AddChoices(Enum.GetValues(typeof(ENums.Status)).Cast<ENums.Status>())
                );

                if (!newStatus.Equals(todo.TodoStatus))
                {
                    todo.TodoStatus = newStatus;
                }

                SaveToFile(Common.TodosFilePath, todos);
                AnsiConsole.MarkupLine("[green]Todo updated successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Todo not found![/]");
            }
        }

        public static void RemoveTodoFunction(string _, string __)
        {
            Todos todos = LoadTodos();

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

            LuaFluxTodoItem todo = todos.Items.FirstOrDefault(t => t.Id == selectedTodo.Id);

            if (todo != null)
            {
                AnsiConsole.MarkupLine("[purple]Are you sure you want to [bold red]REMOVE[/] this ToDo? [/]");

                Panel todoPanel = new Panel($"[purple]Title: [white]{todo.Title}[/]\nDescription: [white]{todo.Description}[/]\nID: [white]{todo.Id}[/]\nStatus: [green]{todo.TodoStatus.ToString()}[/][/]").Header($"[purple]ToDo: [white]{todo.Id}[/][/]").HeaderAlignment(Justify.Center);
                AnsiConsole.Write(todoPanel);

                var confirm = AnsiConsole.Prompt(new SelectionPrompt<bool>().Title($"[red]Are you sure you want to remove todo:[/] [bold white]{todo.Id}: {todo.Title}[/]?").AddChoices(true, false).HighlightStyle(Color.Fuchsia));

                if (confirm)
                {
                    todos.Items.Remove(todo);
                    SaveToFile(Common.TodosFilePath, todos);
                    AnsiConsole.MarkupLine($"[green]Todo '{todo.Title}' has been successfully removed.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[yellow]Action cancelled. Todo was not removed.[/]");
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Todo not found![/]");
            }
        }
    }

}
