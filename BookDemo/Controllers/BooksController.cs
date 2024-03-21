using BookDemo.Data;
using BookDemo.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;
            return Ok(books);

        }

        //GetOneBook methodu için id parametresi alınması gerekiyor şeklinde ayarladık.
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id" )] int id )
        {
            //Linq sorgusu ile parametreye gönderilen id ile books da ki id ye eşit olan kayıt getirilecek.
            var book = ApplicationContext
                .Books
                .Where(b => b.Id == id)
                .SingleOrDefault();

            //Eğer book da veri yoksa NotFound yani bulunamadı hatası veriyoruz.
            if (book  is null) 
                return NotFound();


            return Ok(book);

        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody]Book book)
        {
            try
            {

                if (book is null)
                    return BadRequest(); //400

                ApplicationContext.Books.Add(book);
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //400
                //throw;
            }

        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute (Name ="id")] int id  , [FromBody] Book book)
        {
            try
            {
                //check book
                var entity = ApplicationContext
                    .Books.Find(b => b.Id == id);

                if (entity is null)
                    return NotFound(); //404

                //check id 
                if (book.Id != id )
                    return BadRequest(); //400

                ApplicationContext.Books.Remove(entity);
                book.Id = entity.Id;
                ApplicationContext.Books.Add(book);

                return Ok(book);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //400
                //throw;
            }

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                //check book
                var entity = ApplicationContext
                    .Books.Find(b => b.Id == id);

                if (entity is null)
                    return NotFound(); //404


                ApplicationContext.Books.Remove(entity);
                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //400
                //throw;
            }

        }

        [HttpPatch("{id:int}")]
        public IActionResult PartialUpdateOneBook([FromRoute(Name = "id")] int id , [FromBody] JsonPatchDocument< Book> bookPatch)
        {
            try
            {
                //check book
                var entity = ApplicationContext
                    .Books.Find(b => b.Id == id);

                if (entity is null)
                    return NotFound(); //404


                bookPatch.ApplyTo(entity);
                return NoContent();    

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //400
                //throw;
            }

        }
    }
}
