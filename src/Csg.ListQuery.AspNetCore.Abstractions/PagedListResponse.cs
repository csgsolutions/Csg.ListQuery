using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore.Abstractions
{
    public class PagedListResponse<T> : ListResponse<T>, IPagedListResponse<T>
    {
        public PagedListResponse() : base()
        {
        }

        public PagedListResponse(IListRequest request, IEnumerable<T> data) : base(request, data)
        {
            this.Meta = this.Meta ?? new PagedListResponseMeta();
            this.Meta.Fields = request.Fields;
        }

        public virtual PagedListResponseMeta Meta { get; set; }
    }
}
