using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System.Linq;
namespace Presentation.ActionFilters
{
    public class ValidateMediaTypeAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var acceptHeaderPresent = context.HttpContext
                .Request
                .Headers
                .ContainsKey("Accept");

            if (!acceptHeaderPresent)
            {
                context.Result = new BadRequestObjectResult($"Accept headers is missing ");
                return;
            }
            var mediaType = context.HttpContext
                .Request
                .Headers["Accept"]
                .FirstOrDefault();

            if (!MediaTypeHeaderValue.TryParse(mediaType, out MediaTypeHeaderValue? mediaTypeHeaderValue) )
            {
                context.Result = new BadRequestObjectResult($"Media type not present." +
                    $"Please add Accept header with required media type.");

                return;

            }
            context.HttpContext.Items.Add("AcceptHeaderMediaType", mediaTypeHeaderValue);
           
        }
    }
}
