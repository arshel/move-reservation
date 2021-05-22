using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Auth;
using movie_reservation.Models;
using movie_reservation.Validation;


namespace movie_reservation.Areas.Viewings.Pages {
    public class ReserveModel : PageModel {
        private readonly ILogger<ReserveModel> _logger;

        private readonly AuthChecker _auth;

        public ReserveModel(ILogger<ReserveModel> logger, AuthChecker auth)
        {
            _auth = auth;
            _logger = logger;
        }

        public Viewing Viewing { get; set; }
        public Reservation Reservation {get; set;}

        public void OnGet(int? id) {
            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.Viewing>("viewings");
                Viewing = col.FindOne(x => x.Id == id);
            }
        }

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (!_auth) {
                return Unauthorized();
            }

            var viewingId = Int32.Parse(Request.Form["viewingId"].ToString());
            var seats = Request.Form["seats"].ToArray();

            List<string> takenSeats;
            Viewing viewing;

            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.Viewing>("viewings");
                viewing = col.FindOne(x => x.Id == viewingId);
                takenSeats = seats.Concat(viewing.SeatsTaken).ToList();
                viewing.SeatsTaken = takenSeats;

                col.Update(viewing);
            }

            var reservation = new Models.Reservation {
                Name = _auth.User.Email,
                Seats = takenSeats,
                Time = viewing.Time,
                Price = viewing.Movie.Price,
                UserId = _auth.User.Id,
                ReservationTime = DateTime.Now.Date,
            };

            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.Reservation>("reservations");
                col.Insert(reservation);
            }

            return RedirectToPage("Index");
        }
    }
}
