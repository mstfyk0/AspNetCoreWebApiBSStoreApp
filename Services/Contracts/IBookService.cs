﻿using Entities.DataTransferObjects;
using Entities.Models;
using Entities.Models.LinkModels;
using Entities.RequestFeatures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IBookService
    {
        Task<(LinkResponse linkResponse, MetaData metaData)> GetAllBooksAsync(LinkParameters linkParameters,bool trackChanges);
        Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges);
        Task<BookDto> CreateOneBookAsync(BookDtoForInsertion book);  
        Task UpdateOneBookAsync(int id ,BookDtoForUpdate bookDto , bool trackChanges);
        Task DeleteOneBookAsync(int id,bool trackChanges);

        Task<(BookDtoForUpdate bookDtoUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trachChanges);

        Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book);
        Task<List<Book>> GetAllBooksAsync(bool trackChanges);
        Task<IEnumerable<Book>> GetAllBooksWithDetailsAsync(bool trackChanges);

    }
}
