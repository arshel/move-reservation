using System;
using System.Collections.Generic;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using JWT.Serializers;
using movie_reservation.Models;

namespace movie_reservation.Auth {
    public class Jwt {
        private const string Secret = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";

        private JwtBuilder Builder;

        public Jwt() {
            Builder = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSerializer(new JsonNetSerializer())
                .WithUrlEncoder(new JwtBase64UrlEncoder())
                .WithSecret(Secret);
        }

        public CreatedToken Create(User user) {
            // In 7 days
            var timespan = TimeSpan.FromDays(7);
            var expiresAt = DateTime.Now.Add(timespan).Subtract(DateTime.UnixEpoch).TotalSeconds;

            var payload = new Dictionary<string, object>{
                {"Id", user.Id },
                {"Email", user.Email },
                {"Role", user.Role },
                // Seconds since epoch
                {"exp", expiresAt },
            };

            var token = Builder.AddClaims(payload).Encode();

            return new CreatedToken{
                TimeSpan = timespan,
                Token = token
            };
        }

        public User Verify(string token) {
            try {
                var payload = Builder
                    .MustVerifySignature()
                    .Decode<IDictionary<string, object>>(token);

                return new User{
                    Id = (int)(Int64)payload["Id"],
                    Email = (string)payload["Email"],
                    Role = (UserRole)(Int64)payload["Role"],
                };
            } catch(TokenExpiredException) {
                return null;
            } catch(SignatureVerificationException) {
                return null;
            }
        }
    }

    public class CreatedToken {
        public string Token;
        public TimeSpan TimeSpan;
    }
}