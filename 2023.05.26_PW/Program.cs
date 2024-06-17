namespace _2023._05._26_PW
{
	internal class Program
	{
		static void Main(string[] args)
		{
			var dto = new AzureTranslateDTO();

			Console.Write("Please, enter the URI: ");
			dto.Uri = Console.ReadLine();

			Console.Write("Please, enter the key: ");
			dto.Key = Console.ReadLine();

			Console.Write("Please, enter the from value: ");
			dto.From = Console.ReadLine();

			Console.Write("Please, enter the to value: ");
			dto.To = Console.ReadLine();

			Console.Write("Please, enter the api version: ");
			dto.ApiVersion = Console.ReadLine();

			Console.Write("Please, enter the thext to translate: ");
			dto.Text = Console.ReadLine();
		}

		static async Task<string> Translate(AzureTranslateDTO dto)
		{
			throw new NotImplementedException();
		}
	}

	internal class AzureTranslateDTO
	{
		public string? Key { get; set; }

		public string? Uri { get; set; }

		public string? From { get; set; }

		public string? To { get; set; }

		public string? ApiVersion { get; set; }

		public string? Text {  get; set; }
	}
}
