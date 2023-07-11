using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Memory> Memories { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserMemory> UserMemories { get; set; }
        public DbSet<UserLike> UserLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                   .HasMany(m => m.Memories)
                   .WithMany(u => u.Users)
                   .UsingEntity<UserMemory>();

            builder.Entity<Memory>()
                   .HasMany(m => m.Users)
                   .WithMany(u => u.Memories)
                   .UsingEntity<UserMemory>();

            builder.Entity<User>()
                    .HasMany(e => e.LikedMemories)
                    .WithMany(e => e.LikedByUsers)
                    .UsingEntity<UserLike>();

            builder.Entity<Memory>()
            .HasMany(e => e.LikedByUsers)
            .WithMany(e => e.LikedMemories)
            .UsingEntity<UserLike>();

            builder.Entity<Memory>()
             .HasMany(x => x.Messages)
             .WithOne(x => x.Memory);
        }
    }
}