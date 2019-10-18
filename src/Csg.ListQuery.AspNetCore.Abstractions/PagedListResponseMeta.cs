using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore.Abstractions
{
    public class PagedListResponseMeta
    {
        public virtual PageInfo? Next { get; set; }

        public virtual PageInfo? Prev { get; set; }

        public virtual int? CurrentCount { get; set; }

        public virtual int? TotalCount { get; set; }

        public virtual IEnumerable<string> Fields { get; set; }
    }
}
