using System.IO;
using LiteDB;
using Microsoft.AspNetCore.Http;
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
        public IFormFile FileUpload { get; set; }

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
            if (FileUpload != null) {
                var filePath = Path.Combine("./wwwroot/images/", FileUpload.FileName);
                using(var stream = System.IO.File.Create(filePath)) {
                    FileUpload.CopyTo(stream);
                }
            }

            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var movieCollection = db.GetCollection<Models.Movie>("Movies");
                var movie  = movieCollection.FindById(id);

                movie.Title = Movie.Title;
                movie.Genre = Movie.Genre;
                movie.Price = Movie.Price;
                movie.Description = Movie.Description;

                if (FileUpload != null) {
                    movie.Image = FileUpload.FileName;
                }

                movieCollection.Update(movie);

        }

           return RedirectToPage("/Movies/Index");
        }

    }
}