using System;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using movie_reservation.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Models;

namespace movie_reservation.Pages
{
 public class DisplayModel : PageModel
{

        private readonly ILogger<DisplayModel> _logger;

        public DisplayModel(ILogger<DisplayModel> logger, AuthChecker auth)
        {
            _logger = logger;
              _auth = auth;
        }

          public IList<Movie> Movie { get; set; }
          private readonly AuthChecker _auth;
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
              var movie  =  movieCollection.FindById(id);

              var filePath = Path.Combine("./wwwroot/images/", movie.Image);
                
              // delete the image if it exisits
              if(filePath != null){
                System.IO.File.Delete(filePath);
              } 
              // then delete the movie 
              movieCollection.Delete(id);
      }

        Success = "Film is succesvol verwijderd!";
        return RedirectToPage("/Movies/Index");
      }
  }
}
