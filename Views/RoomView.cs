using Spectre.Console;
using Cinema.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cinema.Views
{
    public class RoomView
    {
        public string SelectRoom(List<Room> rooms)
        {
            var roomNames = rooms.Select(r => r.Name).ToList();
            roomNames.Add("Powrót");

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Wybierz salę:")
                    .AddChoices(roomNames));
        }

        public string SelectRoomAction()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Wybierz działanie:")
                    .AddChoices("Przegląd sali", "Zaplanuj film"));
        }

        public void DisplayRoomGrid(Room room)
        {
            int col = 22; // Liczba kolumn w siatce
            var grid = new Grid();
            grid.Expand().Centered().Width(Console.WindowWidth);

            // Dodajemy kolumny do siatki
            for (int i = 0; i < col; i++)
            {
                grid.AddColumn(new GridColumn());
            }

            // Markup dla różnych stanów foteli
            Markup taken = new Markup("{[yellow]X[/]}");
            Markup free = new Markup("{ }");
            Markup broken = new Markup("{[Red]B[/]}");
            Markup empty = new Markup(" ");

            int checkedSeats = 0;

            // Dodajemy rzędy do siatki na podstawie stanu foteli
            for (int i = 0; i < room.ChairsQuantity / (col - 2); i++) // Uwzględniamy kolumny pomocnicze (2)
            {
                var row = new Markup[col];
                row[0] = empty; // Lewa kolumna pusta
                row[col - 1] = empty; // Prawa kolumna pusta

                for (int j = 1; j < col - 1; j++)
                {
                    if (checkedSeats < room.ChairsQuantity)
                    {
                        row[j] = room.States[checkedSeats] switch
                        {
                            Room.State.Free => free,
                            Room.State.Taken => taken,
                            Room.State.Broken => broken,
                            _ => empty
                        };
                        checkedSeats++;
                    }
                    else
                    {
                        row[j] = empty;
                    }
                }

                grid.AddRow(row);
            }

            AnsiConsole.Write(grid);
        }


        public void DisplayPlannedMovies(Room room, List<RoomMovies> moviesInRoom)
        {
            // Filtruj filmy przypisane do konkretnej sali
            var roomMoviesList = moviesInRoom.Where(rm => rm.Room == room).ToList();

            if (roomMoviesList.Any())
            {
                var grid = new Grid().AddColumn();

                // Iteruj przez każdą salę filmów
                foreach (var roomMovies in roomMoviesList)
                {
                    foreach (var movieTime in roomMovies.MovieDuration)
                    {
                        grid.AddRow($"Film: {movieTime.Movie.Name}, Start: {movieTime.StartTime:HH:mm}, Koniec: {movieTime.EndTime:HH:mm}");
                    }
                }

                // Wyświetl siatkę z filmami
                AnsiConsole.Write(grid);
            }
            else
            {
                Console.WriteLine("Brak zaplanowanych filmów dla tej sali.");
            }
        }

        public void ShowError(string message)
        {
            AnsiConsole.Write(new Markup($"[Red]{message}[/]\n"));
        }

        public string SelectMovie(List<Film> films)
        {
            if (films.Count == 0)
                return null;

            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Wybierz film, który chcesz dodać do kolejki:")
                    .AddChoices(films.Select(f => f.Name)));
        }

        public DateTime GetMovieStartTime()
        {
            TimeInput timeInput = new TimeInput();
            return timeInput.DrawTimeInputPanel();
        }
    }
}
