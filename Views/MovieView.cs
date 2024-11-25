using Spectre.Console;
using Cinema.Models;
using System;
using System.Collections.Generic;

namespace Cinema.Views
{
    public class MovieView
    {
        public string ShowMenu()
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Wybierz działanie na bazie:")
                    .AddChoices("Dodaj film", "Usuń film", "Przeglądaj bazę", "Powrót"));
        }

        public void DisplayMovies(List<Film> films)
        {
            Table movies = new Table();
            movies.Expand();
            movies.Border(TableBorder.Horizontal);

            movies.AddColumns(
                new TableColumn("[bold]Nazwa filmu:[/]"),
                new TableColumn("[bold]Opis:[/]"),
                new TableColumn("[bold]Gatunek:[/]"),
                new TableColumn("[bold]Czas odtwarzania:[/]"),
                new TableColumn("[bold]Oceny:[/]"));

            foreach (var film in films)
            {
                movies.AddRow(
                    $"{film.Name}",
                    $"{film.Description}",
                    $"{film.Type}",
                    $"{film.WriteFilmLength(film.LengthSec)}",
                    $"{film.Points}/10");
            }

            AnsiConsole.Write(movies);
            Console.WriteLine(" Dowolny przycisk aby kontynuoać...");
            Console.ReadKey();
            Console.SetCursorPosition(0, 13);
            for (int i = 0; i < Console.WindowHeight; i++)
            {
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 13);
        }

        public Film GetFilmDetails()
        {
            Console.Write("Dodawanie filmu. Wprowadzane dane potwierdzaj Enter.\n");
            Console.WriteLine("Podaj nazwę filmu (nie może być pusta):");
            string name;
            while (string.IsNullOrWhiteSpace(name = Console.ReadLine())) { }

            Console.WriteLine("Wprowadź opis filmu:");
            string description = Console.ReadLine();

            Console.WriteLine("Podaj gatunek filmu:");
            string genre = Console.ReadLine();

            Console.WriteLine("Podaj czas trwania filmu w sekundach:");
            int duration = int.Parse(Console.ReadLine());

            Console.WriteLine("Podaj ocenę filmu (używaj przecinka):");
            double rating = double.Parse(Console.ReadLine());

            return new Film(name, description, genre, duration, rating);
        }

        public string GetFilmNameToRemove()
        {
            Console.WriteLine("Podaj tytuł filmu do usunięcia (dokładny):");
            return Console.ReadLine();
        }

        public void ShowRemovalResult(bool success)
        {
            if (success)
                Console.WriteLine("Film został usunięty.");
            else
                Console.WriteLine("Nie znaleziono filmu o podanej nazwie.");
        }
    }
}
