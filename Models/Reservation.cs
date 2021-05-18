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

        public int UserId {get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime Time { get; set; }

        [Required]
        public DateTime ReservationTime { get; set; }

        [Required]
        public decimal Price { get; set; }

    }
}