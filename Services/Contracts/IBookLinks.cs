using Entities.DataTransferObjects;
using Entities.Models.LinkModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;



namespace Services.Contracts
{
    public interface IBookLinks
    {
        LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto
            , string fields, HttpContext httpContext);
    }
}
