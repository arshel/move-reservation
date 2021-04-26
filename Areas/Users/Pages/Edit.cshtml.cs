using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using movie_reservation.Auth;
using movie_reservation.Models;
using movie_reservation.Validation;

namespace movie_reservation.Areas.Users.Pages {
    public class EditModel : PageModel {
        private readonly ILogger<EditModel> _logger;

        private readonly AuthChecker _auth;

        public EditModel(ILogger<EditModel> logger, AuthChecker auth) {
            _logger = logger;
            _auth = auth;
        }

        public string Error;
        public string Success;

        public Models.User UserToEdit;
        public IActionResult OnGet(int id) {
            if (!_auth) {
                return Unauthorized();
            }

            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.User>("users");
                UserToEdit = col.FindOne(x => x.Id == id);
                if (UserToEdit == null) {
                    return NotFound();
                }
                return Page();
            }
        }

        public EditRequest EditRequest { get; set; }
        public IActionResult OnPostEdit([Bind("Id, Email, Role")] EditRequest editRequest) {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (!_auth) {
                return Unauthorized();
            }

            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.User>("users");
                UserToEdit = col.FindOne(x => x.Id == editRequest.Id);

                if (UserToEdit == null) {
                    Error = "Geen gebruiker gevonden, probeer opnieuw.";
                    return Page();
                }

                // If the email is updated
                if (UserToEdit.Email != editRequest.Email) {
                    // Check for email availability
                    var maybeUser = col.FindOne(x => x.Email == editRequest.Email);
                    if (maybeUser != null) {
                        Error = "Email al in gebruik.";
                        return Page();
                    }
                }

                // If the role is updated
                if (UserToEdit.Role != editRequest.Role) {
                    // Check if the user should be able to update this user
                    if (!_auth.CanEdit(UserToEdit)) {
                        Error = "U kunt deze gebruiker niet bewerken.";
                        return Page();
                    }

                    if (_auth.User.Role != UserRole.Owner) {
                        Error = "Alleen de owner kan rollen bewerken.";
                        return Page();
                    }

                    if (UserToEdit.Role == UserRole.Owner) {
                        Error = "Owners mogen hun eigen rol niet veranderen.";
                        return Page();
                    }
                }

                UserToEdit.Email = editRequest.Email;
                UserToEdit.Role = editRequest.Role;
                var updated = col.Update(UserToEdit);

                if (!updated) {
                    Error = "Kon gebruiker niet bewerken.";
                    return Page();
                }

                Success = "Gebruiker bewerkt.";
                return Page();
            }
        }

        public ChangePasswordRequest ChangePasswordRequest { get; set; }
        public IActionResult OnPostChangePassword([Bind("Id, Password, PasswordCheck")] ChangePasswordRequest changePasswordRequest) {
            if (!ModelState.IsValid) {
                return Page();
            }

            if (!_auth) {
                return Unauthorized();
            }

            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<User>("users");

                var maybeUser = col.FindOne(x => x.Id == changePasswordRequest.Id);
                if (maybeUser == null) {
                    Error = "Gebruiker niet gevonden.";
                    return Page();
                }

                if (!_auth.CanEdit(maybeUser)) {
                    Error = "U kunt deze gebruiker niet bewerken.";
                    return Page();
                }

                // Hash the password
                var salt = Salt.Create();
                var hash = Hash.Create(changePasswordRequest.Password, salt);

                // Update the user
                maybeUser.HashPassword = hash;
                maybeUser.HashSalt = salt;
                var updated = col.Update(maybeUser);

                if (!updated) {
                    Error = "Kon wachtwoord niet veranderen.";
                    return Page();
                }
            }

            Success = "Wachtwoord veranderd.";
            return Page();
        }
    }

    public class EditRequest {
        [Required]
        public int Id { get; set; }

        [EmailAddress(ErrorMessage = "Email is niet goed ingevuld.")]
        [Required(ErrorMessage = "Email is vereist.")]
        public string Email { get; set; }

        public UserRole Role { get; set; }
    }

    public class ChangePasswordRequest {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Wachtwoord is vereist.")]
        [MinLength(6, ErrorMessage = "Wachtwoord moet minimaal 6 karakters lang zijn.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Herhaald wachtwoord is vereist.")]
        [MinLength(6, ErrorMessage = "Herhaald wachtwoord moet minimaal 6 karakters lang zijn.")]
        [Compare("Password", ErrorMessage = "Herhaald wachtwoord moet overeenkomen met Wachtwoord.")]
        public string PasswordCheck { get; set; }
    }
}
