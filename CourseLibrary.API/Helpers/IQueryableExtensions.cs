using CourseLibrary.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace CourseLibrary.API.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDict)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (mappingDict == null)
            {
                throw new ArgumentNullException(nameof(mappingDict));
            }

            if (string.IsNullOrEmpty(orderBy))
            {
                return source;
            }

            // the orderBy string is separated by ",", so we split it.
            var splitedOrderBy = orderBy.Split(',');
            string orderByString = "";
            // foreach(var orderByClause in splitedOrderBy.Reverse())
            foreach (var orderByClause in splitedOrderBy)
            {
                // remove leading or trailing spaces.
                var trimmedOrderByClause = orderByClause.Trim();

                // e.g. "orderBy=Name desc"
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");

                // get the orderBy property name by removing "asc" or "desc".
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause
                    : trimmedOrderByClause.Remove(indexOfFirstSpace);

                if (!mappingDict.ContainsKey(propertyName))
                {
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                }

                var propertyMappingValue = mappingDict[propertyName];
                if (propertyMappingValue == null)
                {
                    throw new ArgumentNullException(nameof(propertyMappingValue));
                }

                // A DTO property can be mapped to multiple DB entity properties. e.g. Name => FirstName + LastName
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties)
                {
                    // revert order if needed. E.g. Order by Age asc => order by birthday desc.
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    orderByString = orderByString +
                        (string.IsNullOrWhiteSpace(orderByString) ? String.Empty : ",")
                        + destinationProperty
                        + (orderDescending ? " descending" : " ascending");
                }
            }

            return source.OrderBy(orderByString);
        }
    }
}
