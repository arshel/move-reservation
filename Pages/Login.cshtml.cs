using System.ComponentModel.DataAnnotations;
using System.Linq;
using LiteDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Auth;

namespace movie_reservation.Pages {
    public class LoginModel : PageModel {
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(ILogger<LoginModel> logger) {
            _logger = logger;
        }

        public void OnGet() {}

        public string Error;
        public string Success;

        [BindProperty]
        public LoginRequest LoginRequest { get; set; }
        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }

            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.User>("users");

                // Get the user with the email
                var maybeUser = col.FindOne(x => x.Email == LoginRequest.Email);
                if (maybeUser == null) {
                    // User does not exist
                    Error = "Deze combinatie email en wachtwoord is onjuist.";
                    return Page();
                }

                // Validate password is correct
                var passwordCorrect = Hash.Validate(LoginRequest.Password, maybeUser.HashSalt, maybeUser.HashPassword);
                if (!passwordCorrect) {
                    Error = "Deze combinatie email en wachtwoord is onjuist.";
                    return Page();
                }

                // Create a JWT token and set JWTAuthToken Cookie
                var jwt = new Jwt();
                var token = jwt.Create(maybeUser);
                var cookieOpts = new CookieOptions{
                    MaxAge = token.TimeSpan,
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                };
                Response.Cookies.Append("JWTAuthToken", token.Token, cookieOpts);

                Success = "Je bent nu ingelogd!";
                return Page();
            }
        }
    }

    public class LoginRequest {
        [Required(ErrorMessage = "Email is verplicht.")]
        [EmailAddress(ErrorMessage = "Email is niet geldig.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [MinLength(6, ErrorMessage = "Wachtwoord moet minimaal 6 karakters lang zijn.")]
        public string Password { get; set; }
    }
}
