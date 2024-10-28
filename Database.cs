using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Models;

namespace Cinema
{
    public class Database
    {
        private List<Film> filmsList;
        private List<Room> roomList;
        private Dictionary<Employee, string> employeeRole;

        public List<Film> FilmsList { get => filmsList; set => filmsList = value; }
        public List<Room> RoomList { get => roomList; set => roomList = value; }
        public Dictionary<Employee,string> EmployeeRole { get => employeeRole; set => employeeRole = value; }

        public Database()
        {
            filmsList = new List<Film>();
            roomList = new List<Room>();
            employeeRole = new Dictionary<Employee, string>();
        }

        public void AddEmployee(string _name, string _surname, short _code, string _role)
        {
            var emp = new Employee(_name, _surname, _code);
            employeeRole.Add(emp, _role);
        }

        public void AddFilm(Film _film)
        {
            filmsList.Add(_film);
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
