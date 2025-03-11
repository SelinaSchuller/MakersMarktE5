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

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql(
				ConfigurationManager.ConnectionStrings["BarrocIntens"].ConnectionString,
				ServerVersion.Parse("8.0.30")
				);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}
	}
}
