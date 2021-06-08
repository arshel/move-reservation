using System;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Auth;
using movie_reservation.Models;

namespace movie_reservation.Pages
{
 public class MyReservationModel : PageModel
{

        private readonly ILogger<MyReservationModel> _logger;

        private readonly AuthChecker _auth;

        public MyReservationModel(ILogger<MyReservationModel> logger, AuthChecker auth)
        {
            _auth = auth;
            _logger = logger;
        }

        
        public Reservation Reservation {get; set;}

        public IActionResult OnGet( int Id) {
           
        using(var db = new LiteDatabase(@"movieReservation.db")) {
            var col = db.GetCollection<Models.Reservation>("reservations");
            Reservation = col.FindOne(x => x.UserId == Id);
            return Page();
        }
    }
  }
}
