﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Extensions
{
    public static class OrderQueryBuilder
    {
        //<T> ifadesi farklı farklı sınıflarıda dahil edilebilmesini sağlamaktadır.
        public static String CreateOrderQuery<T>(String orderByQueryString)
        {
            var parameters = orderByQueryString.Trim().Split(',');

            var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);


            var orderQueryBuilder = new StringBuilder();



            foreach (var param in parameters)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split(' ')[0];

                if (propertyInfos is null)
                    continue;

                var objectPropery = propertyInfos
                    .FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName
                    , StringComparison.InvariantCultureIgnoreCase));

                if (objectPropery is null)
                {
                    continue;
                }
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";

                orderQueryBuilder.Append($"{objectPropery.Name.ToString()} {direction},");
            }


            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',');

            return orderQuery;
        }
    }
}
