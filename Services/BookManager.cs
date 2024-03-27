using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using static Entities.Exceptions.BadRequestException;
using static System.Reflection.Metadata.BlobBuilder;

namespace Services
{
    public class BookManager : IBookService
    {

        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<BookDto> _shaper;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper, IDataShaper<BookDto> shaper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
            _shaper = shaper;
        }

        public async Task<BookDto> CreateOneBookAsync (BookDtoForInsertion bookDto)
        {

            //if (book is null)
            //    throw new ArgumentNullException(nameof(book));
            var entity = _mapper.Map<Book>(bookDto);
            _manager.BookRepository.CreateOneBook(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            //var entity = await _manager.BookRepository.GetOneBookByIdAsync(id, trackChanges);
            var entity = await GetOneBookByIdAndCheckExists(id, trackChanges);

            //GetOneBookByIdAndCheckExists
            //if (entity is null)
            //    throw new BookNotFoundException(id);

            _manager.BookRepository.DeleteOneBook(entity);
            await _manager.SaveAsync();
        }

        public  async Task<(IEnumerable<ExpandoObject> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters,bool trackChanges)
        {
            if (!bookParameters.ValidPriceRange)
                throw new PriceOutOfRangeBadRequestException();

            var booksWithMetaData =  await _manager.BookRepository.GetAllBooksAsync(bookParameters, trackChanges);

            var bookDto = _mapper.Map<IEnumerable<BookDto>>(booksWithMetaData);

            var shapedData = _shaper.ShapeData(bookDto, bookParameters.Fields);

            return (books: shapedData, metaData: booksWithMetaData.MetaData);
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {

            //var book = await _manager.BookRepository.GetOneBookByIdAsync(id, trackChanges);
            var book = await GetOneBookByIdAndCheckExists(id, trackChanges);

            //Eğer book da veri yoksa NotFound yani bulunamadı hatası veriyoruz.
            //GetOneBookByIdAndCheckExists functionundan taşındı
            //if (book is null)
            //    throw new BookNotFoundException(id);

            return _mapper.Map<BookDto>(book);

        }

        public async Task<(BookDtoForUpdate bookDtoUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trachChanges)
        {
            var book = await GetOneBookByIdAndCheckExists(id, trachChanges);
            //Kod GetOneBookByIdAndCheckExists functionuna taşındı.
            //if (book is null)
            //    throw new BookNotFoundException(id);

            var bookDtoForUpdate =  _mapper.Map<BookDtoForUpdate>(book);

            return (bookDtoForUpdate, book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _manager.SaveAsync();
        }

        public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
        { 
            var entity = await GetOneBookByIdAndCheckExists(id,false);

            //GetOneBookByIdAndCheckExists functionuna taşındı

            //if (entity is null)
            //    throw new BookNotFoundException(id);

            //if (book is null)
            //    throw new ArgumentNullException(nameof (book)); 


            //entity.Title = book.Title;
            //entity.Price = book.Price;

            entity = _mapper.Map<Book>(bookDto);

            _manager.BookRepository.UpdateOneBook(entity);
            await _manager.SaveAsync();

        }

        private async Task<Book> GetOneBookByIdAndCheckExists(int id , bool trachChanges)
        {
            var entity  = await _manager.BookRepository.GetOneBookByIdAsync (id, trachChanges);

            if (entity is null)
            {
                throw new BookNotFoundException(id);    
            }   
            return entity;

        }
    }
}
