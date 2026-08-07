using DotAuth.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotAuth.Infrastructure.Persistence.Context
{
    public sealed class DotAuthDbContext : DbContext
    {

        public DotAuthDbContext(DbContextOptions<DotAuthDbContext> options) : base(options)
        {
        }
        public DbSet<DotAuthUser> Users => Set<DotAuthUser>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RefreshToken>()
                .HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DotAuthDbContext).Assembly);
        }
    }
}
