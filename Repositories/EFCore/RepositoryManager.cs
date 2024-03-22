
using Repositories.Contracts;

namespace Repositories.EFCore
{
    public class RepositoryManager : IRepositoryManager
    {

        private readonly RepositoriesContext _repositoriesContext;
        private readonly Lazy<IBookRepository> _bookRepository; 

        public RepositoryManager(RepositoriesContext repositoriesContext)
        {
            _repositoriesContext = repositoriesContext;
            _bookRepository = new Lazy<IBookRepository>(new BookRepository(_repositoriesContext));
        }

        public IBookRepository BookRepository => _bookRepository.Value;

        public void Save()
        {
            _repositoriesContext.SaveChanges();
        }
    }
}
