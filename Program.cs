using Spectre.Console;
using System.Text.Json;

namespace Cinema
{
    internal class Program
    {
        public void Main(string[] args)
        {
            Console.CursorVisible = false;
            string path = "CinemaDB.json";

            Database database = LoadOrCreateDatabase(path);

            

            ShowTitlePage();
            string role = ChooseRole();

            if (role == "Gość")
            {
                ShowRepertoireCalendar();
            }
            else if (role == "Pracownik")
            {
                bool accessGranted = PromptForEmployeeCode();
                if (accessGranted)
                {
                    ShowAdminPanel();
                }
                else
                {
                    AnsiConsole.Markup("[red]Niepoprawny kod dostępu![/]");
                }
            }
        }

        static void ShowAsciiArt()
        {
            AnsiConsole.Write(
                new FigletText("CinemaStar")
                .Centered()
                .Color(Color.Gold1));
        }

        static void ShowTitlePage()
        {
            ShowAsciiArt();

            // Tworzenie ramki
            var desc = new Markup("[red]!To kino nie jest najlepsze ale nada się na wyjśćie z dziewczyną czy tam innym helikopterem bojowym![/]\n" +
                "U nas jest najdrożej ale za to najgorzej, a jakie zapaszki wydobywające się z WC\n" +
                "Witaj przybyszu i zanurz się w krainie cierpienia na kolejną niezapomnianą przygodę");
            desc.Justify(Justify.Center)
                .Overflow(Overflow.Fold);

            var menu = new Panel(desc)
                .Header("[bold blue]Witamy w CinemaStar[/]")
                .Border(BoxBorder.Double)
                .Expand();

            menu.HeaderAlignment(Justify.Center);

            AnsiConsole.Write(menu);
        }

        static string ChooseRole()
        {
            // Obliczanie szerokości konsoli dla wyśrodkowania
            int width = Console.WindowWidth;
            string[] options = { "Gość", "Pracownik" };
            int selectedOption = 0;

            // Zapamiętaj pozycję kursora, aby czyścić tylko opcje wyboru
            int startCursorTop = Console.CursorTop;

            // Pętla wyboru
            while (true)
            {
                // Ustawienie kursora na pozycję początkową
                Console.SetCursorPosition(0, startCursorTop);

                // Wyświetlenie opcji wyboru
                for (int i = 0; i < options.Length; i++)
                {
                    // Czyszczenie aktualnej linii
                    Console.SetCursorPosition(0, startCursorTop + i);
                    Console.Write(new string(' ', width));

                    // Wyśrodkowanie każdej opcji
                    string option = options[i];
                    Console.SetCursorPosition((width - option.Length) / 2, startCursorTop + i);

                    // Wyróżnienie wybranej opcji
                    if (i == selectedOption)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write($"> {option} <");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(option);
                    }
                }

                // Obsługa klawiszy strzałek i Enter
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    selectedOption = (selectedOption == 0) ? options.Length - 1 : selectedOption - 1;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    selectedOption = (selectedOption == options.Length - 1) ? 0 : selectedOption + 1;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    // Zwracamy wybraną opcję po naciśnięciu Enter
                    Console.SetCursorPosition(0, startCursorTop);
                    Console.Write(new string(' ', width * 10));
                    Console.SetCursorPosition(0, startCursorTop);
                    return options[selectedOption];
                }
            }
        }

        Database LoadOrCreateDatabase (string _path)
        {
            if (!File.Exists(_path))
            {
                return new Database();
            }

            string json = File.ReadAllText(_path);

            return JsonSerializer.Deserialize<Database>(json);
        }

        void SaveDatabase(Database _database, string _path)
        {
            var option = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(_database, option);

            File.WriteAllText(_path, json);
        }

        static void ShowRepertoireCalendar()
        {
            // Tworzenie kalendarza na dany miesiąc
            var calendar = new Calendar(DateTime.Now.Year, DateTime.Now.Month)
                .AddCalendarEvent(DateTime.Now.AddDays(1)) // Film A
                .AddCalendarEvent(DateTime.Now.AddDays(3)) // Film B
                .AddCalendarEvent(DateTime.Now.AddDays(5)) // Film C
                .Centered();

            // Wyświetlenie kalendarza
            AnsiConsole.Write(calendar);

            // Wyświetlenie opisów filmów pod kalendarzem
            AnsiConsole.MarkupLine("[green]Film A:[/] 1 dzień od teraz");
            AnsiConsole.MarkupLine("[green]Film B:[/] 3 dni od teraz");
            AnsiConsole.MarkupLine("[green]Film C:[/] 5 dni od teraz");
        }

        static bool PromptForEmployeeCode()
        {
            // Wyświetlenie promptu do wpisania kodu
            string code = AnsiConsole.Prompt(
                new TextPrompt<string>("Podaj kod pracownika: ").Secret());

            // Weryfikacja kodu
            if (code == "1234") // przykładowy kod
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void ShowAdminPanel()
        {
            // Panel administracyjny - na razie placeholder
            AnsiConsole.Markup("[bold green]Witamy w panelu administratorskim kina![/]");
        }
    }
}
