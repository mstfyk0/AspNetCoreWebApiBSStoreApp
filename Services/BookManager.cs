using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System.Collections.Generic;

namespace Services
{
    public class BookManager : IBookService
    {

        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public BookDto CreateOneBook(BookDtoForInsertion bookDto)
        {

            //if (book is null)
            //    throw new ArgumentNullException(nameof(book));
            var entity = _mapper.Map<Book>(bookDto);
            _manager.BookRepository.CreateOneBook(entity);
            _manager.Save();
            return _mapper.Map<BookDto>(entity);
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            var entity = _manager.BookRepository.GetOneBookById(id, trackChanges);

            if (entity is null)
                throw new BookNotFoundException(id);

            _manager.BookRepository.DeleteOneBook(entity);
            _manager.Save();
        }

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            var books = _manager.BookRepository.GetAllBooks(trackChanges);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public BookDto GetOneBookById(int id, bool trackChanges)
        {

            var book = _manager.BookRepository.GetOneBookById(id, trackChanges);

            //Eğer book da veri yoksa NotFound yani bulunamadı hatası veriyoruz.
            if (book is null)
                throw new BookNotFoundException(id);

            return _mapper.Map<BookDto>(book);

        }

        public (BookDtoForUpdate bookDtoUpdate, Book book) GetOneBookForPatch(int id, bool trachChanges)
        {
            var book = _manager.BookRepository.GetOneBookById(id, trachChanges);

            if (book is null)
                throw new BookNotFoundException(id);

            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);

            return (bookDtoForUpdate, book);
        }

        public void SaveChangesForPatch(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            _manager.Save();
        }

        public void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            var entity = _manager.BookRepository.GetOneBookById(id,false);

            if (entity is null)
                throw new BookNotFoundException(id);

            //if (book is null)
            //    throw new ArgumentNullException(nameof (book)); 


            //entity.Title = book.Title;
            //entity.Price = book.Price;

            entity= _mapper.Map<Book>(bookDto);

            _manager.BookRepository.UpdateOneBook(entity);
            _manager.Save();

        }
    }
}
