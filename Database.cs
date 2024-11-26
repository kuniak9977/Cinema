using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Cinema.Models;
using static Cinema.Models.Employee;

namespace Cinema
{
    public class Database : IDatabase
    {
        private List<Film> filmsList;
        private List<Room> roomList;
        private List<Employee> employeeList;
        private List<RoomMovies> moviesInRoom;

        public List<RoomMovies> MoviesInRoom { get => moviesInRoom; set => moviesInRoom = value; }
        public List<Film> FilmsList { get => filmsList; set => filmsList = value; }
        public List<Room> RoomList { get => roomList; set => roomList = value; }
        public List<Employee> EmployeeList { get => employeeList; set => employeeList = value; }

        public Database()
        {
            filmsList = new List<Film>();
            roomList = new List<Room>();
            employeeList = new List<Employee>();
            moviesInRoom = new List<RoomMovies>();
        }
        
        public void AddEmployee(string _name, string _surname, short _code, int _role)
        {
            var emp = new Employee(_name, _surname, _code, _role);
            EmployeeList.Add(emp);
        }

        public void AddFilm(Film _film)
        {
            filmsList.Add(_film);
        }

        public void AddMovieToRoom(Room _room, Film _movie, DateTime _start)
        {
            MovieTime mt = new MovieTime(_movie, _start);
            RoomMovies rm = new RoomMovies(_room, mt);
            moviesInRoom.Add(rm);
        }
        public Film GetFilmByName(string filmName)
        {
            // Znajdujemy film na podstawie nazwy, ignorując wielkość liter
            return FilmsList.FirstOrDefault(f => f.Name.Equals(filmName, StringComparison.OrdinalIgnoreCase));
        }

        public void AddRoom(Room _room)
        {
            roomList.Add(_room);
        }

        public void RemoveFilm(Film _film)
        {
            filmsList.Remove(_film);
        }

    }
}
