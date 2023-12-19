using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerLinkingPart
{
    public class CinemaCon
    {
        public Guid CinemaId { get; set; }

        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public int Halls { get; set; }

        public int Capacity { get; set; }

        public bool Has3d { get; set; }

        public CinemaCon ReturnCinema(List<string> cinemaList)
        {
            CinemaCon cinema = new CinemaCon();
            string cinema_id = (cinemaList[0]);
            cinema.CinemaId = Guid.Parse(cinema_id);
            cinema.Name = cinemaList[1];
            cinema.Address = cinemaList[2];
            cinema.Halls = int.Parse(cinemaList[3]);
            cinema.Capacity = int.Parse(cinemaList[4]);
            cinema.Has3d = bool.Parse(cinemaList[5]);
            return cinema;
        }
    }
}
