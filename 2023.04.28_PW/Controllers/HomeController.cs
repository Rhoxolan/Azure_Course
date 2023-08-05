using _2023._04._28_PW.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _2023._04._28_PW.Controllers
{
	public class HomeController : Controller
	{
		private readonly BlobServiceClient _blobServiceClient;

		public HomeController( BlobServiceClient blobServiceClient)
		{
			_blobServiceClient = blobServiceClient;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost("blob")]
		public async Task<IActionResult> PostPhile(UploadPhotoViewModel uploadPhotoViewModel)
		{
			BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("Containers:Home"));
			await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
			await containerClient.CreateIfNotExistsAsync();
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