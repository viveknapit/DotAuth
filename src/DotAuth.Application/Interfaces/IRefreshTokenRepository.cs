using DotAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Application.Interfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken token);

        Task<RefreshToken?> GetByHashAsync(string tokenHash);

        Task SaveChangesAsync();
    }
}
