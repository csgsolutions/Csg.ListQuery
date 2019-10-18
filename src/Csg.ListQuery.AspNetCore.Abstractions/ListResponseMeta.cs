using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.ListQuery.AspNetCore.Abstractions
{
    public class ListResponseMeta
    {
        public virtual int? TotalCount { get; set; }

        public virtual IEnumerable<string> Fields { get; set; }
    }
}
