using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.Data.ListQuery.Abstractions
{
    public class ListQueryDefinition
    {
        public virtual IEnumerable<string> Selections { get; set; }

        public virtual IEnumerable<ListQueryFilter> Filters { get; set; }

        public virtual IEnumerable<ListQuerySort> Sort { get; set; }
    }
}
