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
using movie_reservation.Models;


 namespace movie_reservation.Areas.Viewings.Pages
{
    public class ViewingEditModel : PageModel
    {

            private readonly ILogger<ViewingEditModel> _logger;
            
            private readonly AuthChecker _auth;
           
            public ViewingEditModel(ILogger<ViewingEditModel> logger,  AuthChecker auth)
            {
                _auth = auth;
                _logger = logger;
            }
            
            [BindProperty]
            public Viewing Viewing  { get; set; }
            
            [BindProperty]
            public Movie Movie  { get; set; }
            
            [BindProperty]
            public testRequest testingRequest { get; set; }

            public IActionResult OnGet(int id) {

                using(var db = new LiteDatabase(@"movieReservation.db")) {

                    var viewCollection = db.GetCollection<Models.Viewing>("viewings");
                    Viewing  = viewCollection.FindById(id);
                }
                    return Page();
            }

            public IActionResult OnPost(int? id) {
            
        
                if (!_auth) {
                    return Unauthorized();
                }

                using(var db = new LiteDatabase(@"movieReservation.db")) {
                    
                    var movieCol = db.GetCollection<Models.Movie>("movies");
                   
                    var viewingCol = db.GetCollection<Models.Viewing>("viewings");
                   
                    var viewing = viewingCol.FindById(id);

                    Movie = movieCol.FindById(testingRequest.MovieId);
                   
                    if (Movie == null) {
                        return NotFound();
                    }

                    viewing.MovieId = Movie.Id;
                    viewing.Venue = testingRequest.Venue  == "small" ? Models.Venue.Small : Models.Venue.Large;
                    viewing.SeatsTaken = new List<string>();
                    viewing.Time = Viewing.Time;
                    
                    viewingCol.Update(viewing);

                }

                return RedirectToPage("/Index");

            }

        public class testRequest {
            [Required]
            public int Id { get; set; }

            [Required]
            public int MovieId { get; set; }

          //  [Required]
            [VenueValid]
            public string Venue { get; set; }

           // [Required]
            public string Date { get; set; }

           // [Required]
            public string Time { get; set; }
        }
    }
}
