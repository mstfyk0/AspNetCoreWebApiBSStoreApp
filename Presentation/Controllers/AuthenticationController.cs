using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Linq.Dynamic.Core.Tokenizer;
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

        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]

        public async Task<IActionResult> Authenticate([FromBody]UserAuthenticationDto userAuthenticationDto)
        {

            if (!await _services.AuthenticationService.ValidateUser(userAuthenticationDto))
            {
                return Unauthorized();
            }
            var tokenDto = await _services.AuthenticationService.CreateToken(true);

            return Ok(tokenDto);

        }

        [HttpPost("refresh")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
            
        public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
        {
            var tokenDtoToReturn = await _services.AuthenticationService.RefreshToken(tokenDto);

            return Ok(tokenDtoToReturn);
        }
    }
}
