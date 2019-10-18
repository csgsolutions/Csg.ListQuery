using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore.Abstractions
{
    public class PagedListHateoasResponse<T> : PagedListResponse<T>, IPagedListHateaosResponse<T>
    {
        public PagedListHateoasResponse() : base()
        {
        }

        public PagedListHateoasResponse(IListRequest request, IEnumerable<T> data) : base(request, data)
        {
        }

        public virtual PagedListLinks Links { get; set; }

    }
}
