using System;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Models;

namespace movie_reservation.Pages
{
 public class DisplayModel : PageModel
{

        private readonly ILogger<DisplayModel> _logger;

        public DisplayModel(ILogger<DisplayModel> logger)
        {
            _logger = logger;
        }

          public IList<Movie> Movie { get; set; }
          public string Success;
          public string Error;

        public void OnGet(){ 
            using(var db = new LiteDatabase(@"movieReservation.db")) {

                  var movieCollection = db.GetCollection<Models.Movie>("Movies");
                  Movie = movieCollection.Query().ToList();
        
                }
        }   
       
      public IActionResult OnPostDelete(int id){

            using(var db = new LiteDatabase(@"movieReservation.db")) {

              var movieCollection = db.GetCollection<Models.Movie>("Movies");
              var image =  movieCollection.FindById(id);

              // Delete the image
              if(System.IO.File.Exists(@"wwwroot/images/movieimages/" + image.Image ))
              {
                  try
                  {
                      System.IO.File.Delete(@"wwwroot/images/movieimages/" + image.Image);
                  }
                  catch (System.IO.IOException e)
                  {
                    Error = e.Message;
                  return RedirectToPage("/Movies/Index");
                  }
              }
              // then delete the movie 
               movieCollection.Delete(id);
      }

        Success = "Film is succesvol verwijderd!";
        return RedirectToPage("/Movies/Index");
      }
  }
}
