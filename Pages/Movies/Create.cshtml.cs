using LiteDB;
using System.IO;   
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using movie_reservation.Models;
using System;



namespace movie_reservation.Pages {
    public class CreateModel : PageModel {
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ILogger<CreateModel> logger)
        {
            _logger = logger;
        }

        public void onGet() {

        }

        [BindProperty]
        public Movie Movie { get; set; }
       
        
        public IActionResult OnPost() {
            
        using(var db = new LiteDatabase(@"movieReservation.db")) {
                var movieCollection = db.GetCollection<Models.Movie>("Movies");
    
                    movieCollection.Insert(new Models.Movie{
                    Title = Movie.Title,
                    Genre = Movie.Genre,
                    Price = Movie.Price,
                    Time1 = Movie.Time1,
                    Time2 = Movie.Time2,
                    Seats = Movie.Seats,
                    Description = Movie.Description
                });
        }
           return RedirectToPage("/Movies/Index");
        }
    }
}