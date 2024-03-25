using Entities.DataTransferObjects;
using Entities.Models;
using System.Collections.Generic;

namespace Services.Contracts
{
    public interface IBookService
    {
        IEnumerable<BookDto> GetAllBooks(bool trackChanges);
        BookDto GetOneBookById(int id, bool trackChanges);
        BookDto CreateOneBook(BookDtoForInsertion book);  
        void UpdateOneBook(int id ,BookDtoForUpdate bookDto , bool trackChanges);
        void DeleteOneBook(int id,bool trackChanges); 


    }
}
