using HelloWebApi.Models;
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
        //Response models tipinde dönmesini isteyedebiliriz. 
        //public ResponseModels GetMessage()
        //{

        //    return new ResponseModels()
        //    {
        //        HttpStatus= 200,
        //        Message="Hello world."

        //    };
        //}
        //Her türlü bir action dönmesi için bu arayüz interfaceden de yararlanabiliriz. Apilerde genelde bu kullanılıyor.
        //Apilerin response kodlarını desteklemesi için de IactionResult kullanacağız
        public IActionResult GetMessage()
        {
            var result = new ResponseModels()
            {
                HttpStatus = 200,
                Message = "Hello world."

            }; 

            return Ok(result);
        }

    }
}
