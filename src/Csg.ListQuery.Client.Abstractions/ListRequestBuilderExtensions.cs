using Csg.ListQuery;
using Csg.ListQuery.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Client
{
    public static class ListRequestBuilderExtensions
    {
        public static ListRequestBuilder Select(this ListRequestBuilder builder, IEnumerable<string> fields)
        {
            builder.Request.Fields = fields.ToList();
            return builder;
        }

        public static ListRequestBuilder Select(this ListRequestBuilder builder, params string[] fields)
        {
            return Select(builder, (IEnumerable<string>)fields);
        }

        //public static ListRequestBuilder Where(this ListRequestBuilder builder, string fieldName, string @operator, string value)
        //{
        //    builder.Request.Filters.Add(new ListQuery.Abstractions.ListQueryFilter()
        //    {
        //        Name = fieldName,
        //        Operator =,
        //        Value = value
        //    });
        //    return builder;
        //}

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

        public static ListRequestBuilder Order(this ListRequestBuilder builder, IEnumerable<string> fields)
        {
            builder.Request.Order = fields.Select(s => new SortField()
            {
                Name = s.TrimStart('-'),
                SortDescending = s.StartsWith("-")
            }).ToList();

            return builder;
        }

        public static ListRequestBuilder Order(this ListRequestBuilder builder, params string[] fields)
        {
            return Order(builder, (IEnumerable<string>)fields);
        }

        public static ListRequestBuilder Limit(this ListRequestBuilder builder, int limit)
        {
            builder.Request.Limit = limit;
            return builder;
        }

        public static ListRequestBuilder Offset(this ListRequestBuilder builder, int offset)
        {
            builder.Request.Offset = offset;
            return builder;
        }

        public static string ToQueryString(this ListRequestBuilder builder)
        {
            return builder.Request.ToQueryString();
        }

        public static System.Uri ToUri(this ListRequestBuilder builder, string baseUrl)
        {
            return new System.Uri(string.Concat(baseUrl, ToQueryString(builder)));
        }
    }
}
