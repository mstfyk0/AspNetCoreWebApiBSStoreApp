using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{

    //Yayınlanmış olan versionu kaldırmaya yarar  Deprecated =true kodu
    //[ApiVersion("2.0")] 
    //versiyon tanımlamayı controlün başına attribute olarak yazılabilir   [ApiVersion("2.0",Deprecated =true)] fakat bunu Conventsions yapısı ile de yazabiliriz.
    //[ApiVersion("2.0",Deprecated =true)]
    [ApiController]
    //Versiyonlamanın link üzerinden olması için ayarlama
    //[Route("api/{v:apiversion}/books")]
    [Route("api/books")]
    public class BooksV2Controller : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksV2Controller(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public  async Task<IActionResult> GetAllBooksAsync()
        {
            var books    = await _manager.BookService.GetAllBooksAsync(false);
            var booksV2 = books.Select(b => new
            {
                Id = b.Id
                ,Title = b.Title     
            });
            return Ok(booksV2);
        }
    }
}
