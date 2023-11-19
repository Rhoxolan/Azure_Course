using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace _2023._05._12_PW
{
	internal class Program
	{
		private static IConfigurationRoot configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();

		static async Task Main(string[] args)
		{
			Database database = await CreateAndGetDatabaseAsync();
			Container container = await CreateAndGetContainerAsync(database);
			var entity = new Entity
			{
				Id = Guid.NewGuid().ToString(),
				Name = "Test"
			};
			var item = await container.CreateItemAsync(item: entity, partitionKey: new(entity.Name));
			Console.WriteLine($"The element with Id: {item.Resource.Id}" +
				$" added to the database. " +
				$"Complexity of the operation: {item.RequestCharge}");
			string query = "SELECT * FROM c WHERE c.Name = 'Test'";
			QueryDefinition queryDefinition = new(query);
			FeedIterator<Entity> feedIterator = container.GetItemQueryIterator<Entity>(queryDefinition);
			var entities = new List<Entity>();
			while (feedIterator.HasMoreResults)
			{
				FeedResponse<Entity> feedResponse = await feedIterator.ReadNextAsync();
				foreach(var responsedEntity in feedResponse)
				{
					entities.Add(responsedEntity);
				}
			}
			foreach (var localResponsedEntity in entities)
			{
				Console.WriteLine(localResponsedEntity.Name);
			}
		}


		static async Task<Database> CreateAndGetDatabaseAsync()
		{
			var uri = configuration["COSMOS:URI"];
			var key = configuration["COSMOS:PRIMARYKEY"];
			var cosmosClient = new CosmosClient(uri, key);
			return await cosmosClient.CreateDatabaseIfNotExistsAsync("TestDatabase");
		}

		static async Task<Container> CreateAndGetContainerAsync(Database database)
		{
			return await database.CreateContainerIfNotExistsAsync(id: "TestContainer", partitionKeyPath: "/Name");
		}
	}
}