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
        public DbSet<Role> Roles { get; set; }

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
                new Role { RoleId = 1, Name = "Maker" },
                new Role { RoleId = 2, Name = "Koper" },
                new Role { RoleId = 3, Name = "Moderator" }
            );

            // Seeding Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Jan", Password = HashPassword("wachtwoord"), RoleId = 1 },
                new User { Id = 2, Name = "Lisa", Password = HashPassword("wachtwoord"), RoleId = 2 },
                new User { Id = 3, Name = "Mark", Password = HashPassword("wachtwoord"), RoleId = 3 }
            );

            // Seeding Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Houtbewerking" },
                new Category { CategoryId = 2, Name = "Metaalbewerking" }
            );

            // Seeding Types
            modelBuilder.Entity<Type>().HasData(
                new Type { TypeId = 1, Name = "Meubel" },
                new Type { TypeId = 2, Name = "Gereedschap" }
            );

            // Seeding Unique Properties
            modelBuilder.Entity<UniqueProperty>().HasData(
                new UniqueProperty { PropertyId = 1, Name = "Handgemaakt" },
                new UniqueProperty { PropertyId = 2, Name = "Duurzaam" }
            );

            // Seeding Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Houten tafel", Description = "Stevige eiken tafel", TypeId = 1, CategoryId = 1, MaterialUsage = "Eikenhout", ProductionTime = "5 dagen", Complexity = "Medium", Sustainability = "Hoog", PropertyId = 1 },
                new Product { Id = 2, Name = "Stalen hamer", Description = "Duurzame hamer", TypeId = 2, CategoryId = 2, MaterialUsage = "Staal", ProductionTime = "2 dagen", Complexity = "Laag", Sustainability = "Middel", PropertyId = 2 }
            );

            // Seeding Sales
            modelBuilder.Entity<Sale>().HasData(
                new Sale { SaleId = 1, UserId = 2, ProductId = 1, Amount = 1, Description = "Gekocht voor woonkamer" },
                new Sale { SaleId = 2, UserId = 2, ProductId = 2, Amount = 2, Description = "Voor werkplaats" }
            );

            // Seeding Reviews
            modelBuilder.Entity<Review>().HasData(
                new Review { ReviewId = 1, UserId = 2, ProductId = 1, Rating = 5, Description = "Geweldig product!" },
                new Review { ReviewId = 2, UserId = 2, ProductId = 2, Rating = 4, Description = "Goede kwaliteit." }
            );
        }
    }
}
