using System.Collections.Generic;
using Csg.ListQuery.AspNetCore.Abstractions;

namespace Csg.ListQuery.AspNetCore
{
    public class PagedListResponse<T> : ListResponse<T>
    {
        public PagedListResponse() : base()
        {
        }

        public PagedListResponse(IListRequest request, IEnumerable<T> data) : base(request, data)
        {
            this.Meta = this.Meta ?? new PagedListResponseMeta();
            this.Meta.Fields = request.Fields;
        }

        public PagedListLinks Links { get; set; }

        public PagedListResponseMeta Meta { get; set; }
    }
}
