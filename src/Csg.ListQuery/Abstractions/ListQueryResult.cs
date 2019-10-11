using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csg.ListQuery.Abstractions
{
    public class ListQueryResult<T>
    {
        public ListQueryResult()
        {

        }

        public ListQueryResult(IEnumerable<T> data) : this()
        {
            this.Data = data;
        }

        public virtual IEnumerable<T> Data { get; set; }

        public int? TotalCount { get; set; }
    }
}
