using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class Film
    {
        private string name;
        private string description;
        private string type;
        private int lengthSec;
        private double points;

        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public string Type { get { return type; } set { type = value; } }
        public int LengthSec { get { return lengthSec; } set { lengthSec = value; } }
        public double Points { get { return points; } set { points = value; } }

        public Film(string _name, string _desc, string _type, int _lengthSec, double _points)
        {
            this.name = _name;
            this.description = _desc;
            this.type = _type;
            this.lengthSec = _lengthSec;
            this.points = _points;
        }

        public Film() { }

        public string ToString(int _time)
        {
            return $"Nazwa: {name}, Czas trwania: {WriteFilmLength(_time)}, Gatunek: {type}";
        }
        public string WriteFilmLength(int _sec)
        {
            int H = _sec / 3600;       // Liczba godzin
            int M = (_sec % 3600) / 60; // Liczba minut
            int S = _sec % 60; // Pozostałe sekundy
            return $"{H}:{M}:{S}";
        }
    }
}
