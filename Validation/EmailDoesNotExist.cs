using System.ComponentModel.DataAnnotations;
using LiteDB;

namespace movie_reservation.Validation {
    // Class that validates the email entered is not already used
    public class EmailDoesNotExist : ValidationAttribute {
        // Try to find a user with the entered email, if it is not null then we know the email exists
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            string entered = value.ToString();
            using(var db = new LiteDatabase(@"movieReservation.db")) {
                var col = db.GetCollection<Models.User>("users");
                var maybeUser = col.FindOne(x => x.Email == entered);
                if (maybeUser != null) {
                    return new ValidationResult("Email bestaat al.");
                }
                return ValidationResult.Success;
            }
        }
    }
}
