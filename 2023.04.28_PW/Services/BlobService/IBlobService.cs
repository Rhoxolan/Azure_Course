namespace _2023._04._28_PW.Services.BlobService
{
	public interface IBlobService
	{
		Task AddBlobAsync(IFormFile blob);

		string? GetBlobUrl(string NameKey);
	}
}