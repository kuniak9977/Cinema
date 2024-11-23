using Cinema.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    internal class RoomPanelAdm
    {
        public RoomPanelAdm() { }

        public bool RoomPanel(Database _database)
        {
            Console.SetCursorPosition(0, 13);
            ClearConsolepart(13, 30);
            Console.WriteLine("Zarządzanie salami. Najpierw wybierasz sale, a następnie czynność.");

            List<Room> list = _database.RoomList;
            string[] rooms = new string[list.Count];
            int i = 0;
            foreach (Room room in list)
            {
                rooms[i++] = room.Name;
            }
            
            string selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz sale:")
                .AddChoices(rooms)
                .AddChoices(new[] {"Powrót"}));

            if (selection == "Powrót")
                return true;

            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz działanie")
                .AddChoices(new[] { "Przegląd sali", "Zaplanuj film"}));

            switch (action)
            {
                case "Przegląd sali":
                    RoomReview(_database, selection);
                    break;
                case "Zaplanuj film":
                    PlanMovie2(_database, selection);
                    break;
            }

            return false;
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
            Markup empty = new Markup(" ");

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
                for (int j = 1; j < col - 1; j++)
                {
                    if (checkedSeats < room.ChairsQuantity)
                    {
                        if (room.States[checkedSeats] == Room.State.Free)
                        {
                            insertRow[j] = free;
                        }
                        else if (room.States[checkedSeats] == Room.State.Taken)
                        {
                            insertRow[j] = taken;
                        }
                        else if (room.States[checkedSeats] == Room.State.Broken)
                        {
                            insertRow[j] = broken;
                        }
                        checkedSeats++;
                        quantity--;
                    } else
                    { insertRow[j] = empty;}
                    
                }

                roomgrid.AddRow(insertRow);

            }

            AnsiConsole.Write(roomgrid);
            ShowPlannedMovies(room, _database.MoviesInRoom);
            Console.WriteLine();
            AnsiConsole.Write(new Markup("[Red]Naciśnij ENTER aby kontynuować![/]"));
            Console.ReadKey();
        }

        public void ClearConsolepart(int _oldY, int _newY)
        {
            Console.SetCursorPosition(0, _oldY);
            int width = Console.WindowWidth;
            for (int i = _oldY; i <= _newY; i++)
            {
                Console.Write(new string(' ', width));
            }
            Console.SetCursorPosition(0, _oldY);
        }

        void PlanMovie2(Database _database, string _selection)
        {
            List<Room> rooms = _database.RoomList;

            // Znajdujemy pokój na podstawie nazwy
            Room selectedRoom = rooms.FirstOrDefault(room => room.Name == _selection);

            if (selectedRoom == null)
            {
                Console.WriteLine("Nie znaleziono pokoju o podanej nazwie.");
                return;
            }

            // Pobieramy nazwy filmów dla użytkownika do wyboru
            var movies = _database.FilmsList.Select(f => f.Name).ToList();

            // Użytkownik wybiera film
            var movieName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Wybierz film, który chcesz dodać do kolejki:")
                    .AddChoices(movies));

            // Znajdujemy film na podstawie nazwy
            Film movieToAddToRoom = _database.GetFilmByName(movieName);

            if (movieToAddToRoom == null)
            {
                Console.WriteLine("Nie znaleziono filmu o podanej nazwie.");
                return;
            }

            // Użytkownik wybiera czas rozpoczęcia
            TimeInput timeInput = new TimeInput();
            DateTime start = timeInput.DrawTimeInputPanel();

            // Dodajemy film do pokoju
            _database.AddMovieToRoom(selectedRoom, movieToAddToRoom, start);
        }

        void ShowPlannedMovies(Room _room, List<RoomMovies> _moviesInRoom)
        {
            // Znajdujemy obiekt RoomMovies dla podanego pokoju
            var roomMovies = _moviesInRoom.FirstOrDefault(rm => rm.room == _room);

            if (roomMovies != null && roomMovies.movieDuration.Any())
            {
                // Tworzymy grid do wyświetlania zaplanowanych filmów
                var grid = new Grid();
                grid.AddColumn(); // Kolumna na dane filmu

                foreach (var movieTime in roomMovies.movieDuration)
                {
                    var movieInfo = $"Film: {movieTime.movie.Name}, Start: {movieTime.startTime:HH:mm}, Koniec: {movieTime.endTime:HH:mm}";
                    grid.AddRow(movieInfo);
                }

                AnsiConsole.Write(grid);
            }
            else
            {
                // Gdy brak zaplanowanych filmów
                Console.WriteLine("Brak zaplanowanych filmów dla tej sali.");
            }
        }
    }
}
