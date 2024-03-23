using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;


namespace Presentation.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = _manager.BookService.GetAllBooks(false);
            return Ok(books);

        }
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            //Linq sorgusu ile parametreye gönderilen id ile books da ki id ye eşit olan kayıt getirilecek.
            var book = _manager
                .BookService.GetOneBookById(id, false);

            //Eğer book da veri yoksa NotFound yani bulunamadı hatası veriyoruz.
            if (book is null)
            {
                throw new Exception();

                //return NotFound();

            }
            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            if (book is null)
                return BadRequest(); //400

            _manager.BookService.CreateOneBook(book);
            return StatusCode(201, book);

            return BadRequest(); //400

        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            if (book is null)
                return BadRequest();

            // //check book
            // var entity = _manager
            //     .BookService.GetOneBookById(id, true);

            // if (entity is null)
            //     return NotFound(); //404

            // //check id 
            // if (book.Id != id)
            //     return BadRequest(); //400

            //entity.Title = book.Title;
            //entity.Price = book.Price;

            // _manager.BookService.UpdateOneBook(id,entity,true);
            // //_manager.Save/*();*/

            _manager.BookService.UpdateOneBook(id, book, true);


            return Ok(book);

        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            _manager.BookService.DeleteOneBook(id, false);
            //_manager.Save();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartialUpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            //check book
            var entity = _manager
                .BookService.GetOneBookById(id, true);

            if (entity is null)
                return NotFound(); //404


            bookPatch.ApplyTo(entity);
            _manager.BookService.UpdateOneBook(id, entity, true);
            return NoContent();
        }
    }
}
