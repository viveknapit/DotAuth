using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DotAuth.Application.Common.Security
{
    public static class TokenHashing
    {
        public static string Hash(string token)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }
    }
}
