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

		public async Task<Cat> AddAsync(Cat cat)
		{
			Cat newCat = await _container.CreateItemAsync(cat, new PartitionKey(cat.Name));
			return newCat;
		}

		public async Task DeleteAsync(string id, string name)
		{
			await _container.DeleteItemAsync<Cat>(id, new PartitionKey(name));
		}

		public async Task<IEnumerable<Cat>> GetAsync(string sqlCosmosQuery)
		{
			FeedIterator<Cat> query = _container.GetItemQueryIterator<Cat>(new QueryDefinition(sqlCosmosQuery));
			List<Cat> cats = new();
			while (query.HasMoreResults)
			{
				FeedResponse<Cat> response = await query.ReadNextAsync();
				cats.AddRange(response);
			}
			return cats;
		}

		public async Task<Cat> UpdateAsync(Cat cat)
		{
			Cat modifiedCat = await _container.UpsertItemAsync(cat, new PartitionKey(cat.Name));
			return modifiedCat;
		}
	}
}
