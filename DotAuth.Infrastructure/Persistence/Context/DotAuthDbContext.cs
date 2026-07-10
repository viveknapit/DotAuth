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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DotAuthDbContext).Assembly);
        }
    }
}
