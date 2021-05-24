using System;
using System.IO;
using LiteDB;
using Microsoft.AspNetCore.Http;
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
        public IFormFile FileUpload { get; set; }

        [BindProperty]
        public Movie Movie { get; set; }

        public IActionResult OnPost() {
            var filePath = Path.Combine("./wwwroot/images/", FileUpload.FileName);
            using(var stream = System.IO.File.Create(filePath)) {
                FileUpload.CopyTo(stream);
            }

            using(var db = new LiteDatabase(@"movieReservation.db")) {
                    var movieCollection = db.GetCollection<Models.Movie>("Movies");

                        movieCollection.Insert(new Models.Movie{
                        Title = Movie.Title,
                        Genre = Movie.Genre,
                        Price = Movie.Price,
                        Description = Movie.Description,
                        Image = FileUpload.FileName,
                    });
            }
            return RedirectToPage("/Movies/Index");
        }
    }
}