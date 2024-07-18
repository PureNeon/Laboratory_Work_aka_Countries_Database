using LabWork1.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Diagnostics.Metrics;

namespace LabWork1
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			builder.Services.AddDbContext<ApplicationDbContext>();


			var app = builder.Build();


			app.MapGet("/", () => "Server is ruining");
			app.MapGet("/ping", () => "pong");

			app.MapGet("/country", async (ApplicationDbContext db) => await db.Countries.ToListAsync());
			app.MapGet("/country/code={code}", async (ApplicationDbContext db, string code) => await GetByCode(db, code));
			app.MapGet("/country/id={id}", async (ApplicationDbContext db, int id) => await GetById(db, id));
			app.MapPatch("/country/id={id}", async (Country country, ApplicationDbContext db, int id) => await PatchById(country, db, id));
			app.MapPost("/country", async (Country country, ApplicationDbContext db) => await AddCountry(country, db));

			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;

				var context = services.GetRequiredService<ApplicationDbContext>();
				if (context.Database.GetPendingMigrations().Any())
				{
					context.Database.Migrate();
				}
			}

			app.Run();
		}

		public static async Task<Country?> AddCountry(Country country, ApplicationDbContext db)
		{
			if (country.ISO31661Alpha2Code.Length != 2 || country.ISO31661Alpha3Code.Length != 3 || country.ISO31661NumericCode.Length != 3) return null;


			foreach (char sym in country.ISO31661Alpha2Code) { if (!Char.IsAsciiLetter(sym)) return null; }

			foreach (char sym in country.ISO31661Alpha3Code) { if (!Char.IsAsciiLetter(sym)) return null; }

			foreach (char sym in country.ISO31661NumericCode) { if (!Char.IsNumber(sym)) return null; }

			Country? containerForCheckingCountryCodeRepeats = null;

			containerForCheckingCountryCodeRepeats = await db.Countries.FirstOrDefaultAsync(countrye => countrye.ISO31661Alpha2Code == country.ISO31661Alpha2Code);
			if (containerForCheckingCountryCodeRepeats != null) return null;

			containerForCheckingCountryCodeRepeats = await db.Countries.FirstOrDefaultAsync(countrye => countrye.ISO31661Alpha3Code == country.ISO31661Alpha3Code);
			if (containerForCheckingCountryCodeRepeats != null) return null;

			containerForCheckingCountryCodeRepeats = await db.Countries.FirstOrDefaultAsync(countrye => countrye.ISO31661NumericCode == country.ISO31661NumericCode);
			if (containerForCheckingCountryCodeRepeats != null) return null;

			await db.Countries.AddAsync(country);
			await db.SaveChangesAsync();
			return country;
		}


		public static async Task<Country?> GetByCode(ApplicationDbContext db, string code)
		{
			Country? country = new Country();
			country = await db.Countries.FirstOrDefaultAsync(countrye => countrye.ISO31661Alpha2Code == code);
			if (country != null) return country;
			country = await db.Countries.FirstOrDefaultAsync(countrye => countrye.ISO31661Alpha3Code == code);
			if (country != null) return country;
			country = await db.Countries.FirstOrDefaultAsync(countrye => countrye.ISO31661NumericCode == code);
			if (country != null) return country;
			return null;
		}
		public static async Task<Country?> GetById(ApplicationDbContext db, int id)
		{
			Country? country = await db.Countries.FirstOrDefaultAsync(countrye => countrye.Id == id);
			return country;
		}

		public static async Task<Country?> PatchById(Country country, ApplicationDbContext db, int id)
		{
			if (country.ISO31661Alpha2Code.Length != 2 || country.ISO31661Alpha3Code.Length != 3 || country.ISO31661NumericCode.Length != 3) return null;


			foreach (char sym in country.ISO31661Alpha2Code) { if (!Char.IsAsciiLetter(sym)) return null; }

			foreach (char sym in country.ISO31661Alpha3Code) { if (!Char.IsAsciiLetter(sym)) return null; }

			foreach (char sym in country.ISO31661NumericCode) { if (!Char.IsNumber(sym)) return null; }

			Country? containerForCheckingCountryCodeRepeats = null;

			containerForCheckingCountryCodeRepeats = await db.Countries.FirstOrDefaultAsync(countrye => countrye.ISO31661Alpha2Code == country.ISO31661Alpha2Code);
			if (containerForCheckingCountryCodeRepeats != null) return null;

			containerForCheckingCountryCodeRepeats = await db.Countries.FirstOrDefaultAsync(countrye => countrye.ISO31661Alpha3Code == country.ISO31661Alpha3Code);
			if (containerForCheckingCountryCodeRepeats != null) return null;

			containerForCheckingCountryCodeRepeats = await db.Countries.FirstOrDefaultAsync(countrye => countrye.ISO31661NumericCode == country.ISO31661NumericCode);
			if (containerForCheckingCountryCodeRepeats != null) return null;

			Country? country2 = await db.Countries.FirstOrDefaultAsync(countrye => countrye.Id == id);
			if (country2 == null)
			{
				return null;
			}
			country2.Name = country.Name;
			country2.ISO31661Alpha2Code = country.ISO31661Alpha2Code;
			country2.ISO31661Alpha3Code = country.ISO31661Alpha3Code;
			country2.ISO31661NumericCode = country.ISO31661NumericCode;
			await db.SaveChangesAsync();
			return country2;
		}
	}
}
