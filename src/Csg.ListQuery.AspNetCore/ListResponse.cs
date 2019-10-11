using System;
using System.Collections;
using System.Collections.Generic;
using Csg.ListQuery.AspNetCore.Abstractions;

namespace Csg.ListQuery.AspNetCore
{
    public class ListResponse<T> : IListResponse
    {
        public ListResponse()
        {
        }

        public ListResponse(IListRequest request, IEnumerable<T> data)
        {
            this.Fields = request.Fields;
            this.Data = data;
        }

        public virtual IEnumerable<string> Fields { get; set; }

        public virtual IEnumerable<T> Data { get; set; }

        IEnumerable<string> IListResponse.Fields => this.Fields;

        IEnumerable IListResponse.Data => this.Data;

        Type IListResponse.GetDataType() => typeof(T);
    }

    public class PagedListResponse<T> : ListResponse<T>
    {
        public PagedListResponse() : base()
        {
        }

        public PagedListResponse(IListRequest request, IEnumerable<T> data) : base(request, data)
        {

        }

        public PagedListLinks Links { get; set; }

        public PagedListMeta Meta { get; set; }
    }

    public class PagedListLinks
    {
        public string Next { get; set; }
        public string Self { get; set; }
        public string Prev { get; set; }
    }

    public struct PageInfo
    {
        private int _offset;

        public PageInfo(int offset)
        {
            _offset = offset;
        }

        public int Offset => _offset;
    }

    public class PagedListMeta : Dictionary<string, object>
    {
        public PagedListMeta() : base(StringComparer.OrdinalIgnoreCase)
        {

        }

        public PageInfo? Next { get => (PageInfo?)this["next"]; set => this["next"] = value; }
        public PageInfo? Prev { get => (PageInfo?)this["prev"]; set => this["prev"] = value; }
        public int? CurrentCount { get => (int?)this["count"]; set => this["count"] = value; }
        public int? TotalCount { get => (int?)this["total_count"]; set => this["total_count"] = value; }
    }
}
