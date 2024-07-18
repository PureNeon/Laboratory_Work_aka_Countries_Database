using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace LabWork1.Model
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<Country> Countries { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.json")
				.Build();
			string useConnection = configuration.GetSection("UseConnection").Value ?? "DefaultConnection";
			optionsBuilder.UseNpgsql(configuration.GetConnectionString(useConnection));
		}
	}

}
