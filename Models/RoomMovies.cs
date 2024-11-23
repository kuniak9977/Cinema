using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Models
{
    public class RoomMovies
    {
        Room room;

        List<MovieTime> movieDuration;

        public RoomMovies(Room _room, MovieTime _movieDuration)
        {
            room = _room;
            movieDuration = [_movieDuration];
        }
    }
}
