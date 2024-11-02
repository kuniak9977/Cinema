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
                .AddChoices(new[] { "Przegląd sali", "Zaplanuj film", "Zobacz historię filmów" }));

            switch (action)
            {
                case "Przegląd sali":
                    RoomReview(_database, selection);
                    break;
                case "Zaplanuj film":
                    PlanMovie(_database, selection);
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
            ShowPlannedMovies(_selection, _database.Sala_film);
            Console.WriteLine();
            AnsiConsole.Write(new Markup("[Red]Naciśnij dowolny przycisk aby kontynuować![/]"));
            Console.ReadKey();
        }

        void ClearConsolepart(int _oldY, int _newY)
        {
            Console.SetCursorPosition(0, _oldY);
            int width = Console.WindowWidth;
            for (int i = _oldY; i <= _newY; i++)
            {
                Console.Write(new string(' ', width));
            }
            Console.SetCursorPosition(0, _oldY);
        }

        void PlanMovie(Database _database, string _selection)
        {
            List<Film> films = _database.FilmsList;
            string[] filmy = new string[films.Count];
            short i = 0;
            foreach (Film film in films)
            {
                filmy[i++] = film.ToString(film.LengthSec);
            }

            var movieToAddToRoom = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz film który chcesz dodać do kolejki:")
                .AddChoices(filmy));

            _database.AddMovieToRoom(_selection, movieToAddToRoom);
            /*
            if (added)
                AnsiConsole.Write(new Markup("[Green]Udało się dodać film do kolejki odtwarzania[/]"));
            else
                AnsiConsole.Write(new Markup("[Red]Nie udało się dodać filmu. Wystąpił błąd![/]"));*/
        }

        void ShowPlannedMovies(string _room, Dictionary<string, List<string>> _sala_film)
        {
            if (_sala_film.ContainsKey(_room))
            {
                string[] plannedMovies = _sala_film[_room].ToArray();
                var grid = new Grid();
                grid.AddColumn();
                grid.AddRow(plannedMovies);
                AnsiConsole.Write(grid);
            }
            else
            {
                Console.WriteLine("Brak zaplanowanych filmów dla tej sali.");
            }
        }
    }
}
