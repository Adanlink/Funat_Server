using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Server.Database.Models;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Server.Database.Context
{
    public class FunatDbContext : DbContext
    {
        public FunatDbContext(DbContextOptions<FunatDbContext> options) : base(options)
        {
        }

        public DbSet<AccountModel> Accounts { get; set; }

        public DbSet<CharacterModel> Characters { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountModel>(e =>
            {
                e.HasKey(a => a.Id);
                e.Property(a => a.EncodedHash).IsRequired();
                e.Property(a => a.Username).IsRequired().HasMaxLength(32);
                e.Property(a => a.Email).IsRequired(false).HasMaxLength(254);
                e.HasMany(a => a.Characters)
                .WithOne(c => c.OwnerAccount);
            });

            modelBuilder.Entity<CharacterModel>(e =>
            {
                e.HasKey(c => c.Id);
                e.HasOne(c => c.OwnerAccount)
                .WithMany(a => a.Characters);
                e.Property(c => c.Authority).IsRequired();
                e.Property(c => c.Nickname).IsRequired().HasMaxLength(24);
                e.Property(c => c.MapId).IsRequired();
                e.Property(c => c.MapX).IsRequired();
                e.Property(c => c.MapY).IsRequired();
            });
        }
    }
}
