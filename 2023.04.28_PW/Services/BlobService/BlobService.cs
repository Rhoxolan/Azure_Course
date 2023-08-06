using _2023._04._28_PW.Data.Contexts;
using _2023._04._28_PW.Data.Entities;
using Azure.Storage.Blobs;

namespace _2023._04._28_PW.Services.BlobService
{
	public class BlobService : IBlobService
	{
		private readonly BlobServiceClient _blobServiceClient;
		private readonly ImagesContext _context;
		private readonly IConfiguration _configuration;

		public BlobService(BlobServiceClient blobServiceClient, ImagesContext context, IConfiguration configuration)
		{
			_blobServiceClient = blobServiceClient;
			_context = context;
			_configuration = configuration;
		}

		public async Task AddBlobAsync(IFormFile blob)
		{
			BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_configuration["Container:DefaultContainerName"]);
			await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
			await containerClient.CreateIfNotExistsAsync();
			BlobClient blobClient = containerClient.GetBlobClient(blob.FileName);
			using var uploadedBlob = blob.OpenReadStream();
			await blobClient.UploadAsync(uploadedBlob, overwrite: true);
			BlobEntity blobEntity = new BlobEntity
			{
				NameKey = blob.FileName,
				Path = $"https://{containerClient.AccountName}.blob.core.windows.net/{containerClient.Name}/{blob.FileName}"
			};
			_context.ImageEntities.Add(blobEntity);
			await _context.SaveChangesAsync();
		}

		public string? GetBlobUrl(string NameKey)
		{
			var blobEntity = _context.ImageEntities.Where(e => e.NameKey == NameKey).FirstOrDefault();
			if (blobEntity == null)
			{
				return null;
			}
			return blobEntity.Path;
		}
	}
}