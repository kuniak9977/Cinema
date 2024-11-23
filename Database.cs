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
    public class Database
    {
        private List<Film> filmsList;
        private List<Room> roomList;
        private List<Employee> employeeList;
        private Dictionary<string, List<string>> sala_film;

        private List<RoomMovies> moviesInRoom;


        public List<RoomMovies> MoviesInRoom { get => moviesInRoom; set => moviesInRoom = value; }
        public List<Film> FilmsList { get => filmsList; set => filmsList = value; }
        public List<Room> RoomList { get => roomList; set => roomList = value; }
        public List<Employee> EmployeeList { get => employeeList; set => employeeList = value; }
        public Dictionary<string,List<string>> Sala_film { get => sala_film; set => sala_film = value; }

        public Database()
        {
            filmsList = new List<Film>();
            roomList = new List<Room>();
            employeeList = new List<Employee>();
            sala_film = new Dictionary<string, List<string>>();
            moviesInRoom = new List<RoomMovies>();
        }

        public void AddEmployee(string _name, string _surname, short _code, int _role)
        {
            var emp = new Employee(_name, _surname, _code, _role);
            SortByRole(employeeList);
        }

        public void AddFilm(Film _film)
        {
            filmsList.Add(_film);
        }
        public void AddMovieToRoom(string _room, string _movie)
        {
            if (sala_film.ContainsKey(_room))
                sala_film[_room].Add(_movie);
            else
                sala_film[_room] = new List<string> { _movie };
        }
        public void AddMovieToRoom(Room _room, string _movie)
        {
            Film selected;
            foreach (Film movie in FilmsList)
            {
                if (_movie == movie.Name)
                    selected = movie;
                break;
            }


            /*
            MovieTime tmp = new MovieTime();
            RoomMovies temp = new RoomMovies();
            moviesInRoom.Add();*/
        }

        public void AddRoom(Room _room)
        {
            roomList.Add(_room);
        }

        public void RemoveFilm(Film _film)
        {
            filmsList.Remove(_film);
        }

        public List<Film> GetFilmList()
        {
            return filmsList;
        }

        public List<Room> GetRoomList()
        {
            return roomList;
        }
    }
}
