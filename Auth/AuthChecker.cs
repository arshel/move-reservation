using System;
using Microsoft.AspNetCore.Http;
using movie_reservation.Models;

namespace movie_reservation.Auth {
    public class AuthChecker {
        public User User { get; }

        public AuthChecker(IHttpContextAccessor accessor) {
            // Get token out of cookies
            var token = accessor.HttpContext.Request.Cookies["JWTAuthToken"];
            if (token == null) {
                return;
            }

            // Try to verify the token and set it as the user
            var jwt = new Jwt();
            User = jwt.Verify(token);
        }

        public bool isLoggedIn() {
            return User != null;
        }

        public bool CanEdit(User user) {
            // Owner can edit everything
            if (User.Role == UserRole.Owner) {
                return true;
            }

            // A user can edit themself
            if (User.Id == user.Id) {
                return true;
            }

            // A clerk can edit a customer
            if (User.Role == UserRole.Clerk && user.Role == UserRole.Customer) {
                return true;
            }

            return false;
        }

        // Allows for @if (authChecker)
        public static implicit operator bool(AuthChecker checker) => checker.isLoggedIn();
    }
}