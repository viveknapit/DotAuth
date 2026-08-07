using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Application.Contracts.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
    }
}
