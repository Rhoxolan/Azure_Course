using _2023._05._03_PW.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace _2023._05._03_PW.Data.Contexts
{
	public class MessagesDataContext : DbContext
	{
		public MessagesDataContext(DbContextOptions<MessagesDataContext> options) : base(options)
		{
			Database.EnsureCreated();
		}

		public DbSet<MessageDataEntity> MessageDataEntities { get; set; }
	}
}
