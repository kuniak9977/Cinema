﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Cinema.Models;

namespace Cinema
{
    public class Database
    {
        private List<Film> filmsList;
        private List<Room> roomList;
        private List<KeyValuePair<Employee, string>> employeeRole;
        private Dictionary<string, string> sala_film = new Dictionary<string, string>();

        public List<Film> FilmsList { get => filmsList; set => filmsList = value; }
        public List<Room> RoomList { get => roomList; set => roomList = value; }
        public List<KeyValuePair<Employee,string>> EmployeeRole { get => employeeRole; set => employeeRole = value; }
        public Dictionary<string,string> Sala_film { get => sala_film; set => sala_film = value; }

        public Database()
        {
            filmsList = new List<Film>();
            roomList = new List<Room>();
            employeeRole = new List<KeyValuePair<Employee, string>>();
        }

        void LoadOrCreateDatabase(string _path)
        {
            if (!File.Exists(_path))
            {
                return new Database();
            }

            string json = File.ReadAllText(_path);
            var option = new JsonSerializerOptions { WriteIndented = true, Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<Database>(json, option);
        }

        public void AddEmployee(string _name, string _surname, short _code, string _role)
        {
            var emp = new Employee(_name, _surname, _code);
            employeeRole.Add(KeyValuePair.Create(emp, _role));
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
