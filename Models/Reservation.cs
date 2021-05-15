using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.IO;
using LiteDB;

namespace movie_reservation.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public List<string> Seats { get; set; }

        [Required]
        public string Name { get; set; }

    }
}