using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class MovieTime
    {
        public Film Movie { get; set; }  // Zmieniono na właściwość z wielką literą
        public DateTime StartTime { get; set; }  // Zmieniono na właściwość z wielką literą
        public DateTime EndTime { get; set; }  // Zmieniono na właściwość z wielką literą

        // Konstruktor z nazwami zgodnymi z właściwościami
        public MovieTime(Film movie, DateTime startTime)
        {
            Movie = movie;
            StartTime = startTime;
            EndTime = startTime.AddSeconds(movie.LengthSec);  // Ustawianie EndTime
        }

        // Domyślny konstruktor na potrzeby deserializacji
        public MovieTime() { }
    }
}
