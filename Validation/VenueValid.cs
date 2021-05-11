using System.ComponentModel.DataAnnotations;

namespace movie_reservation.Validation {
    public class VenueValid : ValidationAttribute {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            string entered = value.ToString();
            if (entered != "large" && entered != "small") {
                return new ValidationResult("Venue must be large or small.");
            }
            return ValidationResult.Success;
        }
    }
}
