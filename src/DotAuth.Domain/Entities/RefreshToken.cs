using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; private set; }

        public Guid UserId { get; private set; }

        public DotAuthUser User { get; private set; } = null!;

        public string TokenHash { get; private set; } = string.Empty;

        public DateTime CreatedAt { get; private set; }

        public DateTime ExpiresAt { get; private set; }

        public DateTime? RevokedAt { get; private set; }

        private RefreshToken()
        {
        }

        public static RefreshToken Create(Guid userId, string tokenHash, int expiresInDays = 7)
        {
            return new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                TokenHash = tokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(expiresInDays)
            };
        }
        public void Revoke()
        {
            RevokedAt = DateTime.UtcNow;
        }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt.HasValue;
    }
}
