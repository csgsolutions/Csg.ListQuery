using Csg.ListQuery;
using Csg.ListQuery.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Client
{
    /// <summary>
    /// Extension methods for creating list requests.
    /// </summary>
    public static class ListRequestBuilderExtensions
    {
        /// <summary>
        /// Sets the <see cref="ListRequest.Fields"/> collection.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static ListRequestBuilder Select(this ListRequestBuilder builder, IEnumerable<string> fields)
        {
            builder.Request.Fields = fields.ToList();
            return builder;
        }

        /// <summary>
        /// Sets the <see cref="ListRequest.Fields"/> collection.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static ListRequestBuilder Select(this ListRequestBuilder builder, params string[] fields)
        {
            return Select(builder, (IEnumerable<string>)fields);
        }

        /// <summary>
        /// Adds a filter to the request builder.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="builder"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ListRequestBuilder Where<TValue>(this ListRequestBuilder builder, string fieldName, TValue value)
        {
            builder.Request.Filters.Add(new ListQuery.ListFilter()
            {
                Name = fieldName,
                Operator = ListFilterOperator.Equal,
                Value = value
            });
            return builder;
        }

        /// <summary>
        /// Adds a filter to the given request builder.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="builder"></param>
        /// <param name="fieldName"></param>
        /// <param name="operator"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ListRequestBuilder Where<TValue>(this ListRequestBuilder builder, string fieldName, ListFilterOperator @operator, TValue value)
        {
            builder.Request.Filters.Add(new ListQuery.ListFilter()
            {
                Name = fieldName,
                Operator = @operator,
                Value = value
            });
            return builder;
        }

        /// <summary>
        /// Sets the order by fields.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static ListRequestBuilder Order(this ListRequestBuilder builder, IEnumerable<string> fields)
        {
            builder.Request.Order = fields.Select(s => new SortField()
            {
                Name = s.TrimStart('-'),
                SortDescending = s.StartsWith("-")
            }).ToList();

            return builder;
        }

        /// <summary>
        /// Sets the order by fields.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        public static ListRequestBuilder Order(this ListRequestBuilder builder, params string[] fields)
        {
            return Order(builder, (IEnumerable<string>)fields);
        }

        /// <summary>
        /// Sets the limit value for the request.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static ListRequestBuilder Limit(this ListRequestBuilder builder, int limit)
        {
            builder.Request.Limit = limit;
            return builder;
        }

        /// <summary>
        /// Sets the offset for the request.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static ListRequestBuilder Offset(this ListRequestBuilder builder, int offset)
        {
            builder.Request.Offset = offset;
            return builder;
        }

        /// <summary>
        /// Gets a query string representing the given query.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static string ToQueryString(this ListRequestBuilder builder)
        {
            return builder.Request.ToQueryString();
        }

        /// <summary>
        /// Gets the underlying request object.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IListRequest ToRequest(this ListRequestBuilder builder)
        {
            return builder.Request;
        }

        /// <summary>
        /// Gets a full URI for the request.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="baseUrl"></param>
        /// <returns></returns>
        public static System.Uri ToUri(this ListRequestBuilder builder, string baseUrl)
        {
            return new System.Uri(string.Concat(baseUrl, ToQueryString(builder)));
        }
    }
}
