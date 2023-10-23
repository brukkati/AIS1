using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Cinema
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Halls { get; set; }
        public int Capacity { get; set; }
        public bool Has3D { get; set; }

        public Cinema(string name, string address, int halls, int capacity, bool has3D)
        {
            Name = name;
            Address = address;
            Halls = halls;
            Capacity = capacity;
            Has3D = has3D;
        }

        public List<string> GetStringList()
        {
            List<string> cinemaList = new()
            {
                Name,
                Address,
                Halls.ToString(),
                Capacity.ToString(),
                Has3D.ToString()
            };
            return cinemaList;
        }
    }

}
