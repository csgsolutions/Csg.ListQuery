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
            IDictionary<string, ListItemPropertyInfo> properties) where T : new()
        {
            return ToListResponse<T, T>(queryResult, request, properties, s => s);
        }

        public static ListResponse<TDomain> ToListResponse<TInfrastructure, TDomain>(
            this ListQueryResult<TInfrastructure> queryResult,
            IListRequest request,
            IDictionary<string, ListItemPropertyInfo> properties,
            Func<TInfrastructure, TDomain> selector)
        {
            properties = properties ?? throw new ArgumentNullException(nameof(properties));

            IEnumerable<TDomain> data = queryResult.Data.Select(selector);
            int? dataCount = queryResult.IsBuffered ? data.Count() : (int?)null;

            var response = new ListResponse<TDomain>()
            {
                Meta = new ListResponseMeta()
                {
                    TotalCount = queryResult.TotalCount
                }
            };

            if (request.Fields != null)
            {
                response.Meta.Fields = properties.Select(s => s.Value.JsonName).Intersect(request.Fields, StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                response.Meta.Fields = properties.Select(s => s.Value.JsonName);
            }

            if (dataCount.HasValue)
            {
                response.Meta.CurrentCount = dataCount;

                if (queryResult.HasMoreData)
                {
                    var nextOffset = (request.Offset + dataCount.Value);
                    response.Meta.Next = new PageInfo(nextOffset);
                }
            }

            if (request.Offset > 0)
            {
                //TODO: Limit will be zero if the default limit was applied inside ListQuery.
                var prevOffset = Math.Max(request.Offset - request.Limit, 0);
                response.Meta.Prev = new PageInfo(prevOffset);
            }

            response.Data = data;

            return response;
        }

        public static ListResponse<TDomain> ToListResponse<TInfrastructure, TDomain>(
            this ListQueryResult<TInfrastructure> queryResult,
            IListRequest request,
            IDictionary<string, ListItemPropertyInfo> properties,
            Func<TInfrastructure, TDomain> selector,
            System.Uri currentUri
        )
        {
            var response = ToListResponse<TInfrastructure, TDomain>(queryResult, request, properties, selector);

            response.Links = new ListResponseLinks()
            {
                Self = currentUri.AbsoluteUri
            };

           

            if (response.Meta?.Next?.Offset >= 0)
            {
                response.Links.Next = CreateUri(request, currentUri, response.Meta.Next.Value.Offset).ToString();
            }
            
            if (response.Meta?.Prev?.Offset >= 0)
            {
                response.Links.Prev = CreateUri(request, currentUri, response.Meta.Prev.Value.Offset).ToString();
            }

            return response;
        }

        private static Uri CreateUri(IListRequest request, Uri currentUri, int offset)
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
