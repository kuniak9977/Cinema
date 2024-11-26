using Cinema.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public interface IConsoleWrapper
    {
        string ReadLine();
        ConsoleKeyInfo ReadKey();
        void WriteLine(string message);
        void Write(string message);
        void SetCursorPosition(int left, int top);
        int WindowWidth { get; }
    }

    public interface IAnsiConsoleWrapper
    {
        string Prompt(SelectionPrompt<string> prompt);
        List<string> MultiPrompt(MultiSelectionPrompt<string> prompt);
        void Write(Table table);
    }

    public interface IEmployeeView
    {
        string PromptAction();
        void DisplayEmployees(List<Employee> employees);
        Employee CollectEmployeeData();
        int ChooseEmployee(List<Employee> employees, string promptTitle);
    }

    public interface IMovieView
    {
        string ShowMenu();
        Film GetFilmDetails();
        string GetFilmNameToRemove(List<Film> films);
        void ShowRemovalResult(bool success);
        void DisplayMovies(List<Film> films);
    }

    public interface IDatabase
    {
        List<Film> FilmsList { get; set; }
        List<Room> RoomList { get; set; }
        List<Employee> EmployeeList { get; set; }
        List<RoomMovies> MoviesInRoom { get; set; }

        void AddEmployee(string name, string surname, short code, int role);
        void AddFilm(Film film);
        void RemoveFilm(Film film);
        Film GetFilmByName(string filmName);
        void AddRoom(Room room);
        void AddMovieToRoom(Room room, Film movie, DateTime start);
    }

}
