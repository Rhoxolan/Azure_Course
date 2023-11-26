using _2023._05._16_PW.Infrastructure.Services.CatCosmosService;
using Microsoft.AspNetCore.Http;
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
	}
}
