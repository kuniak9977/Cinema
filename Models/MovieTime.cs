using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class MovieTime
    {
        public Film movie { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }

        public MovieTime(Film _film, DateTime _start) 
        {
            movie = _film;
            startTime = _start;
            endTime = _start.AddSeconds(movie.LengthSec);
        }
    }
}
