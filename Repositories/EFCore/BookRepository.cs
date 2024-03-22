using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public IQueryable<Book> GetAllBooks(bool trackchanges) => FindAll(trackchanges);

        public Book GetOneBookById(int id, bool trackchanges) => FindByCondition(b=> b.Id==id 
        , trackchanges).SingleOrDefault();

        public void UpdateOneBook(Book book) => Update(book);
    }
}
