using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Application.Contracts.Responses
{
    public class LoginResponse
    {
        public Guid UserId { get; init; }

        public string AccessToken { get; init; } = string.Empty;

        public string RefreshToken { get; init; } = string.Empty;
    }
}
