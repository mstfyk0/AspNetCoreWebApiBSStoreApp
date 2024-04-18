
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Entities.DataTransferObjects;
using System.Threading.Tasks;
using Presentation.ActionFilters;
using Entities.RequestFeatures;
using System.Text.Json;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.Serialization;

namespace Presentation.Controllers
{
    //versiyon tanımlamayı controlün başına attribute olarak yazılabilir  [ApiVersion("1.0")] fakat bunu Conventsions yapısı ile de yazabiliriz.
    //[ApiVersion("1.0")]
    [ServiceFilter(typeof(LogFilterAttribute))]
    //Versiyonlamayı link üzerinden de verebiliriz. 
    //[Route("api/{v:apiversion}/books")]
    //Header üzerinde nde 
    [Route("api/books")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]

    //Cache profile kontrollera tanımlama.
    //[ResponseCache(CacheProfileName ="5mins")]
    //Service extensionda HTTP cache response ile merkezi yerden kotnrol edilmesi sağlandığı için bu tanımlara ihtiyaç yok.
    //[HttpCacheExpiration(CacheLocation = CacheLocation.Public , MaxAge =80)]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }
        //KUllanıcı girişi olmadan bu method kullanılmaz özelliği kazandırmış oluyoruz. [Authorize] 
        [Authorize]
        [HttpHead]
        [HttpGet(Name = "GetAllBooksAsync")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        //[ResponseCache(Duration =60)]
        public async Task<IActionResult> GetAllBooksAsync([FromQuery] BookParameters bookParameters)
         {
            var linkParameter = new LinkParameters()
            {

                BookParameters = bookParameters,
                HttpContext = HttpContext
            };

            var result = await _manager.BookService.GetAllBooksAsync(linkParameter, false);
            Response.Headers.Add("X-Pagination",
                JsonSerializer.Serialize(result.metaData));

            return result.linkResponse.HasLinks ?
                Ok(result.linkResponse.LinkedEntities) :
                Ok(result.linkResponse.ShapedEntities);

        }

        [Authorize]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            //Linq sorgusu ile parametreye gönderilen id ile books da ki id ye eşit olan kayıt getirilecek.
            var book =  await _manager
                .BookService.GetOneBookByIdAsync(id, false);

            return Ok(book);
        }

        //[Authorize]
        //[DataContract(IsReference = true)]
        [HttpGet("details")]
        public async Task<IActionResult> GetAllBooksWithDetailsAsync()
        {
            return Ok(await _manager.BookService.GetAllBooksWithDetailsAsync(false));
        }

        [Authorize(Roles = "Admin, Editpor ")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost(Name = "CreateOneBookAsync")]  
        public async Task<IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion book)
        {
            //Action filer yapısını kurmamızla ve IoC kaydını yapmamızla bu kontrolleri action filter class larında yapıyoruz.
            //Bu sebeple burdaki bu kontrollere ihtiyacımız kalmıyor.
            //[ServiceFilter(typeof(ValidationFilterAttribute))]
            //if (book is null)
            //    return BadRequest(); //400

            ////varsayılan davranış değerini değiştiriyorız.
            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState);

            await _manager.BookService.CreateOneBookAsync(book);
            return StatusCode(201, book);

        }

        [Authorize(Roles = "Admin, Editpor ")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            //[ServiceFilter(typeof(ValidationFilterAttribute))]
            //if (bookDto is null)
            //    return BadRequest();

            ////varsayılan davranış değerini değiştiriyorız.
            //if (!ModelState.IsValid)
            //    return UnprocessableEntity(ModelState);

            await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
            return Ok(bookDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBookAsync([FromRoute(Name = "id")] int id)
        {
            await _manager.BookService.DeleteOneBookAsync(id, false);
            //_manager.Save();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartialUpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if (bookPatch is null)
            {
                return BadRequest();
            }
            var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);

            bookPatch.ApplyTo(result.bookDtoUpdate, ModelState);

            TryValidateModel(result.bookDtoUpdate);

            //varsayılan davranış değerini değiştiriyorız.
            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoUpdate, result.book);
            return NoContent();
        }
        [HttpOptions]

        public IActionResult GetBooksOptions()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, PATCH, DELETE, HEAD, OPTIONS");
            return Ok();
        }
    }
}
