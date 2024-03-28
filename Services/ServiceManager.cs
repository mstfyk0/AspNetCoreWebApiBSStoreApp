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
            ,IBookLinks bookLinks
            )
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager, loggerService, mapper, bookLinks));
        }
        public IBookService BookService => _bookService.Value;
    }
}
