
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoriesContext _repositoriesContext;

        public BooksController(RepositoriesContext repositoriesContext)
        {
            _repositoriesContext = repositoriesContext;
        }
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _repositoriesContext.Books.ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }


        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {

            try
            {
                //Linq sorgusu ile parametreye gönderilen id ile books da ki id ye eşit olan kayıt getirilecek.
                var book = _repositoriesContext
                    .Books
                    .Where(b => b.Id == id)
                    .SingleOrDefault();

                //Eğer book da veri yoksa NotFound yani bulunamadı hatası veriyoruz.
                if (book is null)
                    return NotFound();


                return Ok(book);

            }
            catch (Exception ex )
            {

                throw new Exception(ex.Message);
            }
            

        }


        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {

                if (book is null)
                    return BadRequest(); //400

                _repositoriesContext.Books.Add(book);
                _repositoriesContext.SaveChanges(); 
                return StatusCode(201, book);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //400
                //throw;
            }

        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                //check book
                var entity = _repositoriesContext
                    .Books.Where(b => b.Id == id)
                    .SingleOrDefault()
                    ;

                if (entity is null)
                    return NotFound(); //404

                //check id 
                if (book.Id != id)
                    return BadRequest(); //400

               entity.Title = book.Title;
               entity.Price = book.Price;
               _repositoriesContext.SaveChanges();

                return Ok(book);

            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                //check book
                var entity = _repositoriesContext
                    .Books.Where(b => b.Id == id)
                    .SingleOrDefault();
                    ;

                if (entity is null)
                    return NotFound(); //404


                _repositoriesContext.Books.Remove(entity);
                _repositoriesContext.SaveChanges();

                return NoContent();

            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }

        }
        [HttpPatch("{id:int}")]
        public IActionResult PartialUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                //check book
                var entity = _repositoriesContext
                    .Books.Where(b => b.Id == id)
                    .SingleOrDefault();

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
