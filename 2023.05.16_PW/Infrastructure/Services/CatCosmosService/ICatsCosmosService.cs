using _2023._05._16_PW.Models;

namespace _2023._05._16_PW.Infrastructure.Services.CatCosmosService
{
	public interface ICatsCosmosService
	{
		Task<IEnumerable<Cat>> GetAsync(string sqlCosmosQuery);
	}
}
