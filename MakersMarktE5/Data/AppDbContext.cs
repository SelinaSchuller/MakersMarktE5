using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MakersMarktE5.Data
{
	class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(
				ConfigurationManager.ConnectionStrings["MakersMarkt"].ConnectionString,
				ServerVersion.Parse("8.0.30")
				);
		}

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding Roles
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, Name = "Creator" },
                new Role { RoleId = 2, Name = "Buyer" },
                new Role { RoleId = 3, Name = "Moderator" }
            );

            // Seeding Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "John", Password = HashPassword("wachtwoord"), RoleId = 1 },
                new User { Id = 2, Name = "Lisa", Password = HashPassword("wachtwoord"), RoleId = 2 },
                new User { Id = 3, Name = "Mark", Password = HashPassword("wachtwoord"), RoleId = 3 }
            );

            // Seeding Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Woodworking" },
                new Category { CategoryId = 2, Name = "Metalworking" }
            );

            // Seeding Types
            modelBuilder.Entity<Type>().HasData(
                new Type { TypeId = 1, Name = "Furniture" },
                new Type { TypeId = 2, Name = "Tool" }
            );

            // Seeding Unique Properties
            modelBuilder.Entity<UniqueProperty>().HasData(
                new UniqueProperty { PropertyId = 1, Name = "Handmade" },
                new UniqueProperty { PropertyId = 2, Name = "Sustainable" }
            );

            // Seeding Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Wooden Table", Description = "Sturdy oak table", TypeId = 1, CategoryId = 1, MaterialUsage = "Oak wood", ProductionTime = "5 days", Complexity = "Medium", Sustainability = "High", PropertyId = 1 },
                new Product { Id = 2, Name = "Steel Hammer", Description = "Durable hammer", TypeId = 2, CategoryId = 2, MaterialUsage = "Steel", ProductionTime = "2 days", Complexity = "Low", Sustainability = "Medium", PropertyId = 2 }
            );

            // Seeding Sales
            modelBuilder.Entity<Sale>().HasData(
                new Sale { SaleId = 1, UserId = 2, ProductId = 1, Amount = 1, Description = "Bought for living room" },
                new Sale { SaleId = 2, UserId = 2, ProductId = 2, Amount = 2, Description = "For workshop" }
            );

            // Seeding Reviews
            modelBuilder.Entity<Review>().HasData(
                new Review { ReviewId = 1, UserId = 2, ProductId = 1, Rating = 5, Description = "Great product!" },
                new Review { ReviewId = 2, UserId = 2, ProductId = 2, Rating = 4, Description = "Good quality." }
            );
        }
    }
}
