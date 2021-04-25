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
            
            if (!ModelState.IsValid) {
                return Page();
            }


            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var movieCollection = db.GetCollection<Models.Movie>("Movies");
                var movie  = movieCollection.FindById(id);
                
                movie.Title = Movie.Title;
                movie.Genre = Movie.Genre;
                movie.Price = Movie.Price;
                movie.Description = Movie.Description;
                movie.Image = Movie.Image;

                movieCollection.Update(movie);
                
                string root = @"wwwroot/images/movieimages";
                string fileName = Movie.Image;
                string pathString = Path.Combine(root, fileName);

                //If directory does not exist, create it. 
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