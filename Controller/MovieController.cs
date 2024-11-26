using Cinema.Models;
using Cinema.Views;
using System;
using System.Linq;

namespace Cinema.Controllers
{
    public class MovieController
    {
        private readonly IDatabase database;
        private readonly IMovieView view;

        public MovieController(IDatabase database, IMovieView view)
        {
            this.database = database;
            this.view = view;
        }

        public void Run()
        {
            bool exit = false;
            while (!exit)
            {
                string action = view.ShowMenu();
                switch (action)
                {
                    case "Dodaj film":
                        AddFilm();
                        break;
                    case "Usuń film":
                        RemoveFilm();
                        break;
                    case "Przeglądaj bazę":
                        ViewFilms();
                        break;
                    case "Powrót":
                        exit = true;
                        break;
                }
            }
        }

        private void AddFilm()
        {
            Film newFilm = view.GetFilmDetails();
            database.AddFilm(newFilm);
            Console.WriteLine("Film został dodany do bazy.");
        }

        private void RemoveFilm()
        {
            string filmName = view.GetFilmNameToRemove(database.FilmsList);
            bool success = database.FilmsList.RemoveAll(f => f.Name.Equals(filmName, StringComparison.OrdinalIgnoreCase)) > 0;
            view.ShowRemovalResult(success);
        }

        private void ViewFilms()
        {
            view.DisplayMovies(database.FilmsList);
        }
    }
}
