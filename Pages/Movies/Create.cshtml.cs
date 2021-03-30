using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using movie_reservation.Models;


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
            
            if (!ModelState.IsValid) {
                return Page();
            }


            using(var db = new LiteDatabase(@"movieReservation.db")) {
            var movieCollection = db.GetCollection<Models.Movie>("Movies");
    
                    movieCollection.Insert(new Models.Movie{
                    Title = Movie.Title,
                    Genre = Movie.Genre,
                    Price = Movie.Price,
                    Image = Movie.Image,
                    // Description = Movie.Description
                });
            }

           return RedirectToPage("/Movies/Index");
        }
    }
}