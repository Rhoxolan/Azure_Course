using _2023._05._16_PW.Infrastructure.Services.CatCosmosService;
using _2023._05._16_PW.Models;
using Microsoft.AspNetCore.Mvc;

namespace _2023._05._16_PW.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
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
			newCat.Id = Guid.NewGuid().ToString();
			Cat catResult = await _catsCosmosService.AddAsync(newCat);
			return Ok(catResult);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Cat>>> Get()
		{
			string sqlCosmosQuery = "SELECT * FROM c";
			IEnumerable<Cat> resutl = await _catsCosmosService.GetAsync(sqlCosmosQuery);
			return Ok(resutl);
		}

		[HttpPut]
		public async Task<ActionResult<Cat>> Put(Cat updatingCat)
		{
			Cat catResult = await _catsCosmosService.UpdateAsync(updatingCat);
			return Ok(catResult);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete(string id, string name)
		{
			await _catsCosmosService.DeleteAsync(id, name);
			return Ok();
		}
	}
}
