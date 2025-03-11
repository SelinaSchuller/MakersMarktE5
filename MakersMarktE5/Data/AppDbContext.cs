using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasData(
				new User { Id = 1, Name = "admin", Password = "wachtwoord"}
			);
		}
	}
}
