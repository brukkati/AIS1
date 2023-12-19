using System;
using System.Collections.Generic;

namespace ConsoleApp1.Models.Entities;

public partial class Cinema
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;

    public int Halls { get; set; }

    public int Capacity { get; set; }

    public bool Has3d { get; set; }

    public Cinema ReturnCinema(List<string> cinemaList)
    {
        Cinema cinema = new Cinema();
        cinema.Id = Guid.Parse(cinemaList[0]);
        cinema.Name = cinemaList[1];
        cinema.Address = cinemaList[2];
        cinema.Halls = int.Parse(cinemaList[3]);
        cinema.Capacity = int.Parse(cinemaList[4]);
        cinema.Has3d = bool.Parse(cinemaList[5]);
        return cinema;
    }

    public List<string> GetStringList()
    {
        List<string> cinemaList = new()
        {
        Id.ToString(),
        Name,
        Address,
        Halls.ToString(),
        Capacity.ToString(),
        Has3d.ToString(),
        };
        return cinemaList;
    }
}
