using Spectre.Console;
using System.Text.Json;
using Cinema.Models;

namespace Cinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var program = new Program();
            program.Run(args);
        }

        public void Run(string[] args)
        {
            Console.CursorVisible = false;
            string path = "CinemaDB.json";
            bool isWorking = true;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;

            Database database = LoadOrCreateDatabase(path);

            //Employee employee = new Employee("Młocigrzyb", "Ćwik", 1111, 2);
            //Console.WriteLine(employee.ToString());
            //Testowanie dodania filmu do bazy
            /*
            var film = new Film("Pogoń za oceną", "Naj naj film o utracie chęci do życia", "Dramat", 6490, 4.5);
            database.AddFilm(film);
            var test = new Room("Sala100", 180);
            var test2 = new Room("Sala343", 134);
            var test3 = new Room("Sala234", 256);
            var test4 = new Room("Sala13", 80);
            database.AddRoom(test);
            database.AddRoom(test2);
            database.AddRoom(test3);
            database.AddRoom(test4);
            database.AddEmployee("Maria","Kowalska", 1234, "kasjerka");
            
            SaveDatabase(database ,path);
            */
            ShowTitlePage();
            string role = ChooseRole();
            int opt;
            bool work = false;
            while (isWorking)
            {
                switch (opt = ShowAdminPanel())
                {
                    case 0:
                        MoviePanelAdm p = new MoviePanelAdm();
                        while (!work)
                        {
                            work = p.MoviePanel(database);
                        }
                        work = false;
                        break;
                    case 1:
                        RoomPanelAdm r = new RoomPanelAdm();
                        while (!work)
                        {
                            work = r.RoomPanel(database);
                        }
                        work = false;
                        break;
                    case 2:
                        WorkersPanel w = new WorkersPanel();
                        while (!work)
                            work = w.WorkersReview(database.EmployeeList);
                        work = false;
                        break;
                    case 3:
                        isWorking = false;
                        break;
                }
            }
            
            SaveDatabase(database, path);
            /*
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
            }*/
        }

        void ShowAsciiArt()
        {
            AnsiConsole.Write(
                new FigletText("CinemaStar")
                .Centered()
                .Color(Color.Gold1));
        }

        void ShowTitlePage()
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

        string ChooseRole()
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

        Database LoadOrCreateDatabase(string _path)
        {
            if (!File.Exists(_path))
            {
                return new Database();
            }

            string json = File.ReadAllText(_path);
            var option = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<Database>(json, option);
        }

        void SaveDatabase(Database _database, string _path)
        {
            var option = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true };
            string json = JsonSerializer.Serialize(_database, option);

            //Console.WriteLine(json);

            File.WriteAllText(_path, json);
        }

        bool PromptForEmployeeCode()
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

        int ShowAdminPanel()
        {
            AnsiConsole.Clear();
            Console.Clear();

            bool isChoosing = true;
            string[] option = { "Filmy", "Sale", "Pracownicy" };
            int selectedOption = 0;
            ConsoleKeyInfo key;

            Markup moviePanel = new Markup("Możliwe opcje do zrobienia w tym panelu:\n" +
                "Dodawanie filmow do bazy\n" +
                "Usuwanie filmow\n" +
                "Przegląd bazy.");

            Markup roomPanel = new Markup("Możliwe opcje do zrobienia w tym panelu:\n" +
                "Dodawanie sal\n" +
                "Usuwanie sal\n" +
                "Przegląd bazy" +
                "Modyfikowanie");

            Markup workersPanel = new Markup("Możliwe opcje do zrobienia w tym panelu:\n" +
                "Dodawanie pracowników\n" +
                "Usuwanie pracowników\n" +
                "Przegląd bazy" +
                "Modyfikowanie zapisów");

            var title = new TableTitle("Cinema Manager Panel");
            title.SetStyle(Style.WithForeground(Color.Green));

            Table mainTable;

            TableColumn tableComlumn1 = new TableColumn("");
            TableColumn tableComlumn2 = new TableColumn("");
            TableColumn tableComlumn3 = new TableColumn("");

            var tablePanelMovie = new Panel(moviePanel);
            tablePanelMovie.Header = new PanelHeader("[Red]Panel do obsługi filmów[/]");

            var tablePanelRoom = new Panel(roomPanel);
            tablePanelRoom.Header = new PanelHeader("[Red]Przegląd sal kinowych[/]");

            var tablePanelEmployers = new Panel(workersPanel);
            tablePanelEmployers.Header = new PanelHeader("[Red]Baza danych pracowników[/]");

            while (isChoosing)
            {
                mainTable = new Table();
                mainTable.Title = title;
                mainTable.Expand();
                mainTable.BorderColor(Color.Yellow4).Border(TableBorder.Rounded);

                //AnsiConsole.Clear();
                Console.SetCursorPosition(0, 0);
                switch (selectedOption)
                {
                    case 0:
                        tableComlumn1 = new TableColumn(new Markup($"[lime]>Filmy<[/]")).Centered();
                        tableComlumn2 = new TableColumn(new Markup($"[aqua]Sale[/]")).Centered();
                        tableComlumn3 = new TableColumn(new Markup($"[aqua]Pracownicy[/]")).Centered();
                        break;
                    case 1:
                        tableComlumn1 = new TableColumn(new Markup($"[aqua]Filmy[/]")).Centered();
                        tableComlumn2 = new TableColumn(new Markup($"[lime]>Sale<[/]")).Centered();
                        tableComlumn3 = new TableColumn(new Markup($"[aqua]Pracownicy[/]")).Centered();
                        break;
                    case 2:
                        tableComlumn1 = new TableColumn(new Markup($"[aqua]Filmy[/]")).Centered();
                        tableComlumn2 = new TableColumn(new Markup($"[aqua]Sale[/]")).Centered();
                        tableComlumn3 = new TableColumn(new Markup($"[lime]>Pracownicy<[/]")).Centered();
                        break;
                }

                mainTable.AddColumn(tableComlumn1);
                mainTable.AddColumn(tableComlumn2);
                mainTable.AddColumn(tableComlumn3);

                mainTable.AddRow(tablePanelMovie, tablePanelRoom, tablePanelEmployers);

                AnsiConsole.Write(mainTable);

                key = Console.ReadKey();
                if (key.Key == ConsoleKey.Escape)
                    return 3;
                if (key.Key == ConsoleKey.RightArrow)
                    selectedOption = (selectedOption + 1) % option.Length;
                else if (key.Key == ConsoleKey.LeftArrow)
                    selectedOption = (selectedOption - 1 + option.Length) % option.Length;
                else if (key.Key == ConsoleKey.Enter)
                    isChoosing = false;
            }
            return selectedOption;
        }

        void ClearConsolepart(int _oldY, int _newY)
        {
            Console.SetCursorPosition(0, _oldY);
            int width = Console.WindowWidth;
            for (int i = _oldY; i <= _newY; i++)
            {
                Console.Write(new string (' ', width));
            }
            Console.SetCursorPosition(0, _oldY);
        }
    }
}
