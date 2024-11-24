using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class RoomMovies
    {
        public Room Room { get; set; }  // Zmieniono na właściwość
        public List<MovieTime> MovieDuration { get; set; }  // Zmieniono na właściwość

        // Konstruktor musi mieć parametry zgodne z nazwami właściwości
        public RoomMovies(Room room, List<MovieTime> movieDuration)
        {
            Room = room;
            MovieDuration = movieDuration;
        }

        // Parametr `_movieDuration` powinien być listą, nie pojedynczym elementem
        public RoomMovies(Room room, MovieTime movieDuration)
            : this(room, new List<MovieTime> { movieDuration })
        {
        }

        // Dodanie bezparametrowego konstruktora na potrzeby serializacji, jeśli wymagane
        public RoomMovies() { }
    }
}
