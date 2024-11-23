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
            bool work = false;
            bool accesGranted = false;
            Console.WindowHeight = 50;
            Console.WindowWidth = 160;

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            Database database = LoadOrCreateDatabase(path);

            database.AddEmployee("Młocigrzyb", "Ćwik", 1111, 2);
            database.AddEmployee("Kurzonoga", "Penis", 1221, 2);
            //Testowanie dodania filmu do bazy
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
            database.AddEmployee("Maria","Kowalska", 1234, 1);
            
            SaveDatabase(database ,path);
            ShowTitlePage();
            

            while (!accesGranted)
            {
                accesGranted = InsertCode(database.EmployeeList);
            }

            while (isWorking)
            {
                switch (ShowAdminPanel())
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

        bool InsertCode(List<Employee> _list)
        {
            ClearConsolepart(13, 30);
            string text = "Wprowadź kod pracownika aby wejść do menadżera";
            int consoleWidth = Console.WindowWidth;
            Console.SetCursorPosition((consoleWidth/2) - (text.Length/2), Console.CursorTop );
            Console.Write(text + "\n");
            return PromptForEmployeeCode(_list);

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

        bool PromptForEmployeeCode(List<Employee> _list)
        {
            // Wyświetlenie promptu do wpisania kodu
            int code = AnsiConsole.Prompt(
                new TextPrompt<int>("Podaj kod pracownika: ").Secret());

            foreach (Employee emp in _list)
            {
                if (emp.EmployeePrivateCode == code)
                    return true;
            }
            Console.WriteLine();
            AnsiConsole.Write(new Markup($"[Red]Niepoprawny kod. Spróbuj pownownie![/]"));
            return false;
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
                "Przegląd bazy.");

            Markup roomPanel = new Markup("Możliwe opcje do zrobienia w tym panelu:\n" +
                "Dodawanie filmów do odworzenia\n" +
                "Przegląd stanu sal");

            Markup workersPanel = new Markup("Możliwe opcje do zrobienia w tym panelu:\n" +
                "Dodawanie pracowników\n" +
                "Przegląd bazy\n" +
                "Dodawanie podwładnych");

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
                Console.WriteLine();
                Console.WriteLine("Aby wyjść kliknij ESC...");

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
