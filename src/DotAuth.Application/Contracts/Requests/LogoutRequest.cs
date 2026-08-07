using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Application.Contracts.Requests
{
    public sealed class LogoutRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
