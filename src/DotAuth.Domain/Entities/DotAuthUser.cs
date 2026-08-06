using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Domain.Entities
{
    public class DotAuthUser
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? ProfilePictureUrl { get; private set; }
        public string PasswordHash { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public bool IsPhoneNumberVerified { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime LastLoginAt { get; private set; }


        public static DotAuthUser Create(
        string? userName,
        string? email,
        string? phoneNumber,
        string passwordHash,
        string? firstName,
        string? lastName)
        {
            if (string.IsNullOrWhiteSpace(email) &&
                string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new Exception("Email or Phone Number is required.");
            }

            return new DotAuthUser
            {
                Id = Guid.NewGuid(),
                Username = userName,
                Email = email,
                PhoneNumber = phoneNumber,
                PasswordHash = passwordHash,
                FirstName = firstName,
                LastName = lastName,
                IsEmailVerified = false,
                IsPhoneNumberVerified = false,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

        }
    }
}
