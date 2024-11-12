using Cinema.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    internal class MoviePanelAdm
    {
        public MoviePanelAdm() { }

        public bool MoviePanel(Database _database)
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

            movies.AddColumns(tb1, tb2, tb3, tb4, tb5);

            foreach (Film film in films)
            {
                movies.AddRow($"{film.Name}", $"{film.Description}", $"{film.Type}", $"{film.WriteFilmLength(film.LengthSec)}", $"{film.Points}/10");
            }

            AnsiConsole.Write(movies);
            int newPosY = Console.CursorTop;

            var wtf = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Wybierz działanie na bazie:")
                .AddChoices(new[] { "Dodaj film", "Usuń film", "Przeglądaj bazę", "Powrót" }));

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
                    return true;
            }
            return false;
        }

        public void AddingFilm(Database _database)
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
            Console.WriteLine("Oceny filmu (Używaj przecinku)");
            double points = double.Parse(Console.ReadLine());
            Film newFilm = new Film(name, desc, type, length, points);
            _database.AddFilm(newFilm);
        }

        public bool RemovingFilm(Database _database)
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
    }
}
