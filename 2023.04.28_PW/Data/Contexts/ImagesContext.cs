using _2023._04._28_PW.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace _2023._04._28_PW.Data.Contexts
{
	public class ImagesContext : DbContext
	{
		public ImagesContext(DbContextOptions<ImagesContext> options) : base(options)
		{
			Database.EnsureCreated();
		}

		public DbSet<ImageEntity> ImageEntities { get; set; }
	}
}
