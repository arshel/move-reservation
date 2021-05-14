using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Auth;
using movie_reservation.Validation;
using movie_reservation.Models
;
namespace movie_reservation.Areas.Viewings.Pages {

    public class CreateModel : PageModel {

        private readonly ILogger<CreateModel> _logger;

        private readonly AuthChecker _auth;

        public CreateModel(ILogger<CreateModel> logger, AuthChecker auth)
        {
            _auth = auth;
            _logger = logger;
        }

        public void onGet() {}

        [BindProperty]
        public Request CreateRequest { get; set; }

        public Movie Movie { get; set; }

        public IActionResult OnPost() {
             if (!ModelState.IsValid) {
                return Page();
            }

            if (!_auth) {
                return Unauthorized();
            }

            var date = DateTime.ParseExact(CreateRequest.Date + " " + CreateRequest.Time, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var movieCol = db.GetCollection<Models.Movie>("movies");
                var viewingCol = db.GetCollection<Models.Viewing>("viewings");

                // check movie exists with given id
                Movie  = movieCol.FindById(CreateRequest.MovieId);
                if (Movie == null) {
                    return NotFound();
                }

                // insert viewing
                viewingCol.Insert(new Models.Viewing{
                    MovieId = Movie.Id,
                    Venue = CreateRequest.Venue == "small" ? Models.Venue.Small : Models.Venue.Large,
                    SeatsTaken = new List<string>(),
                    Time = date,
                });
            }

            return RedirectToPage("/Index");
        }
    }

    public class Request {
        [Required]
        public int MovieId { get; set; }

        [Required]
        [VenueValid]
        public string Venue { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }
    }
}
