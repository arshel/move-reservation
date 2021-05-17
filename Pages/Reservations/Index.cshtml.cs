using System;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Auth;

namespace movie_reservation.Pages
{
 public class ReservationIndexModel : PageModel
{

        private readonly ILogger<ReservationIndexModel> _logger;

        private readonly AuthChecker _auth;

        public ReservationIndexModel(ILogger<ReservationIndexModel> logger, AuthChecker auth)
        {
            _auth = auth;
            _logger = logger;
        }

         public List<Models.Reservation>  Reservation {get; set;}


        public void OnGet() {
            
        using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.Reservation>("reservations");
                Reservation = col.Query().ToList();        
            }
        }



        public IActionResult OnPostDelete(int id){

            using(var db = new LiteDatabase(@"movieReservation.db")) {

              var col = db.GetCollection<Models.Reservation>("reservations");
              // then delete the movie 
               col.Delete(id);
      }
        return RedirectToPage("Index");
      }
  }
}
