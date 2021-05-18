using System;
using System.Collections.Generic;
using System.IO;
using LiteDB;

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

        private Movie cachedMovie { get; set; }

        public Movie Movie
        {
            get {
                if(this.cachedMovie == null) {
                    try {
                        using(var db = new LiteDatabase(@"movieReservation.db")) {
                            var col = db.GetCollection<Movie>("movies");
                            this.cachedMovie = col.FindOne(x => x.Id == MovieId);
                        }
                    } catch(IOException) {
                        Console.WriteLine("cant get movie");
                    }
                }
                return this.cachedMovie;
            }
        }
    }
}