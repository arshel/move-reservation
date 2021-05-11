using System;
using System.Collections.Generic;

namespace movie_reservation.Models
{
    public enum Venue {
        Small,
        Large,
    }

    public class Viewing
    {
        public int Id { get; set; }

        public int MovieId { get; set; }

        public Venue Venue { get; set; }

        public List<string> SeatsTaken { get; set; }

        public DateTime Time { get; set; }
    }
}