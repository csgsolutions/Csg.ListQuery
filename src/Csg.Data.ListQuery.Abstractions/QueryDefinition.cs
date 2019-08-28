using System;
using System.Collections.Generic;
using System.Text;

namespace Csg.Data.ListQuery.Abstractions
{
    public class QueryDefinition
    {
        public IEnumerable<string> Selections { get; set; }

        public IEnumerable<ListQueryFilter> Filters { get; set; }

        public IEnumerable<ListQuerySort> Sorts { get; set; }
    }
}
