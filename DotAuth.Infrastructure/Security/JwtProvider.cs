using DotAuth.Application.Interfaces;
using DotAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Infrastructure.Security
{
    public class JwtProvider : IJwtProvider
    {
        public string GenerateAccessToken(DotAuthUser user)
        {
            return Guid.NewGuid().ToString();
        }

        public string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
