using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema
{
    public class Database
    {
        private List<Film> filmsList;
        private List<Room> roomList;

        public Database()
        {
            filmsList = new List<Film>();
            roomList = new List<Room>();
        }

        public void AddFilm(Film _film)
        {
            filmsList.Add(_film);
        }

        public void RemoveFilm(Film _film)
        {
            filmsList.Remove(_film);
        }

        public List<Film> GetFilmList()
        {
            return filmsList;
        }
    }

    public class Film
    {
        private string name;
        private string description;
        private string type;

        public Film (string _name, string _desc, string _type)
        {
            this.name = _name;
            this.description = _desc;
            this.type = _type;
        }

        public override string ToString()
        {
            return $"Nazwa: {name}, Opis: {description}, Gatunek: {type}";
        }
    }

    public class Room
    {
        private string name;
        private int chairsQuantity;
        private State[] states;

        public string Name { get { return name; } set { name = value; } }
        public int ChairsQuantity { get {  return chairsQuantity; } }

        public Room (string _name, int _quantity)
        {
            this.name = _name;
            this.chairsQuantity = _quantity;
            this.states = new State[this.chairsQuantity];

            for (int i = 0; i < this.chairsQuantity; i++)
            {
                this.states[i] = State.Free;
            }
        }

        public enum State {
            Free = 0,
            Taken = 1,
            Broken = 2
        };

        public State GetChairState (int _chairNumber)
        {
            State state = states[_chairNumber];
            return state;
        }

        public override string ToString()
        {
            return $"Nazwa: {name}, Ilość_miejsc: {chairsQuantity}, Zajętość: {states}";
        }
    }
}
