using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoriesContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book) => Create(book);

        public void DeleteOneBook(Book book) => Delete(book);

        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackchanges)
        { 
            var books= await FindByCondition(b=>b.Price>= bookParameters.minPrice && b.Price <= bookParameters.maxPrice ,trackchanges)
            .OrderBy(b => b.Id)
            .ToListAsync();

            return PagedList<Book>.ToPagedList(books,bookParameters.pageNumber,bookParameters.pageSize);
        }

        public async Task<Book> GetOneBookByIdAsync(int id, bool trackchanges) => await FindByCondition(b=> b.Id==id 
        , trackchanges).SingleOrDefaultAsync();

        public void UpdateOneBook(Book book) => Update(book);
    }
}
