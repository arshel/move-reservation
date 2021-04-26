using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Auth;
using movie_reservation.Models;

namespace movie_reservation.Areas.Users.Pages {
    public class IndexModel : PageModel {
        private readonly ILogger<IndexModel> _logger;

        private readonly AuthChecker _auth;

        public IndexModel(ILogger<IndexModel> logger, AuthChecker auth) {
            _logger = logger;
            _auth = auth;
        }

        public List<Models.User> Users;

        public IActionResult OnGet() {
            // Only Clerk and Owner can look at the users
            if (_auth.User.Role != UserRole.Owner && _auth.User.Role != UserRole.Clerk) {
                return Unauthorized();
            }

            // Populate Users variable for showing in view
            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.User>("users");
                Users = col.FindAll().ToList();
                return Page();
            }

        }
    }
}
