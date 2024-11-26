using Cinema.Models;
using Cinema;
using Spectre.Console;
using Cinema.Views;

public class MovieView : IMovieView
{
    private readonly IConsoleWrapper _consoleWrapper;
    private readonly IAnsiConsoleWrapper _ansiConsoleWrapper;

    // Konstruktor z interfejsami
    public MovieView(IConsoleWrapper consoleWrapper, IAnsiConsoleWrapper ansiConsoleWrapper)
    {
        _consoleWrapper = consoleWrapper ?? new ConsoleWrapper();
        _ansiConsoleWrapper = ansiConsoleWrapper ?? new AnsiConsoleWrapper();
    }

    public string ShowMenu()
    {
        return _ansiConsoleWrapper.Prompt(
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

        _ansiConsoleWrapper.Write(movies);
        _consoleWrapper.WriteLine(" Dowolny przycisk aby kontynuoać...");
        _consoleWrapper.ReadKey();
        _consoleWrapper.SetCursorPosition(0, 13);
        for (int i = 0; i < _consoleWrapper.WindowWidth; i++)
        {
            _consoleWrapper.Write(new string(' ', _consoleWrapper.WindowWidth));
        }
        _consoleWrapper.SetCursorPosition(0, 13);
    }

    public Film GetFilmDetails()
    {
        _consoleWrapper.Write("Dodawanie filmu. Wprowadzane dane potwierdzaj Enter.\n");
        _consoleWrapper.WriteLine("Podaj nazwę filmu (nie może być pusta):");
        string name;
        while (string.IsNullOrWhiteSpace(name = _consoleWrapper.ReadLine())) { }

        _consoleWrapper.WriteLine("Wprowadź opis filmu:");
        string description = _consoleWrapper.ReadLine();

        _consoleWrapper.WriteLine("Podaj gatunek filmu:");
        string genre = _consoleWrapper.ReadLine();

        _consoleWrapper.WriteLine("Podaj czas trwania filmu w sekundach:");
        TimeInput timeInput = new TimeInput();
        DateTime dateTime = timeInput.DrawTimeInputPanel();
        int duration = (int)dateTime.TimeOfDay.TotalSeconds;

        _consoleWrapper.WriteLine("Podaj ocenę filmu (używaj przecinka):");
        double rating = double.Parse(_consoleWrapper.ReadLine());

        return new Film(name, description, genre, duration, rating);
    }

    public string GetFilmNameToRemove(List<Film> _list)
    {
        string[] listsOfMovies = new string[_list.Count];
        int i = 0;
        foreach (Film film in _list)
        {
            listsOfMovies[i++] = film.Name;
        }

        var selection = _ansiConsoleWrapper.Prompt(
            new SelectionPrompt<string>()
                .Title("Wybierz film do usunięcia:")
                .AddChoices(listsOfMovies));
        return selection;
    }

    public void ShowRemovalResult(bool success)
    {
        if (success)
            _consoleWrapper.WriteLine("Film został usunięty.");
        else
            _consoleWrapper.WriteLine("Nie znaleziono filmu o podanej nazwie.");
    }
}
