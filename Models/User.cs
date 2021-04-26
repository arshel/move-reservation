using LiteDB;

namespace movie_reservation.Models {
    public enum UserRole {
        Customer,
        Clerk,
        Owner,
    }

    public class User {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashPassword { get; set; }
        public string HashSalt { get; set; }
        public UserRole Role { get; set; }
    }
}