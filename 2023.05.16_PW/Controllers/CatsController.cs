using _2023._05._16_PW.Filters;
using _2023._05._16_PW.Infrastructure.Services.CatCosmosService;
using _2023._05._16_PW.Models;
using Microsoft.AspNetCore.Mvc;

namespace _2023._05._16_PW.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[ApplicationExceptionFilter]
	public class CatsController : ControllerBase
	{
		private readonly ICatsCosmosService _catsCosmosService;

		public CatsController(ICatsCosmosService catsCosmosService)
		{
			_catsCosmosService = catsCosmosService;
		}

		[HttpPost]
		public async Task<ActionResult<Cat>> Post(Cat newCat)
		{
			return Ok(await _catsCosmosService.AddAsync(newCat));
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Cat>>> Get()
		{
			return Ok(await _catsCosmosService.GetAsync());
		}

		[HttpPut]
		public async Task<ActionResult<Cat>> Put(Cat updatingCat)
		{
			return Ok(await _catsCosmosService.UpdateAsync(updatingCat));
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(string id, string name)
		{
			await _catsCosmosService.DeleteAsync(id, name);
			return Ok();
		}
	}
}
