using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class MovieTime
    {
        Film movie { get; set; }
        DateTime startTime { get; set; }
        DateTime endTime { get; set; }

        public MovieTime(Film _film, DateTime _start) 
        {
            movie = _film;
            startTime = _start;
        }
    }
}
