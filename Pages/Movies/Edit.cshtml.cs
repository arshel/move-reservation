using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Models;

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
                movie.Description = Movie.Description;

                movieCollection.Update(movie);

        }

           return RedirectToPage("/Movies/Index");
        }

    }
}