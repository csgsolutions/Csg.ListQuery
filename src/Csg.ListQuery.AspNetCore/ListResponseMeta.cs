using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.AspNetCore
{
    public class ListResponseMeta
    {
        public int? TotalCount { get; set; }

        public virtual IEnumerable<string> Fields { get; set; }
    }
}
