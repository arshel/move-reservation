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
       
        
        public IActionResult OnPost(IFormFile file) {
            
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
                    Description = Movie.Description
                });

                string root = @"wwwroot/images/movieimages";
                string fileName = Movie.Image;
                string pathString = Path.Combine(root, fileName);
            

                // If directory does not exist, create it. 
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                // If image does not exist, create it. 
                if (!System.IO.File.Exists(pathString)){
                    System.IO.File.Create(pathString);
                }
        }
           return RedirectToPage("/Movies/Index");
        }
    }
}