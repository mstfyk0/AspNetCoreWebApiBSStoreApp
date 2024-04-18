using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.EFCore.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.EFCore
{
    public sealed class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoriesContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book) => Create(book);

        public void DeleteOneBook(Book book) => Delete(book);

        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackchanges)
        { 
            var books= await FindAll(trackchanges)
                .FilterBooks(bookParameters.minPrice, bookParameters.maxPrice)
                .Search(bookParameters.SearchTerm) 
            .Sort(bookParameters.OrderBy)
            .ToListAsync();

            return PagedList<Book>.ToPagedList(books,bookParameters.pageNumber,bookParameters.pageSize);
        }

        public async Task<List<Book>> GetAllBooksAsync(bool trackChanges)
        {
            return await FindAll(trackChanges)
                 .OrderBy(b => b.Id)
                 .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges)
        {
            return await _context.Books.
                Include(b => b.Category)
                .OrderBy(b => b.Id)
                .ToListAsync();
                
        }

        public async Task<Book> GetOneBookByIdAsync(int id, bool trackchanges) => await FindByCondition(b=> b.Id==id 
        , trackchanges).SingleOrDefaultAsync();

        public void UpdateOneBook(Book book) => Update(book);
    }
}
