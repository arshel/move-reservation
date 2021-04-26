using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Auth;
using movie_reservation.Models;
using movie_reservation.Validation;

namespace movie_reservation.Pages {
    public class RegisterModel : PageModel {
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(ILogger<RegisterModel> logger)
        {
            _logger = logger;
        }

        public void onGet() {

        }

        [BindProperty]
        public RegisterRequest RegisterRequest { get; set; }

        public string Success;

        public IActionResult OnPost() {
            if (!ModelState.IsValid) {
                return Page();
            }

            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.User>("users");

                // Hash the password
                var salt = Salt.Create();
                var hash = Hash.Create(RegisterRequest.Password, salt);

                // Give the first user in the database the Owner role
                var userCount = col.Count();
                var role = userCount > 0 ? UserRole.Customer : UserRole.Owner;

                // Insert a user with our values
                col.Insert(new Models.User{
                    Email = RegisterRequest.Email,
                    HashSalt = salt,
                    HashPassword = hash,
                    Role = role,
                });
            }

            Success = "Je bent geregistreerd!";
            return Page();
        }
    }

    public class RegisterRequest {
        [Required(ErrorMessage = "Email is verplicht.")]
        [EmailAddress(ErrorMessage = "Email moet een echt email adress zijn.")]
        [EmailDoesNotExist]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wachtwoord is verplicht.")]
        [MinLength(6, ErrorMessage = "Wachtwoord moet minimaal 6 karakters lang zijn.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Herhaal wachtwoord is verplicht.")]
        [Compare("Password", ErrorMessage = "Wachtwoorden zijn niet gelijk.")]
        public string PasswordCheck { get; set; }
    }
}
