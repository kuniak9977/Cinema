using Cinema.Models;
using Cinema.Views;
using System;
using System.Linq;

namespace Cinema.Controllers
{
    public class RoomController
    {
        private readonly Database database;
        private readonly RoomView view;

        public RoomController(Database database, RoomView view)
        {
            this.database = database;
            this.view = view;
        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                string selectedRoom = view.SelectRoom(database.RoomList);
                if (selectedRoom == "Powrót")
                {
                    exit = true;
                    continue;
                }

                string action = view.SelectRoomAction();
                switch (action)
                {
                    case "Przegląd sali":
                        ShowRoomDetails(selectedRoom);
                        break;
                    case "Zaplanuj film":
                        PlanMovieInRoom(selectedRoom);
                        break;
                }
            }
        }

        private void ShowRoomDetails(string roomName)
        {
            var room = database.RoomList.FirstOrDefault(r => r.Name == roomName);
            if (room == null)
            {
                view.ShowError("Nie znaleziono sali.");
                return;
            }

            view.DisplayRoomGrid(room);
            view.DisplayPlannedMovies(room, database.MoviesInRoom);
        }

        private void PlanMovieInRoom(string roomName)
        {
            var room = database.RoomList.FirstOrDefault(r => r.Name == roomName);
            if (room == null)
            {
                view.ShowError("Nie znaleziono sali.");
                return;
            }

            string selectedMovie = view.SelectMovie(database.FilmsList);
            var movie = database.FilmsList.FirstOrDefault(f => f.Name == selectedMovie);
            if (movie == null)
            {
                view.ShowError("Nie znaleziono filmu.");
                return;
            }

            DateTime startTime = view.GetMovieStartTime();
            database.AddMovieToRoom(room, movie, startTime);
        }
    }
}
