using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Book>> GetAllBooksAsync(BookParameters bookParameters,bool trackchanges)
            
            => await FindAll(trackchanges)
            .OrderBy(b=>b.Id)
            .Skip((bookParameters.pageNumber-1)*bookParameters.pageSize)
            .Take(bookParameters.pageSize)
            .ToListAsync();

        public async Task<Book> GetOneBookByIdAsync(int id, bool trackchanges) => await FindByCondition(b=> b.Id==id 
        , trackchanges).SingleOrDefaultAsync();

        public void UpdateOneBook(Book book) => Update(book);
    }
}
