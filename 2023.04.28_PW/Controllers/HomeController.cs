using _2023._04._28_PW.Data.Contexts;
using _2023._04._28_PW.Data.Entities;
using _2023._04._28_PW.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _2023._04._28_PW.Controllers
{
	public class HomeController : Controller
	{
		private readonly BlobServiceClient _blobServiceClient;
		private readonly ImagesContext _context;
		private readonly IConfiguration _configuration;


		public HomeController(BlobServiceClient blobServiceClient, ImagesContext context, IConfiguration configuration)
		{
			_blobServiceClient = blobServiceClient;
			_context = context;
			_configuration = configuration;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost("blob")]
		public async Task<IActionResult> PostBlob(UploadBlobViewModel uploadBlobViewModel)
		{
			if(uploadBlobViewModel.Blob is null)
			{
				return UnprocessableEntity();
			}
			BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_configuration["Container:DefaultContainerName"]);
			await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
			await containerClient.CreateIfNotExistsAsync();
			BlobClient blobClient = containerClient.GetBlobClient(uploadBlobViewModel.Blob.FileName);
			using var uploadedBlob = uploadBlobViewModel.Blob.OpenReadStream();
			await blobClient.UploadAsync(uploadedBlob, overwrite: true);
			BlobEntity blobEntity = new BlobEntity
			{
				NameKey = uploadBlobViewModel.Blob.FileName,
				Path = $"https://{containerClient.AccountName}.blob.core.windows.net/{containerClient.Name}/{uploadBlobViewModel.Blob.FileName}"
			};
			_context.ImageEntities.Add(blobEntity);
			await _context.SaveChangesAsync();
			return Ok();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}