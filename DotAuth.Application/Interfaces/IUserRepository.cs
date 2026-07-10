using DotAuth.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> ExistsByEmailAsync(string email);

        Task<bool> ExistsByPhoneAsync(string phone);

        Task<bool> ExistsByUserNameAsync(string userName);

        Task AddAsync(DotAuthUser user);

        Task SaveChangesAsync();
    }
}
