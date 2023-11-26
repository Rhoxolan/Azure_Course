using Newtonsoft.Json;

namespace _2023._05._16_PW.Models
{
	public class Cat
	{
		[JsonProperty("id")]
		public string Id { get; set; } = default!;

		[JsonProperty("name")]
		public string Name { get; set; } = default!;

		[JsonProperty("breed")]
		public string Breed { get; set; } = default!;
	}
}
