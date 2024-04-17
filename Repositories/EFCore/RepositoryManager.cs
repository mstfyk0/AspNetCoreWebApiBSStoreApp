
using Repositories.Contracts;
using System;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {

        private readonly RepositoriesContext _repositoriesContext;
        private readonly Lazy<IBookRepository> _bookRepository;
        private readonly Lazy<ICategoryRepository> _categoryRepository;

        public RepositoryManager(RepositoriesContext repositoriesContext )
        {
            _repositoriesContext = repositoriesContext;
            _bookRepository = new Lazy<IBookRepository>(new BookRepository(_repositoriesContext));
            _categoryRepository = new Lazy<ICategoryRepository>(new CategoryRepository(_repositoriesContext));
        }

        public IBookRepository BookRepository => _bookRepository.Value;

        public ICategoryRepository CategoryRepository => _categoryRepository.Value;

        public async Task SaveAsync()
        {
            await _repositoriesContext.SaveChangesAsync();
        }
    }
}
