using Csg.ListQuery.AspNetCore.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.AspNetCore.Client
{
    public static class ListRequestBuilderExtensions
    {
        public static ListRequestBuilder Select(this ListRequestBuilder builder, IEnumerable<string> fields)
        {
            throw new NotImplementedException();
        }

        public static ListRequestBuilder Select(this ListRequestBuilder builder, params string[] fields)
        {
            throw new NotImplementedException();
        }

        public static ListRequestBuilder Where(this ListRequestBuilder builder, string fieldName, string @operator, string value)
        {
            throw new NotImplementedException();
        }

        public static ListRequestBuilder Where<TValue>(this ListRequestBuilder builder, string fieldName, TValue value)
        {
            throw new NotImplementedException();
        }

        public static ListRequestBuilder Where<TValue>(this ListRequestBuilder builder, string fieldName, Csg.ListQuery.Abstractions.ListFilterOperator @operator, TValue value)
        {
            throw new NotImplementedException();
        }

        public static ListRequestBuilder Order(this ListRequestBuilder builder, IEnumerable<string> fields)
        {
            throw new NotImplementedException();
        }

        public static ListRequestBuilder Order(this ListRequestBuilder builder, params string[] fields)
        {
            throw new NotImplementedException();
        }

        public static ListRequestBuilder Limit(this ListRequestBuilder builder, int limit)
        {
            throw new NotImplementedException();
        }

        public static ListRequestBuilder Offset(this ListRequestBuilder builder, int offset)
        {
            throw new NotImplementedException();
        }

        public static string ToQueryString(this ListRequestBuilder builder)
        {
            return builder.ToQueryString();
        }

        public static System.Uri ToUri(this ListRequestBuilder builder, string baseUrl)
        {
            throw new NotImplementedException();
        }
    }
}
