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
        private readonly IBookService _bookService;
        private readonly ICategoryService _categoryService;
        private readonly IAuthenticationService _authenticationServices;

        public ServiceManager(IBookService bookService, ICategoryService categoryService, IAuthenticationService authenticationServices)
        {
            _bookService = bookService;
            _categoryService = categoryService;
            _authenticationServices = authenticationServices;
        }

        public IBookService BookService => _bookService;

        public IAuthenticationService AuthenticationService => _authenticationServices;

        public ICategoryService CategoryService => _categoryService;
    }
}
