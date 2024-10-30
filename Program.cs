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

            Database database = LoadOrCreateDatabase(path);

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
            int opt = ShowAdminPanel();

            switch (opt)
            {
                case 1:
                    MoviePanel(database);
                    break;
                case 2:
                    RoomPanel(database);
                    break;
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

        void ShowRepertoireCalendar()
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
            int option = 1;
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
                switch (option)
                {
                    case 1:
                        tableComlumn1 = new TableColumn(new Markup($"[lime]>Filmy<[/]")).Centered();
                        tableComlumn2 = new TableColumn(new Markup($"[aqua]Sale[/]")).Centered();
                        tableComlumn3 = new TableColumn(new Markup($"[aqua]Pracownicy[/]")).Centered();
                        break;
                    case 2:
                        tableComlumn1 = new TableColumn(new Markup($"[aqua]Filmy[/]")).Centered();
                        tableComlumn2 = new TableColumn(new Markup($"[lime]>Sale<[/]")).Centered();
                        tableComlumn3 = new TableColumn(new Markup($"[aqua]Pracownicy[/]")).Centered();
                        break;
                    case 3:
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
                if (key.Key == ConsoleKey.RightArrow)
                    option++;
                else if (key.Key == ConsoleKey.LeftArrow)
                    option--;
                else if ( key.Key == ConsoleKey.Enter)
                    isChoosing = false;
            }
            return option;
        }

        void MoviePanel(Database _database)
        {
            List<Film> films = _database.FilmsList;
            int posY = 14;

            Table movies = new Table();
            movies.Expand();
            movies.Border(TableBorder.Horizontal);

            TableColumn tb1 = new TableColumn("[bold] Nazwa filmu:[/]");
            TableColumn tb2 = new TableColumn("[bold] Opis:[/]");
            TableColumn tb3 = new TableColumn("[bold] Gatunek:[/]");
            TableColumn tb4 = new TableColumn("[bold] Czas odtwarzania:[/]");
            TableColumn tb5 = new TableColumn("[bold] Oceny:[/]");

            movies.AddColumns(tb1,tb2,tb3,tb4,tb5);

            foreach (Film film in films)
            {
                movies.AddRow($"{film.Name}", $"{film.Description}", $"{film.Type}", $"{WriteFilmLength(film.LengthSec)}", $"{film.Points}/10");
            }

            AnsiConsole.Write(movies);
            int newPosY = Console.CursorTop;

            var wtf = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz działanie na bazie:")
                .AddChoices(new[] {"Dodaj film", "Usuń film", "Przeglądaj bazę", "Powrót" }));

            ClearConsolepart(posY, newPosY);
            switch (wtf)
            {
                case "Dodaj film":
                    AddingFilm(_database);
                    ClearConsolepart(posY, newPosY + 20);
                    break;
                case "Usuń film":
                    RemovingFilm(_database);
                    break;
                case "Powrót":
                    ShowAdminPanel();
                    break;
            }
            MoviePanel(_database);
        }

        string WriteFilmLength(int _sec)
        {
            int M = (_sec / 60) % 60;
            int S = _sec % 60;
            int H = M / 60;
            return $"{H}:{M}:{S}";
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

        void AddingFilm(Database _database)
        {
            Console.Write("Dodawanie filmu. Wprowadzane dane potwierdzaj Enter\n");
            Console.WriteLine("Podaj nazwę filmu. Nie może być pusty!");
            string name = null;
            while (name == null)
            {
                name = Console.ReadLine();
            }
            Console.WriteLine("Wprowadź opis filmu:");
            string desc = Console.ReadLine();
            Console.WriteLine("Gatunek filmu:");
            string type = Console.ReadLine();
            Console.WriteLine("Czas trwania w sekundach");
            int length = int.Parse(Console.ReadLine());
            Console.WriteLine("Oceny filmu");
            double points = double.Parse(Console.ReadLine());
            Film newFilm = new Film(name, desc, type, length, points);
            _database.AddFilm(newFilm);
        }

        bool RemovingFilm(Database _database)
        {
            List<Film> list = _database.FilmsList;

            Console.WriteLine("Podaj tytuł filmu do usunięcia. (Dokładny)");
            string _name = Console.ReadLine();

            var filmToRemove = list.FirstOrDefault(f => f.Name.Equals(_name, StringComparison.OrdinalIgnoreCase));
            if (filmToRemove != null)
            {
                list.Remove(filmToRemove);
                _database.FilmsList = list;
                return true;
            }
            return false;
        }

        void RoomPanel(Database _database)
        {
            Console.SetCursorPosition(0, 13);
            ClearConsolepart(13,30);
            Console.WriteLine("Zarządzanie salami. Najpierw wybierasz sale, a następnie czynność.");

            List<Room> list = _database.RoomList;
            string[] rooms= new string[list.Count];
            int i = 0;
            foreach (Room room in list)
            {
                rooms[i++] = room.Name;
            }

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz sale:")
                .AddChoices(rooms));

            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz działanie")
                .AddChoices(new[] {"Przegląd sali", "Zaplanuj film", "Zobacz historię filmów"}));

            RoomReview(_database, selection);
        }

        void RoomReview(Database _database, string _selection)
        {
            Console.SetCursorPosition(0, 13);

            List<Room> list = _database.RoomList;

            int col = 22;

            Grid roomgrid = new Grid();
            roomgrid.Expand();
            roomgrid.Centered();
            roomgrid.Width(Console.WindowWidth);

            Markup taken = new Markup("{[yellow]X[/]}");
            Markup free = new Markup("{ }");
            Markup broken = new Markup("{[Red]B[/]}");

            Room room = list.Find(r => r.Name == _selection);

            string[] gridColumsHeader = { "", "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI", "XVII", "XVIII", "XIX", "XX", "" };
            roomgrid.AddColumns(22);
            roomgrid.AddRow(gridColumsHeader);
            roomgrid.Alignment(Justify.Center);
            
            int quantity = room.ChairsQuantity;
            int checkedSeats = 0;
            
            for (int i = 0; i < quantity; i++)
            {
                Markup[] insertRow = new Markup[col];
                insertRow[0] = new Markup("");
                insertRow[21] = new Markup("");
                for (int j = 1; j < col -1; j++)
                {
                    if (room.States[checkedSeats] == Room.State.Free)
                    {
                        insertRow[j] = free;
                    } else if (room.States[checkedSeats] == Room.State.Taken)
                    {
                        insertRow[j] = taken;
                    }else
                    {
                        insertRow[j] = broken;
                    }
                    checkedSeats++;
                    quantity--;
                }

                roomgrid.AddRow(insertRow);
                
            }
            
            AnsiConsole.Write(roomgrid);
        }
    }
}
