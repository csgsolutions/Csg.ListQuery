using System.Collections.Generic;

namespace Csg.ListQuery.AspNetCore
{
    public class PagedListResponseMeta
    {
        public PageInfo? Next { get; set; }

        public PageInfo? Prev { get; set; }

        public int? CurrentCount { get; set; }

        public int? TotalCount { get; set; }

        public virtual IEnumerable<string> Fields { get; set; }
    }
}
