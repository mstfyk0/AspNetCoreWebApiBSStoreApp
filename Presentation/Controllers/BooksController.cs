
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using Entities.DataTransferObjects;
using System.Threading.Tasks;
using Presentation.ActionFilters;
using Entities.RequestFeatures;
using System.Text.Json;

namespace Presentation.Controllers
{
    [ServiceFilter(typeof(LogFilterAttribute))]
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpHead]
        [HttpGet(Name = "GetAllBooksAsync")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            //Linq sorgusu ile parametreye gönderilen id ile books da ki id ye eşit olan kayıt getirilecek.
            var book =  await _manager
                .BookService.GetOneBookByIdAsync(id, false);

            return Ok(book);
        }

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
