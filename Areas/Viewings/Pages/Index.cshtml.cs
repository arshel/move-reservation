using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Auth;

namespace movie_reservation.Areas.Viewings.Pages {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;

        private readonly AuthChecker _auth;

        public IndexModel(ILogger<IndexModel> logger, AuthChecker auth)
        {
            _auth = auth;
            _logger = logger;
        }

        public List<Models.Viewing> Viewings { get; set; }

        public void OnGet() {
            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.Viewing>("viewings");
                Viewings = col.Query().ToList();
            }
        }

         public IActionResult OnPostDelete(int id){

            using(var db = new LiteDatabase(@"movieReservation.db")) {

              var col = db.GetCollection<Models.Viewing>("viewings");
              // then delete the movie 
               col.Delete(id);
      }

       
        return RedirectToPage("/Index");
      }
    }
}
