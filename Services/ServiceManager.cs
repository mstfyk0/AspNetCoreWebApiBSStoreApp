using AutoMapper;
using Entities.DataTransferObjects;
using Repositories.Contracts;
using Services.Contracts;
using System;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        public ServiceManager(IRepositoryManager repositoryManager
            ,ILoggerService loggerService
            ,IMapper mapper 
            ,IDataShaper<BookDto> shaper
            )
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager, loggerService, mapper, shaper));
        }
        public IBookService BookService => _bookService.Value;
    }
}
