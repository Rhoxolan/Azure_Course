using _2023._04._28_PW.Models;
using _2023._04._28_PW.Services.BlobService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace _2023._04._28_PW.Controllers
{
	public class HomeController : Controller
	{
		private IBlobService _blobService;

		public HomeController(IBlobService blobService)
		{
			_blobService = blobService;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost("blob")]
		public async Task<IActionResult> PostBlob(UploadBlobViewModel uploadBlobViewModel)
		{
			try
			{
				if (uploadBlobViewModel.Blob is null)
				{
					return UnprocessableEntity();
				}
				await _blobService.AddBlobAsync(uploadBlobViewModel.Blob);
				return Ok();
			}
			catch
			{
				return Problem("Data processing error. Please contact to developer");
			}
		}

		[HttpGet("blob/{name?}")]
		public IActionResult GetBlob(SearchBlobViewModel searchBlobViewModel)
		{
			try
			{
				if (string.IsNullOrEmpty(searchBlobViewModel.Name))
				{
					return NotFound();
				}
				var path = _blobService.GetBlobUrl(searchBlobViewModel.Name);
				if (string.IsNullOrEmpty(path))
				{
					return NotFound();
				}
				return Content(path);
			}
			catch
			{
				return Problem("Data processing error. Please contact to developer");
			}
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