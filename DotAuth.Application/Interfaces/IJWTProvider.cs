using DotAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Application.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(DotAuthUser user);

        string GenerateRefreshToken();
    }
}
