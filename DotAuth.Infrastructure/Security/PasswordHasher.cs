using DotAuth.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Infrastructure.Security
{
    public sealed class PasswordHasher : IPasswordHasher
    {
        string IPasswordHasher.Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        bool IPasswordHasher.Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
