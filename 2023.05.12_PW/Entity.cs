namespace _2023._05._12_PW
{
	public class Entity
	{
		[Newtonsoft.Json.JsonProperty(PropertyName = "id")]
		public required string Id { get; set; }

		public required string Name { get; set; }
	}
}