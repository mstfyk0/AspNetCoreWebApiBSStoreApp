using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BookManager : IBookService
    {

        private readonly IRepositoryManager _manager;

        public BookManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public Book CreateOneBook(Book book)
        {

            if (book is null)
                throw new ArgumentNullException(nameof(book));

            _manager.BookRepository.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            var entity = _manager.BookRepository.GetOneBookById(id, trackChanges);

            if (entity is null)
                throw new Exception($"Book with id: {id} could not found. ");

            _manager.BookRepository.DeleteOneBook(entity);
            _manager.Save();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.BookRepository.GetAllBooks(trackChanges);
        }

        public Book GetOneBookById(int id, bool trackChanges)
        {
            return _manager.BookRepository.GetOneBookById(id, trackChanges);
        }

        public void UpdateOneBook(int id, Book book, bool trackChanges)
        {
            var entity = _manager.BookRepository.GetOneBookById(id,false);

            if (entity is null)
                throw new Exception($"Book with id: {id} could not found. ");

            if (book is null)
                throw new ArgumentNullException(nameof (book)); 


            entity.Title = book.Title;
            entity.Price = book.Price;

            _manager.BookRepository.UpdateOneBook(entity);
            _manager.Save();

        }
    }
}
