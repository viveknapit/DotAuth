using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Application.Contracts.Requests
{
    public class RegisterRequest
    {
        public string? UserName { get; init; }

        public string? Email { get; init; }

        public string? PhoneNumber { get; init; }

        public string Password { get; init; } = string.Empty;

        public string? FirstName { get; init; }

        public string? LastName { get; init; }
    }
}
