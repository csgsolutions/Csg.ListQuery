using System;
using System.Collections.Generic;
using System.Linq;
using Csg.ListQuery.AspNetCore;
using Csg.ListQuery.AspNetCore.Abstractions;
using Csg.ListQuery.Abstractions;

namespace Csg.ListQuery.AspNetCore
{
    public static class ListResponseExtensions
    {
        public static ListResponse<T> ToListResponse<T>(
            this ListQueryResult<T> queryResult,
            IListRequest request,
            IDictionary<string, DomainPropertyInfo> properties) where T : new()
        {
            return ToListResponse<T, T>(queryResult, request, properties, s => s);
        }

        public static ListResponse<TDomain> ToListResponse<TInfrastructure, TDomain>(
            this ListQueryResult<TInfrastructure> queryResult,
            IListRequest request,
            IDictionary<string, DomainPropertyInfo> properties,
            Func<TInfrastructure, TDomain> selector) where TDomain : new()
        {
            var response = new ListResponse<TDomain>(request, queryResult.Data.Select(selector));

            if (request.Fields != null)
            {
                response.Fields = properties.Select(s => s.Value.JsonName).Intersect(request.Fields, StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                response.Fields = properties.Select(s => s.Value.JsonName);
            }

            return response;
        }

        public static PagedListResponse<TDomain> ToListResponse<TInfrastructure, TDomain>(
            this ListQueryResult<TInfrastructure> queryResult,
            IPagedListRequest request,
            IDictionary<string, DomainPropertyInfo> properties,
            Func<TInfrastructure, TDomain> selector,
            System.Uri currentUri
        ) where TDomain : new()
        {
            IEnumerable<TDomain> data = queryResult.Data.Select(selector).ToList();
            int actualCount = data.Count();
            int responseCount = Math.Min(actualCount, request.Limit);

            if (actualCount > request.Limit)
            {
                data = data.Take(request.Limit);
            }

            var response = new PagedListResponse<TDomain>()
            {
                Data = data,
                Links = new PagedListLinks()
                {
                    Self = currentUri.AbsoluteUri
                },
                Meta = new PagedListMeta()
                {
                    CurrentCount = responseCount,
                    TotalCount = queryResult.TotalCount
                }
            };

            if (actualCount > request.Limit)
            {
                var nextOffset = (request.Offset + request.Limit);
                response.Links.Next = CreateUri(request, currentUri, nextOffset).ToString();
                response.Meta.Next = new PageInfo(nextOffset);
            }

            if (request.Offset > 0)
            {
                var prevOffset = Math.Max(request.Offset - request.Limit, 0);
                response.Links.Prev = CreateUri(request, currentUri, prevOffset).ToString();
                response.Meta.Prev = new PageInfo(prevOffset);
            }
            
            if (request.Fields != null)
            {
                response.Fields = properties.Select(s => s.Value.JsonName).Intersect(request.Fields, StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                response.Fields = properties.Select(s => s.Value.JsonName);
            }

            if (queryResult.TotalCount.HasValue)
            {
                response.Meta = new PagedListMeta();
                response.Meta.TotalCount = queryResult.TotalCount;
            }

            return response;
        }

        private static Uri CreateUri(IPagedListRequest request, Uri currentUri, int offset)
        {
            var keyPairs = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(currentUri.Query)
                                    .Where(x => x.Key.ToLower() != "offset")
                                    .Append(new KeyValuePair<string, Microsoft.Extensions.Primitives.StringValues>(nameof(request.Offset).ToLower(), offset.ToString()));

            var queryString = Microsoft.AspNetCore.Http.QueryString.Create(keyPairs).ToString()
                .Replace("%5B", "[")
                .Replace("%5D", "]")
                .Replace("%3A", ":");

            return new Uri(string.Concat(currentUri.GetUriWithoutQueryString(), queryString));
        }

        private static string GetUriWithoutQueryString(this System.Uri uri)
        {
            return $"{uri.Scheme}://{uri.Authority}{uri.AbsolutePath}";
        }
    }
}
