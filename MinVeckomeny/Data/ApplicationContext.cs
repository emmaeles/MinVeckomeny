using Microsoft.EntityFrameworkCore;

namespace MinVeckomeny.Data
{
	public class ApplicationContext : DbContext
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
		}

		public DbSet<Recipe> Recipes { get; set; }
		public DbSet<Ingredient> Ingredients { get; set; }
		public DbSet<Ingredients2Recipes> Ingredients2Recipes { get; set; }
		public DbSet<Hashtag> Hashtags { get; set; }
		public DbSet<Hashtags2Recipes> Hashtags2Recipes { get; set; }
	}
}
