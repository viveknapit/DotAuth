using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Domain.Entities
{
    public class DotAuthUser
    {
        public Guid Id { get; private set; }
        public string username { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? ProfilePictureUrl { get; private set; }
        public string PasswordHash { get; private set; }
        public bool isEmailVerified { get; private set; }
        public bool isPhoneNumberVerified { get; private set; }
        public bool isActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime LastLoginAt { get; private set; }

    }
}
