
using Repositories.Contracts;
using System;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {

        private readonly RepositoriesContext _repositoriesContext;
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public RepositoryManager(RepositoriesContext repositoriesContext, IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _repositoriesContext = repositoriesContext;
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public IBookRepository BookRepository => _bookRepository;

        public ICategoryRepository CategoryRepository => _categoryRepository;

        public async Task SaveAsync()
        {
            await _repositoriesContext.SaveChangesAsync();
        }
    }
}
