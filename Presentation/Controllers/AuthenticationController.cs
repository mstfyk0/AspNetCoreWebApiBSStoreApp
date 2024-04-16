using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("/api/aut")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IServiceManager _services;

        public AuthenticationController(IServiceManager services)
        {
            _services = services;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public  async Task<IActionResult> RegisterUser([FromBody]UserForRegistrationDto userForRegistrationDto)
        {
            var result = await _services.AuthenticationService.RegisterUser(userForRegistrationDto);


            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                    return BadRequest(ModelState);
                }
            }
            return StatusCode(200);
        }
    }
}
