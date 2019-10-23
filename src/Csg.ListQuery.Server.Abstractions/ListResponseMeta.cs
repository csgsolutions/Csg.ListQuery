using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.Server
{
    public class ListResponseMeta
    {
        public virtual PageInfo? Next { get; set; }

        public virtual PageInfo? Prev { get; set; }

        public virtual int? CurrentCount { get; set; }

        public virtual int? TotalCount { get; set; }

        public virtual IEnumerable<string> Fields { get; set; }
    }
}
