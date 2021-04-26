using LiteDB;
using System.IO;  
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using movie_reservation.Models;
using System.Threading.Tasks;


namespace movie_reservation.Pages {


    public class EditModel : PageModel {

        
        private readonly ILogger<EditModel> _logger;

        public EditModel(ILogger<EditModel> logger)
        {
            _logger = logger;
        }


        [BindProperty]
        public Movie Movie { get; set; }        
        
        public IActionResult OnGet(int? id) {

           using(var db = new LiteDatabase(@"movieReservation.db")) {

                var movieCollection = db.GetCollection<Models.Movie>("Movies");
                Movie  = movieCollection.FindById(id);
                 
                }
                return Page();
            }

         public IActionResult OnPost(int id) {
            
            
            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var movieCollection = db.GetCollection<Models.Movie>("Movies");
                var movie  = movieCollection.FindById(id);
                
                movie.Title = Movie.Title;
                movie.Genre = Movie.Genre;
                movie.Price = Movie.Price;
                movie.Time1 = Movie.Time1;
                movie.Time2 = Movie.Time2;
                movie.Seats = Movie.Seats;
                movie.Description = Movie.Description;
               
                movieCollection.Update(movie);
                
        }

           return RedirectToPage("/Movies/Index");
        }
        
    }
}