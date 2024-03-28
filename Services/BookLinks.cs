﻿using Entities.DataTransferObjects;
using Entities.Models;
using Entities.Models.LinkModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Net.Http.Headers;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class BookLinks : IBookLinks
    {

        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<BookDto> _shaper;

        public BookLinks(LinkGenerator linkGenerator, IDataShaper<BookDto> shaper)
        {
            _linkGenerator = linkGenerator;
            _shaper = shaper;
        }

        public LinkResponse TryGenerateLinks(IEnumerable<BookDto> booksDto, string fields, HttpContext httpContext)
        {
            var shapedBooks = ShapeData(booksDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkedBooks(booksDto, fields, httpContext, shapedBooks);


            return ReturnShapedBooks(shapedBooks);
        }

        private LinkResponse ReturnLinkedBooks(IEnumerable<BookDto> booksDto, string fields, HttpContext httpContext, List<Entity> shapedBooks)
        {
            var bookDtoList= booksDto.ToList();

            for (int index = 0; index < bookDtoList.Count(); index++)
            {
                var bookLinks = CreateForBook(httpContext, bookDtoList[index], fields);
                shapedBooks[index].Add("Links", bookLinks);
            }
            var bookCollection = new LinkCollectionWrapper<Entity>(shapedBooks);
            return new LinkResponse { HasLinks = true, LinkedEntities = bookCollection };
        }

        private List<Link> CreateForBook(HttpContext httpContext, BookDto bookDto, string fields)
        {
            var links = new List<Link>()
            {

                new Link("a1","b1","c1"),
                new Link("a2","b2","c2")
            }
                ;


            return links;
        }

        private LinkResponse ReturnShapedBooks(List<Entity> shapedBooks)
        {
            return new LinkResponse() { ShapedEntities = shapedBooks }; 
        }

        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];
            return mediaType.SubTypeWithoutSuffix
                .EndsWith("hateoas" , StringComparison.InvariantCultureIgnoreCase)
        }

        private List<Entity> ShapeData(IEnumerable<BookDto> booksDto, string fields)
        {
            return _shaper.ShapeData(booksDto, fields)
                .Select(b=>b.Entity)
                .ToList();  
        }
    }
}
