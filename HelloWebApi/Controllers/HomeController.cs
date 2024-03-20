using Microsoft.AspNetCore.Mvc;

namespace HelloWebApi.Controllers
{
    //Api controller kodu api desteklemesi için yazılmıştır.
    [ApiController]
    //Yönlendirme mekanizmasını home adı altına adreslemek için yazılmıştır.
    [Route("home")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public String GetMessage()
        {

            return "Hello world.";
        }

    }
}
