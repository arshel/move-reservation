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
 public class IndexModel : PageModel
{

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public Viewing[] UpcomingViewings;

        public void OnGet() {
          using(var db = new LiteDatabase(@"movieReservation.db")) {
            var col = db.GetCollection<Viewing>("viewings");
            UpcomingViewings = col.Find(Query.All("Time", Query.Ascending), limit: 4).ToArray();
          }
        }
  }
}
