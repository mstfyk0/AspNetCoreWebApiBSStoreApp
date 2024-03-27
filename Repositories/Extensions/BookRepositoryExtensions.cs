﻿using Entities.Models;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Linq.Dynamic.Core;
namespace Repositories.Extensions
{
    public static class BookRepositoryExtensions
    {

        public static IQueryable<Book> FilterBooks(this IQueryable<Book> books, uint minPrice, uint maxPrice) =>
            books.Where(book => book.Price >= minPrice && book.Price <= maxPrice);

        public static   IQueryable<Book> Search (this IQueryable<Book> books , string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return books;

            var lowerCaseTerm= searchTerm.Trim().ToLower();

            return books
                .Where(b => b.Title
                .ToLower()
                .Contains(lowerCaseTerm)
                );
        }

        public static IQueryable<Book> Sort(this IQueryable<Book> books, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return books.OrderBy(b=>b.Id);

            var parameters = orderByQueryString.Trim().Split(',');

            var propertyInfos =  typeof(Book).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBuilder = new StringBuilder();

            foreach (var param in parameters)
            {
                if(string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(' ')[0];

                if (propertyInfos is null )
                    continue;

                var objectPropery = propertyInfos
                    .FirstOrDefault( pi => pi.Name.Equals(propertyFromQueryName
                    ,StringComparison.InvariantCultureIgnoreCase));
                
                if (objectPropery is null)
                {
                    continue;
                }
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectPropery.Name.ToString()} {direction},");
            }

            var orderQuery  = orderQueryBuilder.ToString().TrimEnd(','); 

            if (string.IsNullOrWhiteSpace(orderQuery))
                return books.OrderBy(b=> b.Id);

            return books.OrderBy(orderQuery); 
        } 

    }
}
