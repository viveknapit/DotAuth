using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Infrastructure.Options
{
    public sealed class JwtOptions
    {
        public string SecretKey { get; set; } = string.Empty;

        public string Issuer { get; set; } = "DotAuth";

        public string Audience { get; set; } = "DotAuth";

        public int AccessTokenExpirationMinutes { get; set; } = 30;
    }
}
