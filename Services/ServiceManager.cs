using AutoMapper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Repositories.Contracts;
using Services.Contracts;
using System;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<IAuthenticationService> _authenticationServices;
        public ServiceManager(IRepositoryManager repositoryManager
            ,ILoggerService loggerService
            ,IMapper mapper 
            ,IConfiguration configuration
            ,UserManager<User> userManager
            ,IBookLinks bookLinks
            )
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager, loggerService, mapper, bookLinks));
            _authenticationServices= new Lazy<IAuthenticationService>(() => 
            new AuthenticationManager(loggerService,mapper, configuration,userManager
            ));
            _categoryService= new Lazy<ICategoryService>(() => new CategoryManager(repositoryManager, loggerService,mapper));
        }
        public IBookService BookService => _bookService.Value;

        public IAuthenticationService AuthenticationService => _authenticationServices.Value;

        public ICategoryService CategoryService => _categoryService.Value;
    }
}
