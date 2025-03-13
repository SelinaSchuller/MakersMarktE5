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

		public DbSet<Product> Products { get; set; }

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
			modelBuilder.Entity<ProductCategory>()
		        .HasKey(pc => new { pc.ProductId, pc.CategoryId });

			modelBuilder.Entity<ProductCategory>()
				.HasOne(pc => pc.Product)
				.WithMany(p => p.ProductCategories)
				.HasForeignKey(pc => pc.ProductId);

			modelBuilder.Entity<ProductCategory>()
				.HasOne(pc => pc.Category)
				.WithMany(c => c.ProductCategories)
				.HasForeignKey(pc => pc.CategoryId);

			modelBuilder.Entity<Material>()
				.HasMany(m => m.Products)
				.WithMany(p => p.Materials)
				.UsingEntity<MaterialProduct>(
				mp => mp
				.HasOne(mp => mp.Product)
				.WithMany(p => p.MaterialProducts)
				.HasForeignKey(mp => mp.ProductId),
				mp => mp
				.HasOne(mp => mp.Material)
				.WithMany(m => m.MaterialProducts)
				.HasForeignKey(mp => mp.MaterialId),
				mp =>
				{
					mp.HasKey(mp => new { mp.ProductId, mp.MaterialId });
				}
				);

			// Seeding Roles
			modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Creator" },
                new Role { Id = 2, Name = "Buyer" },
                new Role { Id = 3, Name = "Moderator" }
            );

            // Seeding Users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "John", Password = HashPassword("wachtwoord"), RoleId = 1 },
                new User { Id = 2, Name = "Lisa", Password = HashPassword("wachtwoord"), RoleId = 2 },
                new User { Id = 3, Name = "Mark", Password = HashPassword("wachtwoord"), RoleId = 3 }
            );

			// Seed Categories
			modelBuilder.Entity<Category>().HasData(
				new Category { Id = 1, Name = "Woodworking" },
				new Category { Id = 2, Name = "Metalworking" }
			);

			// Seed Material
			modelBuilder.Entity<Material>().HasData(
				new Material { Id = 1, Name = "Oak wood" },
				new Material { Id = 2, Name = "Steel" }
			);

			// Seeding Types
			modelBuilder.Entity<Type>().HasData(
                new Type { Id = 1, Name = "Furniture" },
                new Type { Id = 2, Name = "Tool" }
            );

            // Seeding Unique Properties
            modelBuilder.Entity<UniqueProperty>().HasData(
                new UniqueProperty { Id = 1, Name = "Handmade" },
                new UniqueProperty { Id = 2, Name = "Sustainable" }
            );

            // Seeding Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Wooden Table", Description = "Sturdy oak table", TypeId = 1, ProductionTime = "5 days", Complexity = "Medium", Sustainability = "High", PropertyId = 1, CreatorId = 2},
                new Product { Id = 2, Name = "Steel Hammer", Description = "Durable hammer", TypeId = 2, ProductionTime = "2 days", Complexity = "Low", Sustainability = "Medium", PropertyId = 2, CreatorId = 2}
            );

			// Seed ProductCategory Relationships (Assigning Products to Categories)
			modelBuilder.Entity<ProductCategory>().HasData(
			   new ProductCategory { ProductId = 1, CategoryId = 1 },
			   new ProductCategory { ProductId = 2, CategoryId = 1 },
			   new ProductCategory { ProductId = 2, CategoryId = 2 }
		   );

			// ✅ Seeding MaterialProduct(Assigning Materials to Products)
			modelBuilder.Entity<MaterialProduct>().HasData(
				new MaterialProduct { ProductId = 1, MaterialId = 1 },
				new MaterialProduct { ProductId = 1, MaterialId = 2 },
				new MaterialProduct { ProductId = 2, MaterialId = 2 }
			);

			// Seeding Sales
			modelBuilder.Entity<Sale>().HasData(
                new Sale { Id = 1, UserId = 2, ProductId = 1, Amount = 1, Description = "Bought for living room", Status = Sale.StatusType.Ordered },
                new Sale { Id = 2, UserId = 2, ProductId = 2, Amount = 2, Description = "For workshop", Status = Sale.StatusType.InProgress }
            );

            // Seeding Reviews
            modelBuilder.Entity<Review>().HasData(
                new Review { Id = 1, UserId = 2, ProductId = 1, Rating = 5, Description = "Great product!" },
                new Review { Id = 2, UserId = 2, ProductId = 2, Rating = 4, Description = "Good quality." }
            );
        }
    }
}
