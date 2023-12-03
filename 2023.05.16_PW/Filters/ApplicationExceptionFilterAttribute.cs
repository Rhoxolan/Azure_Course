using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _2023._05._16_PW.Filters
{
	public class ApplicationExceptionFilterAttribute : ExceptionFilterAttribute
	{
		public override void OnException(ExceptionContext context)
		{
			if (!context.ExceptionHandled)
			{
				context.Result = new ObjectResult("Problem! Please contact to developer!")
				{
					StatusCode = 500
				};
				context.ExceptionHandled = true;
			}
		}
	}
}
