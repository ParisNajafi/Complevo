using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Salt = "K49uoQolP4F+arcr8DW/Pg==",
                    Password = "9Tg/YYjaW7qAcFilhYaVgyBwbd8w+lRNnJva/4/0EDg=",
                    Username = "Admin"
                }
            );
        }
    }

}

