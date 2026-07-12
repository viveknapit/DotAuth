using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Application.Contracts.Responses
{
    public sealed class CurrentUserResponse
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
