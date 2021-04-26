using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace movie_reservation.Pages {
    public class LogoutModel : PageModel {
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(ILogger<LogoutModel> logger) {
            _logger = logger;
        }

        public IActionResult OnGet() {
            Response.Cookies.Delete("JWTAuthToken");
            return RedirectToPage("Index");
        }
    }
}
