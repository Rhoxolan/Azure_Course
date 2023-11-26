using _2023._05._16_PW.Models;
using Microsoft.Azure.Cosmos;

namespace _2023._05._16_PW.Infrastructure.Services.CatCosmosService
{
	public class CatsCosmosService : ICatsCosmosService
	{
		private readonly Container _container;

		public CatsCosmosService(CosmosClient cosmosClient, string databaseName, string containerName)
		{
			_container = cosmosClient.GetContainer(databaseName, containerName);
		}

		public Task<IEnumerable<Cat>> GetAsync(string sqlCosmosQuery)
		{
			throw new NotImplementedException();
		}
	}
}
